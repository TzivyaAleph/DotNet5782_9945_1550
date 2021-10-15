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
            public int ID { private set; get; }
            public string model {  set; get; }
            public weightCategories maxWeight { set; get; }
            public string status { set; get; }
            public string battery { set; get; }
        }
    }
    
}
