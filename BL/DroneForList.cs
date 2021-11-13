using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneForList
    {
        public int ID { set; get; }
        public string Model { set; get; }
        public Weight MaxWeight { set; get; }
        public DroneStatuses DroneStatuses { set; get; }
        public double Battery { set; get; }
        public Location CurrentLocation { get; set; }
        public int ParcelId { get; set; }
    }
}
