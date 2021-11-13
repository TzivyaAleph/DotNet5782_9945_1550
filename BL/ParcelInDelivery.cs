using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInDelivery
    {
        public int Id { get; set; }
        public bool ParcelStatus { get; set; }
        public Weight Weight { get; set; }
        public CustomerParcel CustomerSender { get; set; }
        public CustomerParcel CustomerReciever { get; set; }
        public Location Collection { get; set; }
        public Location Destination { get; set; }
        public int Transportation { get; set; }//by meters

        public override string ToString()
        {
            string result = " ";
            result += $"ID is {Id},\n";
            result += $"the parcel  is ";
            if (ParcelStatus)
                result += "waiting for pick up,\n";
            else
                result += "on its way,\n";
            result += $"the sender of the parcel is {MaxWeight},\n";
            result += $"the drone is {DroneStatuses},\n";
            result += $"battery: {Battery},\n";
            result += $"the parcel in delivery is: {ParcelInDelivery},\n";
            result += $"the current location of th edrone is {CurrentLocation},\n";
            return result;
        }
    }
}
