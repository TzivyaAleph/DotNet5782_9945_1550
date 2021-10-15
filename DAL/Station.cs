using System;
namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public string ID { private set; get; }
            public string stationName { set; get; }
            public int chargeSlots { set; get; }
            public long longitude { set; get; }
            public long lattitude { set; get; }


        }
    }

}
