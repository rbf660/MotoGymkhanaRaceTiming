﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Config
{
    public class TrackerConfig
    {
        public int EndMatchTimeout { get; set; }
        public int StartTimingGateId { get; set; }
        public int EndTimingGateId { get; set; }
    }
}
