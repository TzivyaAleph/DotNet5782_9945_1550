using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id {  set; get; }
            public string Model {  set; get; }
            public Weight MaxWeight { set; get; } 

            public override string ToString()
            {
                string result = " ";
                result += $"ID is {Id},\n";
                result += $"model is {Model},\n";
                result += $"the maximum weight is {MaxWeight},\n";
                return result;
            }
        }
    }
    
}
