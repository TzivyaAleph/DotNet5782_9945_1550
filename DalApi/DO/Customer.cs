using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public double Lattitude { get; set; }
        public double Longtitude { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public CustomersType CustomerType { get; set; }
        public override string ToString()
        {
            string result = " ";
            result += $"ID is {Id},\n";
            result += $"cosumer's name is {Name},\n";
            result += $"phoneNumber is {PhoneNumber},\n";
            result += $"lattitude is {Lattitude},\n";
            result += $"longtitude is {Longtitude},\n";
            result += $"customer type is {CustomerType},\n";
            return result;
        }
    }
}

