using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{

    public partial class BL
    {

        public void UpdateDrone(Drone drone)
        {
            IDAL.DO.Drone droneTemp = new IDAL.DO.Drone();
            if (drone.ID < 1000 || drone.ID > 10000)
                throw new BLIdException($"Drone ID {drone.ID} is not valid\n");
            droneTemp.ID = drone.ID;
            if (String.IsNullOrEmpty(drone.Model))
                throw new BLInvalidStringException($"Drone model {drone.Model} is not valid\n");
            droneTemp.Model = drone.Model;
            try
            {
              myDal.UpdateDrone(droneTemp);
            }
            catch (IDAL.DO.ExistingObjectException custEx)
            {
                //throw new Exception($"Customer id {id} was not found", custEx);
            }
        }

        public void SendDroneToChargeSlot(Drone d, Station s)
        {
            if (d.DroneStatuses != DroneStatuses.Available)
                throw new DronechargeException($"Drone {d.ID} is not available");
            //if(d.Battery)
            List<IDAL.DO.Station> stations =(List<IDAL.DO.Station>) myDal.FindAvailableStations();
            IDAL.DO.Station clossestStation = new IDAL.DO.Station();
            clossestStation = myDal.GetClossestStation(d.CurrentLocation.Latitude, d.CurrentLocation.Longitude, stations);
            if (clossestStation.ChargeSlots == 0)
                throw new DronechargeException($"There are no available charge slots in station {clossestStation.ID}");
            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone()
            {
                ID = d.ID,
                Model = d.Model,
                MaxWeight=(IDAL.DO.WeightCategories)d.MaxWeight,
            };
            try
            {
                myDal.SendDroneToChargeSlot(dalDrone, clossestStation);
            }
            catch (IDAL.DO.ExistingObjectException custEx)
            {
                //throw new Exception($"Customer id {id} was not found", custEx);
            }

        }
    }


}
