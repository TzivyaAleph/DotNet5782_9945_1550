using System;
namespace IDAL
{
    namespace DO
    {
        public struct Client
        {
            public string ID { get; private set; }
            public String name { get; set; }
            public string number { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public override string ToString()
            {
                String result = " ";
                result+=$"ID is{ID},"
                return base.ToString();
            }
        }
    }
}