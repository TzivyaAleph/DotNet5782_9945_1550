using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneParcel
    {
        public int Id { get; set; }
        public double BatteryLevel { get; set; }
        public Location Location { get; set; }
    }
}
