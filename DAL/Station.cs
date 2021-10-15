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

            public override string ToString()
            {
                string result=" ";
                result += $"ID is {ID},";
                result += $"station's name is {stationName},";
                result += $"number of charge slot is {chargeSlots},";
                result += $"longitude is {longitude},";
                result += $"lattitude is {lattitude},";
                return result;
            }
        }
    }

}
