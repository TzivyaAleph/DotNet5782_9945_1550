using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IBL
{
    namespace BO
    {
        public class ParcelInDelivery
        {
            public int Id { get; set; }
            public bool OnTheWay { get; set; }
            public Weight Weight { get; set; }
            public CustomerParcel CustomerSender { get; set; }
            public CustomerParcel CustomerReciever { get; set; }
            public Location Collection { get; set; }
            public Location Destination { get; set; }
            public int Transportation { get; set; }//the distance by meters

            public override string ToString()
            {
                string result = " ";
                result += $"ID is {Id},\n";
                result += $"the parcel  is ";
                if (!OnTheWay)
                    result += "waiting for pick up,\n";
                else
                    result += "on its way,\n";
                result += $"the sender of the parcel is {CustomerSender},\n";
                result += $"the reciever of the parcel is {CustomerReciever},\n";
                result += $"the location's collection: {Collection},\n";
                result += $"the location's destination: {Destination},\n";
                result += $"the distance of the transportation(by meters)  is  {Transportation},\n";
                return result;
            }
        }
    }
}
