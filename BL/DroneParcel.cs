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

        public override string ToString()
        {
            string result = " ";
            result += $"ID is {Id},\n";
            result += $"battery level is {BatteryLevel},\n";
            result += $"location is {Location},\n";
            return result;
        }
    }
}
