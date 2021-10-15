using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DO
    {
        class Customer
        {
            public string ID { get; private set; }
            public string name { get; set; }
            public string phoneNumber { get; set; }
            public double lattitude { get; set; }
            public double longtitude { get; set; }
        }
    }
}
