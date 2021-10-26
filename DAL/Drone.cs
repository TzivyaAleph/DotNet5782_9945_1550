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
            public int ID {  set; get; }
            public string Model {  set; get; }
            public WeightCategories MaxWeight { set; get; } 
            public DroneStatuses Status { set; get; }
            public double Battery { set; get; }

            public override string ToString()
            {
                string result = " ";
                result += $"ID is {ID},\n";
                result += $"model is {Model},\n";
                result += $"the maximum weight is {MaxWeight},\n";
                result += $"status: {Status},\n";
                result += $"battery: {Battery},\n";
                return result;
            }
        }
    }
    
}
