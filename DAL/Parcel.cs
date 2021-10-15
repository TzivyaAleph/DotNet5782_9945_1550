using System;
using System.Collections.Generic;
using System.Text;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public string ID { get; private set; }
            public string senderID { get;  set; }
            public string targeID { get;  set; }
            public WeightCategories weight { get;  set; }
            public Priorities priority { get; set; }
            public DateTime requested { get; set; }
            public string droneID { get; set; }
            public DateTime scheduled { get; set; }
            public DateTime pickedUp { get; set; }
            public DateTime delivered { get; set; }


        }
    }
}
