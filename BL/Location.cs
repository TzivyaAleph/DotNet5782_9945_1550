﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Location
    {
        public double Longitude { get; init; }
        public double Latitude { get; init; }

        public override string ToString()
        {
            string result = " ";
            result += $"longtitude is {Longitude},\n";
            result += $"lattitude is {Latitude},\n";
            return result;
        }
    }


}
