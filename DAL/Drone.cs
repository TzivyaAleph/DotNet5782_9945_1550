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
            public string ID { private set; get; }
            public string model {  set; get; }
            public WeightCategories maxWeight { set; get; }
            public string status { set; get; }
            public double battery { set; get; }

            public override string ToString()
            {
                string result = " ";
                result += $"ID is {ID},";
                result += $"model is {model},";
                result += $"the maximum weight is {maxWeight},";
                result += $"status: {status},";
                result += $"battery: {battery},";
                return result;
            }
        }
    }
    
}
