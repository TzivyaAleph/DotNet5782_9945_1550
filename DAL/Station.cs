using System;
namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int ID {  set; get; }
            public string StationName { set; get; }
            public int ChargeSlots { set; get; }//the amout of slots that available
            public long Longitude { set; get; }
            public long Lattitude { set; get; }

            public override string ToString()
            {
                string result=" ";
                result += $"ID is {ID},\n";
                result += $"station's name is {StationName},\n";
                result += $"number of charge slot is {ChargeSlots},\n";
                result += $"longitude is {Longitude},\n";
                result += $"lattitude is {Lattitude},\n";
                return result;
            }
        }
    }

}
