﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public string  Phone { get; set; }
        public Location  Location { get; set; }
        public List<ParcelCustomer> SentParcels { get; set; }
        public List<ParcelCustomer> ReceiveParcels { get; set; }

        public override string ToString()
        {
            string result = " ";
            result += $"ID is {Id},\n";
            result += $"cosumer's name is {Name},\n";
            result += $"phoneNumber is {Phone},\n";
            result += $"location is {Location},\n";
            result += $"sent parcels are:\n";
            foreach (var pc in SentParcels)
            {
                result += $"{pc}\n";
            }
            result += $"recieved parcels are:\n";
            foreach (var pc in ReceiveParcels)
            {
                result += $"{pc}\n";
            }
            return result;
        }

    }


}
