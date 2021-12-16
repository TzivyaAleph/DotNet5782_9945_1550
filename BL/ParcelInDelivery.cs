using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string result = "";
            result += $"ID is: {Id}\n";
            result += $"The parcel is ";
            if (!OnTheWay)
                result += "waiting for pick up,\n";
            else
                result += "on its way,\n";
            result += $"The parcel's sender is:\n {CustomerSender}";
            result += $"The parcel's reciever is:\n {CustomerReciever}";
            result += $"Collection's location:\n {Collection}";
            result += $"Destination's location:\n {Destination}";
            result += $"The distance of the transportation (by meters) is: {Transportation}\n";
            return result;
        }
    }
}

