using System;
using System.Collections.Generic;
using System.Text;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public string droneID { get; set; }
            public string stationID { get; set; }
            public override string ToString()
            {
                string result = "";
                result += $"drone ID is: {droneID} \n";
                result += $"station ID is: {stationID} \n";
                return result;
            }
        }
    }
}
