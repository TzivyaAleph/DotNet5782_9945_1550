using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Drone
    {
        public int ID { set; get; }
        public string Model { set; get; }
        public Weight MaxWeight { set; get; }
        public  DroneStatuses DroneStatuses { set; get; }
        public double Battery { set; get; }
        public ParcelInDelivery ParcelInDelivery { get; set; }
        public Location CurrentLocation { get; set; }


        public override string ToString()
        {
            string result = " ";
            result += $"ID is {ID},\n";
            result += $"model is {Model},\n";
            result += $"the maximum weight is {MaxWeight},\n";
            result += $"the drone is {DroneStatuses},\n";
            result += $"battery: {Battery},\n";
            if(DroneStatuses==DroneStatuses.Delivered)
                result += $"the parcel in delivery is: {ParcelInDelivery},\n";
            result += $"the current location of th edrone is {CurrentLocation},\n";
            return result;
        }
    }
}
