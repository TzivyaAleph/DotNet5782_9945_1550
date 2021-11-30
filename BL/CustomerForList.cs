using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerForList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int ParcelProvided { get; set; }//number of parcels whov been provided
            public int ParcelNotProvided { get; set; }//number of parcels whov not been provided
            public int ParcelRecieved { get; set; }
            public int ParcelOnTheWay { get; set; }

            public override string ToString()
            {
                string result = "";
                result += $"ID is: {Id}\n";
                result += $"cosumer's name is: {Name}\n";
                result += $"phoneNumber is: {Phone}\n";
                result += $"The number of provided parcels is: {ParcelProvided}\n";
                result += $"The number of unprovided parcels is: {ParcelNotProvided}\n";
                result += $"The number of recieved parcels is: {ParcelRecieved}\n";
                result += $"The number of  parcels on the way is: {ParcelOnTheWay}\n";
                return result;
            }
        }
    }
}
