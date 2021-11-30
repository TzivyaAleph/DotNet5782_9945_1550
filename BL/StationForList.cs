using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class StationForList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int AvailableChargingSlots { get; set; }
            public int UnAvailableChargingSlots { get; set; }

            public override string ToString()
            {
                string result = "";
                result += $"ID is: {Id}\n";
                result += $"station's name is: {Name}\n";
                result += $"The number of available charging slots in station is: {AvailableChargingSlots}\n";
                result += $"The number of not available charging slots in station is: {UnAvailableChargingSlots}\n";
                return result;
            }
        }
    }

 
}
