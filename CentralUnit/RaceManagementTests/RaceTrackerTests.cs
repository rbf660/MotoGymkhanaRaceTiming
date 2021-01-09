// <copyright file="RaceTrackerTests.cs" company="Moto Gymkhana">
//     Copyright (c) Moto Gymkhana. All rights reserved.
// </copyright>

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaceManagement;
using RaceManagementTests.TestHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RaceManagementTests
{
    [TestClass]
    public class RaceTrackerTests
    {
        MockRiderIdUnit StartId;
        MockRiderIdUnit EndId;
        MockTimingUnit Timer;

        CancellationTokenSource Source;

        RaceTracker Subject;

        Task<RaceSummary> Race;

        [TestInitialize]
        public void Init()
        {
            StartId = new MockRiderIdUnit();
            EndId = new MockRiderIdUnit();
            Timer = new MockTimingUnit();

            Source = new CancellationTokenSource();

            Subject = new RaceTracker(Timer, StartId, EndId, 0, 1);

            Race = Subject.Run(Source.Token);
        }


        [TestMethod]
        public void OnStartId_ShouldSaveEvent()
        {
            StartId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 1, 1), "StartId");

            Source.Cancel();

            RaceSummary summary = Race.Result;
            var state = Subject.GetState;

            Assert.AreEqual(1, summary.Events.Count);

            EnteredEvent entered = summary.Events[0] as EnteredEvent;
            Assert.AreEqual(entered, state.waiting[0]);

            Assert.AreEqual("Martijn", entered.Rider);
            CollectionAssert.AreEqual(new byte[] { 0, 1 }, entered.SensorId);
            Assert.AreEqual(new DateTime(2000, 1, 1, 1, 1, 1), entered.Time);
        }

        [TestMethod]
        public void OnTimer_ForStart_WithoutWaitingRider_ShouldIgnoreEvent()
        {
            //0 is the start gate
            Timer.EmitTriggerEvent(100, "Timer", 0, new DateTime(2000,1,1,1,1,1));

            Source.Cancel();

            RaceSummary summary = Race.Result;
            var state = Subject.GetState;

            Assert.AreEqual(0, summary.Events.Count);
            Assert.AreEqual(0, state.onTrack.Count);
            Assert.AreEqual(0, state.waiting.Count);
        }

        [TestMethod]
        public void OnTimer_ForStart_WitWaitingRider_ShouldMatchWithRider()
        {
            //rider enters start box
            StartId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 1, 1), "StartId");
            //rider triggers timing gate
            Timer.EmitTriggerEvent(100, "Timer", 0, new DateTime(2000, 1, 1, 1, 1, 1));

            Source.Cancel();

            RaceSummary summary = Race.Result;
            var state = Subject.GetState;

            //we expect an EnteredEvent and a TimingEvent, in that order
            Assert.AreEqual(2, summary.Events.Count);

            EnteredEvent id = summary.Events[0] as EnteredEvent;
            TimingEvent start = summary.Events[1] as TimingEvent;
            Assert.AreEqual(state.onTrack[0].id, id);
            Assert.AreEqual(state.onTrack[0].timer, start);

            Assert.AreEqual("Martijn", start.Rider);
            Assert.AreEqual(100L, start.Microseconds);
            Assert.AreEqual(new DateTime(2000, 1, 1, 1, 1, 1), start.Time);

            Assert.AreEqual(0, state.waiting.Count);
            Assert.AreEqual(0, state.unmatchedIds.Count);
            Assert.AreEqual(0, state.unmatchedTimes.Count);
        }

        [TestMethod]
        public void OnTimer_ForEnd_WithoutRiderLeft_ShouldSaveEvent()
        {
            //1 is the end gate
            Timer.EmitTriggerEvent(100, "Timer", 1, new DateTime(2000, 1, 1, 1, 1, 1));

            Source.Cancel();

            RaceSummary summary = Race.Result;
            var state = Subject.GetState;

            //It is possible for the end timing gate to be triggered before the rider id is caught be the end id unit
            //save the timer event for later matching
            Assert.AreEqual(1, summary.Events.Count);

            //the state should match the summary
            TimingEvent end = summary.Events[0] as TimingEvent;
            Assert.AreEqual(state.unmatchedTimes[0], end);

            Assert.AreEqual(null, end.Rider);//a lone timer event cannot have a rider
            Assert.AreEqual(100L, end.Microseconds);
            Assert.AreEqual(new DateTime(2000, 1, 1, 1, 1, 1), end.Time);
        }

        [TestMethod]
        public void OnEndId_WithoutRiderOnTrack_ShouldIgnoreEvent()
        {
            EndId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 1, 1), "StartId");

            Source.Cancel();

            RaceSummary summary = Race.Result;
            var state = Subject.GetState;

            Assert.AreEqual(0, summary.Events.Count);
            Assert.AreEqual(0, state.onTrack.Count);
            Assert.AreEqual(0, state.waiting.Count);
            Assert.AreEqual(0, state.unmatchedIds.Count);
            Assert.AreEqual(0, state.unmatchedTimes.Count);
        }

        [TestMethod]
        public void OnEndId_WithDifferentRiderOnTrack_ShouldIgnoreEvent()
        {
            //rider enters start box
            StartId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 1, 1), "StartId");
            //rider triggers timing gate
            Timer.EmitTriggerEvent(100, "Timer", 0, new DateTime(2000, 1, 1, 1, 1, 1));

            //rider not on track triggers end id
            EndId.EmitIdEvent("Richard", new byte[] { 0, 2 }, new DateTime(2000, 1, 1, 1, 1, 1), "StartId");

            Source.Cancel();

            RaceSummary summary = Race.Result;
            var state = Subject.GetState;

            //we expect only the events for Martijn tto be recorded
            Assert.AreEqual(2, summary.Events.Count);

            EnteredEvent id = summary.Events[0] as EnteredEvent;
            TimingEvent start = summary.Events[1] as TimingEvent;
            Assert.AreEqual(state.onTrack[0].id, id);
            Assert.AreEqual(state.onTrack[0].timer, start);

            Assert.AreEqual("Martijn", start.Rider);
            Assert.AreEqual(100L, start.Microseconds);
            Assert.AreEqual(new DateTime(2000, 1, 1, 1, 1, 1), start.Time);

            //no riders should be waiting at the start.
            //no end times or ids shoudl be waiting to be matched
            Assert.AreEqual(0, state.waiting.Count);
            Assert.AreEqual(0, state.unmatchedIds.Count);
            Assert.AreEqual(0, state.unmatchedTimes.Count);
        }

        [TestMethod]
        [DataRow(true, true, false)]
        [DataRow(false, false, false)]
        [DataRow(true, false, false)]
        [DataRow(false, true, false)]
        [DataRow(true, true, true)]
        [DataRow(false, false, true)]
        [DataRow(true, false, true)]
        [DataRow(false, true, true)]
        public void OnEndId_WithMatchingTiming_ShouldCompleteLap(bool includeUnmatchedTime, bool includeUnmatchedId, bool flipEndEvents)
        {
            //rider enters start box
            StartId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 1, 1), "StartId");
            //rider triggers timing gate
            Timer.EmitTriggerEvent(100, "Timer", 0, new DateTime(2000, 1, 1, 1, 1, 2));

            //somebody walks through end timing gate 10 secs after rider has started
            if (includeUnmatchedTime)
                Timer.EmitTriggerEvent(400, "Timer", 1, new DateTime(2000, 1, 1, 1, 1, 12));

            //a different rider gets too close to the stop box
            if (includeUnmatchedId)
                EndId.EmitIdEvent("Richard", new byte[] { 0, 2 }, new DateTime(2000, 1, 1, 1, 1, 30), "EndId");

            List<Action> endEvents = new List<Action>
            { 
                //rider triggers id in stop box
                () => EndId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 2, 1), "EndId"),
                //rider triggers timing in stop box
                () => Timer.EmitTriggerEvent(500, "Timer", 1, new DateTime(2000, 1, 1, 1, 2, 2))
            };

            if (flipEndEvents)
                endEvents.Reverse();
            foreach (Action a in endEvents)
                a.Invoke();

            Source.Cancel();
            RaceSummary summary = Race.Result;
            var state = Subject.GetState;

            FinishedEvent finish = Race.Result.Events.Last() as FinishedEvent;

            //Martijn should have done a lightning fast 400 microsecond lap
            Assert.AreEqual("Martijn", finish.Rider);
            Assert.AreEqual(400L, finish.LapTime);

            //There should be nothing going on in the race at this point
            Assert.AreEqual(0, state.waiting.Count);
            Assert.AreEqual(0, state.unmatchedIds.Count);
            Assert.AreEqual(0, state.unmatchedTimes.Count);
            Assert.AreEqual(0, state.onTrack.Count);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(-10)]
        public void OnEndId_ShouldRespectTimeout(int timeDifference)
        {
            //rider enters start box
            StartId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 1, 1), "StartId");
            //rider triggers timing gate
            Timer.EmitTriggerEvent(100, "Timer", 0, new DateTime(2000, 1, 1, 1, 1, 20));

            //rider triggers timing in stop box
            Timer.EmitTriggerEvent(500, "Timer", 1, new DateTime(2000, 1, 1, 1, 2, 20));

            //end id is triggered 11 seconds apart, should not match
            EndId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 2, 20 + timeDifference + Math.Sign(timeDifference)), "EndId");

            //end id is triggered 10 seconds apart, should match
            EndId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 2, 20 + timeDifference), "EndId");

            Source.Cancel();
            RaceSummary summary = Race.Result;
            var state = Subject.GetState;

            FinishedEvent finish = summary.Events.Last() as FinishedEvent;

            Assert.AreEqual(20+timeDifference, finish.Left.Time.Second);

            //There should be nothing going on in the race at this point
            Assert.AreEqual(0, state.waiting.Count);
            Assert.AreEqual(0, state.unmatchedTimes.Count);
            Assert.AreEqual(0, state.onTrack.Count);

            //depending on whether the timeDifference is negative or positive the unmatched time should be cleared
            //on positive timeDifference the unmatched time is not old enough to be cleared
            if(timeDifference > 0)
                Assert.AreEqual(1, state.unmatchedIds.Count);
            else
                Assert.AreEqual(0, state.unmatchedIds.Count);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(-10)]
        public void OnTimer_ForEnd_ShouldRespectTimeout(int timeDifference)
        {
            //rider enters start box
            StartId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 1, 1), "StartId");
            //rider triggers timing gate
            Timer.EmitTriggerEvent(100, "Timer", 0, new DateTime(2000, 1, 1, 1, 1, 20));

            //rider triggers id in stop box
            EndId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 2, 20), "EndId");

            //end timer triggered 11 seconds apart, should not match
            Timer.EmitTriggerEvent(500, "Timer", 1, new DateTime(2000, 1, 1, 1, 2, 20 + timeDifference + Math.Sign(timeDifference)));

            //end timer triggered 10 seconds apart, should match
            Timer.EmitTriggerEvent(500, "Timer", 1, new DateTime(2000, 1, 1, 1, 2, 20 + timeDifference));

            Source.Cancel();
            RaceSummary summary = Race.Result;
            var state = Subject.GetState;

            FinishedEvent finish = summary.Events.Last() as FinishedEvent;

            Assert.AreEqual(20 + timeDifference, finish.TimeEnd.Time.Second);

            //There should be nothing going on in the race at this point
            Assert.AreEqual(0, state.waiting.Count);
            Assert.AreEqual(0, state.unmatchedIds.Count);
            Assert.AreEqual(0, state.onTrack.Count);

            //depending on whether the timeDifference is negative or positive the unmatched id should be cleared
            //on positive timeDifference the unmatched time is not old enough to be cleared
            if (timeDifference > 0)
                Assert.AreEqual(1, state.unmatchedTimes.Count);
            else
                Assert.AreEqual(0, state.unmatchedTimes.Count);
        }

        [TestMethod]
        public void RaceWithMultipleOnTrack_AndDNF_ShouldWork()
        {
            SimulateRaceWithDNF();

            Source.Cancel();
            RaceSummary summary = Race.Result;
            var state = Subject.GetState;

            List<FinishedEvent> finishes = summary.Events.Where(e => e is FinishedEvent).Select(e => e as FinishedEvent).ToList();
            DNFEvent dnf = summary.Events.Last() as DNFEvent;

            Assert.AreEqual("Martijn", finishes[0].Rider);
            Assert.AreEqual("Bert", finishes[1].Rider);

            Assert.AreEqual("Richard", dnf.Rider);
            Assert.AreEqual("Bert", dnf.OtherRider.Rider);

            //There should be nothing going on in the race at this point
            Assert.AreEqual(0, state.waiting.Count);
            Assert.AreEqual(0, state.unmatchedIds.Count);
            Assert.AreEqual(0, state.onTrack.Count);
            Assert.AreEqual(0, state.unmatchedTimes.Count);
        }

        /// <summary>
        /// Simulates a race where Martijn, Richard and Bert start, but only Martijn and Bert finish
        /// </summary>
        private void SimulateRaceWithDNF()
        {
            DateTime start = new DateTime(2000, 1, 1, 1, 1, 1);

            (string name, byte[] id) martijn = ("Martijn", new byte[] { 0, 1 });
            (string name, byte[] id) richard = ("Richard", new byte[] { 0, 2 });
            (string name, byte[] id) bert = ("Bert", new byte[] { 0, 3 });

            MakeStartEvents(martijn.name, martijn.id, start, StartId, Timer);

            start = start.AddSeconds(30);

            MakeStartEvents(richard.name, richard.id, start, StartId, Timer);

            start = start.AddSeconds(30);

            MakeStartEvents(bert.name, bert.id, start, StartId, Timer);

            //all riders have started. Martijn and bert will finsh, this will mark Richard as DNF
            start = start.AddSeconds(30);

            MakeEndEvents(martijn.name, martijn.id, start, EndId, Timer);

            start = start.AddSeconds(30);

            MakeEndEvents(bert.name, bert.id, start, EndId, Timer);
        }

        [TestMethod]
        public void OnEndId_WithAccidentalEndId_ShouldMatch()
        {
            //rider enters start box
            StartId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 1, 1), "StartId");
            //rider triggers timing gate
            Timer.EmitTriggerEvent(100, "Timer", 0, new DateTime(2000, 1, 1, 1, 1, 1));

            //rider triggers id in stop box accidentally (maybe the track was constructed to pass too close to stop box
            EndId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 2, 20), "EndId");

            //rider triggers id in stop box for real five seconds later
            EndId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 2, 25), "EndId");

            //end timer triggered 1 second later
            Timer.EmitTriggerEvent(500, "Timer", 1, new DateTime(2000, 1, 1, 1, 2, 26));

            Source.Cancel();
            RaceSummary summary = Race.Result;
            var state = Subject.GetState;

            FinishedEvent finish = summary.Events.Last() as FinishedEvent;
            Assert.AreEqual(25, finish.Left.Time.Second);

            //There should be nothing going on in the race at this point, except for the lingering end id event
            Assert.AreEqual(0, state.waiting.Count);
            Assert.AreEqual(0, state.onTrack.Count);
            Assert.AreEqual(0, state.unmatchedTimes.Count);

            Assert.AreEqual(1, state.unmatchedIds.Count);
        }

        [TestMethod]
        public void OnRiderWaiting_WithFirstAndSecondRider_ShouldFire()
        {
            string waiting = null;

            Subject.OnRiderWaiting += (obj, args) => waiting = args.Rider.Rider;

            //rider enters start box
            StartId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 1, 1), "StartId");

            //Martijn should be flagged as ready to start
            Assert.AreEqual(waiting, "Martijn");

            //Second rider enters the queue to start
            StartId.EmitIdEvent("Bert", new byte[] { 0, 2 }, new DateTime(2000, 1, 1, 1, 2, 1), "StartId");

            //OnRiderWaiting should not be fired since Martijn has not left
            Assert.AreEqual(waiting, "Martijn");

            //Martijn triggers timing gate
            Timer.EmitTriggerEvent(100, "Timer", 0, new DateTime(2000, 1, 1, 1, 2, 1));

            //Bert moves to the front of the waiting queue, so he is now ready to start
            Assert.AreEqual(waiting, "Bert");

            Source.Cancel();
        }

        [TestMethod]
        public void OnStartEmpty_WhenLastRiderStart_ShouldFire()
        {
            bool isEmpty = false;

            Subject.OnStartEmpty += (obj, args) => isEmpty = true;

            //rider enters start box
            StartId.EmitIdEvent("Martijn", new byte[] { 0, 1 }, new DateTime(2000, 1, 1, 1, 1, 1), "StartId");

            //event should not have fired
            Assert.IsFalse(isEmpty);

            //Second rider enters the queue to start
            StartId.EmitIdEvent("Bert", new byte[] { 0, 2 }, new DateTime(2000, 1, 1, 1, 2, 1), "StartId");

            //event should not have fired
            Assert.IsFalse(isEmpty);

            //Martijn and Bert trigger timing gate
            Timer.EmitTriggerEvent(100, "Timer", 0, new DateTime(2000, 1, 1, 1, 2, 1));
            Timer.EmitTriggerEvent(100, "Timer", 0, new DateTime(2000, 1, 1, 1, 2, 2));

            Assert.IsTrue(isEmpty);

            Source.Cancel();
        }

        [TestMethod]
        public void OnRiderFinished_WithFinishAndDNF_ShouldFireForFinish()
        {
            List<string> finished = new List<string>();

            Subject.OnRiderFinished += (obj, args) => finished.Add(args.Finish.Rider);

            SimulateRaceWithDNF();

            Source.Cancel();

            Assert.AreEqual(2, finished.Count);
            Assert.AreEqual("Martijn", finished[0]);
            Assert.AreEqual("Bert", finished[1]);
        }

        /// <summary>
        /// Makes events for a lap begin at start
        /// </summary>
        /// <param name="riderName"></param>
        /// <param name="sensorId"></param>
        /// <param name="start"></param>
        /// <param name="id"></param>
        /// <param name="time"></param>
        private void MakeStartEvents(string riderName, byte[] sensorId, DateTime start, MockRiderIdUnit id, MockTimingUnit time)
        {
            id.EmitIdEvent(riderName, sensorId, start, "StartId");
            time.EmitTriggerEvent(100, "Timer", 0, start);
        }

        /// <summary>
        /// Makes events for a lap finish at start + 1 minute
        /// </summary>
        /// <param name="riderName"></param>
        /// <param name="sensorId"></param>
        /// <param name="end"></param>
        /// <param name="id"></param>
        /// <param name="time"></param>
        private void MakeEndEvents(string riderName, byte[] sensorId, DateTime end, MockRiderIdUnit id, MockTimingUnit time)
        {
            id.EmitIdEvent(riderName, sensorId, end, "EndId");
            time.EmitTriggerEvent(100, "Timer", 1, end);
        }
    }
}
