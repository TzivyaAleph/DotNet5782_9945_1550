﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class StationForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailableChargingSlots { get; set; }
        public int UnAvailableChargingSlots { get; set; }
    }
}