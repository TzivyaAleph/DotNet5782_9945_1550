using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Location StationLocation { get; set; }
            public int ChargeSlots { get; set; }
            public List<DroneCharge> DroneCharges { get; set; }

            public override string ToString()
            {
                string result = "";
                result += $"ID is: {Id}\n";
                result += $"station's name is: {Name}\n";
                result += $"location is:\n{StationLocation}";
                result += $"number of available charging slots is: {ChargeSlots}\n";
                result += $"the drones who are charging in this station are:\n";
                foreach (var dc in DroneCharges)
                {
                    result += $"{dc}\n";
                }
                return result;
            }
        }
    }
}
