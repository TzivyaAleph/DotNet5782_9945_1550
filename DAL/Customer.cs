using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            public string ID { get; private set; }
            public string name { get; set; }
            public string phoneNumber { get; set; }
            public double lattitude { get; set; }
            public double longtitude { get; set; }

            public override string ToString()
            {
                string result = " ";
                result += $"ID is {ID},\n";
                result += $"cosumer's name is {name},\n";
                result += $"phoneNumber is {phoneNumber},\n";
                result += $"lattitude is {lattitude},\n";
                result += $"longtitude is {longtitude},\n";
                return result;
            }
        }
    }
}
