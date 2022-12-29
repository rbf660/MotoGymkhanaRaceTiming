﻿// <copyright file="RaceTracker.cs" company="Moto Gymkhana">
//     Copyright (c) Moto Gymkhana. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models;
using Models.Config;
using SensorUnits.TimingUnit;
using log4net;

namespace RaceManagement
{
    /// <summary>
    /// Class that keeps track of a race through events provided raised by timing and id units
    /// </summary>
    public class RaceTracker : IRaceTracker
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TrackerConfig config;
        /// <summary>
        /// The timing unit that contains the timing gates at the start and stop box
        /// </summary>
        private ITimingUnit timing;

        /// <summary>
        /// The events triggered by riders entering the start box. In FIFO order so the first rider to enter the box is the first to start
        /// </summary>
        private RiderReadyEvent waitingRider;

        /// <summary>
        /// The matched id and time events that indicate a driver has left the start box and is on track. in FIFO order so the first element in this queue represent the rider that entered the track first
        /// </summary>
        private Dictionary<Guid, (RiderReadyEvent id, TimingEvent timer)> onTrackRiders = new Dictionary<Guid, (RiderReadyEvent id, TimingEvent timer)>();

        /// <summary>
        /// The complete list of events in chronological order
        /// </summary>
        private Queue<RaceEvent> raceState = new Queue<RaceEvent>();

        /// <summary>
        /// List of timing events picked up by the end timing gate that have not been matched to an id from <see cref="endIds"/>. Oldest first
        /// </summary>
        private Dictionary<Guid, TimingEvent> endTimes = new Dictionary<Guid, TimingEvent>();

        /// <summary>
        /// Events waiting to be processed by the main loop
        /// </summary>
        private ConcurrentQueue<EventArgs> toProcess = new ConcurrentQueue<EventArgs>();

        private List<Lap> laps = new List<Lap>();

        private Dictionary<Guid, DSQEvent> pendingDisqualifications = new Dictionary<Guid, DSQEvent>();
        private Dictionary<Guid, List<PenaltyEvent>> pendingPenalties = new Dictionary<Guid, List<PenaltyEvent>>();

        private List<Rider> knownRiders = new List<Rider>();

        /// <summary>
        /// Fired when the system is ready for the next rider to trigger the start timing gate
        /// </summary>
        public event EventHandler<WaitingRiderEventArgs> OnRiderWaiting;

        /// <summary>
        /// Fired when a rider's lap time is known
        /// </summary>
        public event EventHandler<LapCompletedEventArgs> OnRiderMatched;

        public event EventHandler<LapCompletedEventArgs> OnRiderDNF;

        /// <summary>
        /// Fired when the system has no riders waiting to start
        /// </summary>
        public event EventHandler OnStartEmpty;

        public RaceTracker(ITimingUnit timing, TrackerConfig config, List<Rider> knownRiders)
        {
            this.timing = timing;
            this.knownRiders = knownRiders;
            this.config = config;

            foreach(Rider rider in knownRiders)
            {
                pendingPenalties.Add(rider.Id, new List<PenaltyEvent>());
            }
        }

        /// <summary>
        /// Gives you an overview of the current state of the race
        /// Do not modify the returned objects
        /// </summary>
        public (RiderReadyEvent waiting, List<(RiderReadyEvent rider, TimingEvent timer)> onTrack, List<TimingEvent> unmatchedTimes) GetState =>
            (waitingRider, onTrackRiders.Values.ToList(), endTimes.Values.ToList());

        /// <summary>
        /// Returns a list of all laps driven so far
        /// </summary>
        public List<Lap> Laps => laps.ToList();

        /// <summary>
        /// Run a task that communicates with the timing and rider units to track the state of a race
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<RaceSummary> Run(CancellationToken token)
        {
            RegisterEvents();

            return Task.Run(() =>
            {
                try
                {
                    while (!(token.IsCancellationRequested && toProcess.Count == 0))
                    {
                        if (toProcess.TryDequeue(out EventArgs e))
                        {
                            switch (e)
                            {
                                case RiderReadyEventArgs rider:
                                    OnRiderReady(rider);
                                    break;
                                case RiderFinishedEventArgs rider:
                                    OnRiderFinished(rider);
                                    break;
                                case TimingTriggeredEventArgs time:
                                    OnTimer(time);
                                    break;
                                case PenaltyEventArgs penalty:
                                    OnPenalty(penalty);
                                    break;
                                case DSQEventArgs dsq:
                                    OnDSQ(dsq);
                                    break;
                                case ManualDNFEventArgs dnf:
                                    OnManualDNF(dnf);
                                    break;
                                default:
                                    throw new ArgumentException($"Unknown event type: {e.GetType()}");
                            }
                        }
                    }

                    if (token.IsCancellationRequested)
                    {
                        Log.Info($"RaceTracker thread ended because cancellation was requested");
                    }
                    else
                    {
                        Log.Info($"RaceTracker thread ended for other reasons");
                    }

                    return new RaceSummary(raceState.ToList(), config);
                }
                catch (Exception ex)
                {
                    Log.Error($"Exception in RaceTracker task: {ex.Message}", ex);
                }

                return new RaceSummary();
            });
        }

        private void RegisterEvents()
        {
            timing.OnTrigger += (_, args) => OnEvent(args);
        }

        private void OnEvent(EventArgs e) => toProcess.Enqueue(e);


        /// <summary>
        /// Triggered by a user when the rider is in the start box and they may start
        /// </summary>
        private void OnRiderReady(RiderReadyEventArgs args)
        {
            Rider ready = knownRiders.Find(r => r.Id == args.RiderId);

            if(ready == null)
            {
                Log.Warn($"Ignoring ready event for rider {args.RiderId}, id unkown");
                return;
            }

            if(onTrackRiders.ContainsKey(ready.Id))
            {
                Log.Warn($"Ignoring ready event for rider {args.RiderId}, rider already on track");
                return;
            }

            if(waitingRider != null)
            {
                Log.Warn($"Ignoring ready event for rider {args.RiderId}, another rider is in the start box: {waitingRider.Rider.Id}");
                return;
            }

            RiderReadyEvent newEvent = new RiderReadyEvent(args.Received, ready, Guid.NewGuid(), args.StaffName);
            waitingRider = newEvent;

            raceState.Enqueue(newEvent);

            OnRiderWaiting?.Invoke(this, new WaitingRiderEventArgs(newEvent));
        }

        /// <summary>
        /// Triggered by a user when the rider has entered the stop box
        /// </summary>
        /// <param name="args"></param>
        private void OnRiderFinished(RiderFinishedEventArgs args)
        {
            bool onTrack = onTrackRiders.TryGetValue(args.RiderId, out (RiderReadyEvent id, TimingEvent timer) pendingLap);

            if(!onTrack)
            {
                Log.Warn($"Ignoring rider finished event for rider {args.RiderId}, rider not on track");
                return;
            }

            bool timeExists = endTimes.TryGetValue(args.TimeId, out TimingEvent matchedTime);

            if(!timeExists)
            {
                Log.Warn($"Ignoring rider finished event for rider {args.RiderId}, time event with id {args.TimeId} does not exist");
                return;
            }

            RiderFinishedEvent userEvent = new RiderFinishedEvent(args.Received, pendingLap.id.Rider, Guid.NewGuid(), args.StaffName, matchedTime);
            raceState.Enqueue(userEvent);

            FinishedEvent systemEvent = new FinishedEvent(pendingLap.id, pendingLap.timer, userEvent, Guid.NewGuid());
            raceState.Enqueue(systemEvent);

            onTrackRiders.Remove(args.RiderId);

            Lap lap = new Lap(systemEvent);
            laps.Add(lap);
            ApplyPendingEvents(lap);

            OnRiderMatched?.Invoke(this, new LapCompletedEventArgs(lap));
        }

        /// <summary>
        /// When a gate of the timing unit is triggered. If its the start gate we will attempt to match it to a start id, if its the end gate we will attempt to match it to an end id.
        /// This method may register a rider as finished, on track or dnf
        /// </summary>
        /// <param name="args"></param>
        private void OnTimer(TimingTriggeredEventArgs args)
        {
            //When a waiting rider triggers the start timing unit, they are recorded as on track
            if (args.GateId == config.StartTimingGateId)
            {
                if (waitingRider != null)
                {
                    TimingEvent newEvent = new TimingEvent(args.Received, waitingRider.Rider, args.Microseconds, args.GateId);
                    Log.Info($"Rider {waitingRider.Rider.Id} is now on track with timestamp {args.Microseconds}");

                    onTrackRiders.Add(waitingRider.Rider.Id, (waitingRider, newEvent));
                    raceState.Enqueue(newEvent);

                    waitingRider = null;
                    
                    OnStartEmpty?.Invoke(this, EventArgs.Empty);
                } 
                else
                {
                    Log.Info($"Discarding timestamp from gate {args.GateId} at {args.Microseconds} us, no waiting rider");
                }
            }
            else if (args.GateId == config.EndTimingGateId)
            {
                //when a rider triggers the end timing unit, that must be matched to an end id unit event
                //if there is such a match, then it must be matched to an on track rider

                //we dont know the rider yet
                TimingEvent newEvent = new TimingEvent(args.Received, null, args.Microseconds, args.GateId);

                raceState.Enqueue(newEvent);
                endTimes.Add(newEvent.EventId, newEvent);
            }
            else
            {
                throw new ArgumentException($"Found unkown gate id in timing event: {args.GateId}. Known gates are start: {config.StartTimingGateId}, end: {config.EndTimingGateId}");
            }
        }

        private void OnManualDNF(ManualDNFEventArgs raceEvent)
        {
            if (onTrackRiders.TryGetValue(raceEvent.RiderId, out (RiderReadyEvent id, TimingEvent timer) onTrack))
            {
                Log.Info($"Received DNF event for rider {raceEvent.RiderId}");

                ManualDNFEvent dnf = new ManualDNFEvent(onTrack.id, raceEvent.StaffName);

                raceState.Enqueue(dnf);

                Lap lap = new Lap(dnf);
                this.laps.Add(lap);
                ApplyPendingEvents(lap);
                OnRiderDNF?.Invoke(this, new LapCompletedEventArgs(lap));
            }
            else
            {
                ManualDNFEvent dnf = new ManualDNFEvent(new RiderReadyEvent(new DateTime(0), new Rider("unknown", raceEvent.RiderId), Guid.NewGuid(), raceEvent.StaffName), raceEvent.StaffName);

                raceState.Enqueue(dnf);

                Log.Warn($"Received DNF event for rider {raceEvent.RiderId} who is not on track. Event will be ignored");
            }
        }

        /// <summary>
        /// Will register a DSQ event.
        /// If the rider is on track the DSQ event will be applied when the lap finishes (normally or DNF)
        /// If the rider is not on track, but has a completed lap the DSQ will be applied to the last completed lap
        /// If the rider is not on track and has no completed laps the event is ignored
        /// </summary>
        /// <param name="raceEvent"></param>
        private void OnDSQ(DSQEventArgs raceEvent)
        {
            Rider rider = knownRiders.Where(r => r.Id == raceEvent.RiderId).FirstOrDefault();

            if (rider == null)
            {
                DSQEvent dsq = new DSQEvent(raceEvent.Received, new Rider("unknown", raceEvent.RiderId), raceEvent.StaffName, raceEvent.Reason);

                raceState.Enqueue(dsq);

                Log.Warn($"Received DSQ event for unkown rider {raceEvent.RiderId}. Event will be ignored");
            }
            else
            {
                DSQEvent dsq = new DSQEvent(raceEvent.Received, rider, raceEvent.StaffName, raceEvent.Reason);

                raceState.Enqueue(dsq);

                if (onTrackRiders.ContainsKey(rider.Id))
                {
                    pendingDisqualifications.Add(rider.Id, dsq);
                }
                else
                {
                    Lap lastLap = laps.FindLast(l => l.Rider == rider && !l.Disqualified);

                    if (lastLap == null)
                    {
                        Log.Info($"Received DSQ event for rider {rider} that is not on track and has no laps without a DSQ this session. Event will be ignored");
                    }
                    else
                    {
                        lastLap.SetDsq(dsq);
                    }
                }
            }
        }

        /// <summary>
        /// Will register a penalty event
        /// If the rider is on track the Penalty will be applied when the lap is finshed (normally or DNF)
        /// If the rider is not on track, but has a completed lap the penalty will be applied to the last completed lap
        /// If the rider is not on track and has no completed laps the event is ignored
        /// </summary>
        /// <param name="raceEvent"></param>
        private void OnPenalty(PenaltyEventArgs raceEvent)
        {
            Rider rider = knownRiders.Where(r => r.Id == raceEvent.RiderId).FirstOrDefault();

            if (rider == null)
            {
                PenaltyEvent penalty = new PenaltyEvent(raceEvent.Received, new Rider("unknown", raceEvent.RiderId), raceEvent.Reason, raceEvent.Seconds, raceEvent.StaffName);

                raceState.Enqueue(penalty);

                Log.Warn($"Received Penalty event for unkown rider {raceEvent.RiderId}. Event will be ignored");
            }
            else
            {
                PenaltyEvent penalty = new PenaltyEvent(raceEvent.Received, rider, raceEvent.Reason, raceEvent.Seconds, raceEvent.StaffName);

                raceState.Enqueue(penalty);

                if (onTrackRiders.ContainsKey(rider.Id))
                {
                    pendingPenalties[rider.Id].Add(penalty);
                }
                else
                {
                    Lap lastLap = laps.FindLast(l => l.Rider == rider);

                    if (lastLap == null)
                    {
                        Log.Info($"Received Penalty event for rider {rider} that is not on track and has no laps this session. Event will be ignored");
                    }
                    else
                    {
                        lastLap.AddPenalties(new List<PenaltyEvent> { penalty });
                    }
                }
            }
        }

        /// <summary>
        /// Applies any panding DSQ and Penalty events to a completed lap
        /// </summary>
        /// <param name="lap"></param>
        private void ApplyPendingEvents(Lap lap)
        {
            if(pendingDisqualifications.ContainsKey(lap.Rider.Id))
            {
                lap.SetDsq(pendingDisqualifications[lap.Rider.Id]);
                pendingDisqualifications.Remove(lap.Rider.Id);
            }

            lap.AddPenalties(pendingPenalties[lap.Rider.Id]);
            pendingPenalties[lap.Rider.Id].Clear();
        }

        public void AddRider(Rider rider)
        {
            knownRiders.Add(rider);
            pendingPenalties.Add(rider.Id, new List<PenaltyEvent>());
        }

        public void RemoveRider(Guid id)
        {
            knownRiders.RemoveAll(r => r.Id == id);
            pendingDisqualifications.Remove(id);
            pendingPenalties.Remove(id);
        }

        public void AddEvent<T>(T manualEvent) where T : ManualEventArgs
        {
            OnEvent(manualEvent);
        }
    }

    public class LapCompletedEventArgs : EventArgs
    {
        public Lap Lap { get; private set; }

        public LapCompletedEventArgs(Lap lap)
        {
            Lap = lap;
        }
    }

    public class WaitingRiderEventArgs : EventArgs
    {
        public RiderReadyEvent Rider { get; private set; }

        public WaitingRiderEventArgs(RiderReadyEvent rider)
        {
            Rider = rider;
        }
    }
}
