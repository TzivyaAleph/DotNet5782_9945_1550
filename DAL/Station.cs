using System;
namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id {  set; get; }
            public string Name { set; get; }
            public int ChargeSlots { set; get; }//the amout of slots that available
            public long Longitude { set; get; }
            public long Lattitude { set; get; }

            public override string ToString()
            {
                string result=" ";
                result += $"ID is {Id},\n";
                result += $"station's name is {Name},\n";
                result += $"number of charge slot is {ChargeSlots},\n";
                result += $"longitude is {Longitude},\n";
                result += $"lattitude is {Lattitude},\n";
                return result;
            }
        }
    }

}
