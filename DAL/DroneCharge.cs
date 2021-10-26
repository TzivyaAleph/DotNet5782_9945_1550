using System;
using System.Collections.Generic;
using System.Text;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int DroneID { get; set; }
            public int StationID { get; set; }
            public override string ToString()
            {
                string result = "";
                result += $"drone ID is: {DroneID} \n";
                result += $"station ID is: {StationID} \n";
                return result;
            }
        }
    }
}
