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
            public string model {  set; get; }
            public WeightCategories maxWeight { set; get; }
            public DroneStatuses status { set; get; }
            public double battery { set; get; }

            public override string ToString()
            {
                string result = " ";
                result += $"ID is {ID},\n";
                result += $"model is {model},\n";
                result += $"the maximum weight is {maxWeight},\n";
                result += $"status: {status},\n";
                result += $"battery: {battery},\n";
                return result;
            }
        }
    }
    
}
