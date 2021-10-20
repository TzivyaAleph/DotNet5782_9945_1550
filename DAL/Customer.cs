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
            public int ID { get;  set; }
            public string name { get; set; }
            public int phoneNumber { get; set; }
            public double lattitude { get; set; }
            public double longitude { get; set; }

            public override string ToString()
            {
                string result = " ";
                result += $"ID is {ID},\n";
                result += $"cosumer's name is {name},\n";
                result += $"phoneNumber is {phoneNumber},\n";
                result += $"lattitude is {lattitude},\n";
                result += $"longitude is {longitude},\n";
                return result;
            }
        }
    }
}
