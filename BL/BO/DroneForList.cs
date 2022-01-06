using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BO
{
    public class DroneForList
    {
        public int Id { set; get; }
        public string Model { set; get; }
        public Weight Weight { set; get; }
        public DroneStatuses DroneStatuses { set; get; }
        public double Battery { set; get; }//by precent
        public Location CurrentLocation { get; set; }
        public int ParcelId { get; set; }
        public bool IsDeleted { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"ID is: {Id}\n";
            result += $"model is: {Model}\n";
            result += $"the maximum weight is: {Weight}\n";
            result += $"the drone is: {DroneStatuses}\n";
            result += $"battery: {Battery}\n";
            result += $"the current location of the drone is:\n {CurrentLocation}";
            result += $"the attributted parcel's ID is: {ParcelId}\n";
            return result;
        }
    }
}

