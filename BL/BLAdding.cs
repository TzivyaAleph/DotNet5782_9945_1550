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
        /// <summary>
        /// adds a new station to the station list
        /// </summary>
        /// <param name="myID">station id</param>
        /// <param name="name">station name</param>
        /// <param name="numOfSlots">number of available charge slots in station</param>
        /// <param name="location">station's location</param>
        public void AddBlStation(Station s)
        {
            if( s.Id < 1000 || s.Id>10000)
            {
                throw new BLInvalidInputException($"id {s.Id} is not valid !!");
            }
            if(s.AvailableChargingSlots<0 || s.AvailableChargingSlots>50)
            {
                throw new BLInvalidInputException($"number of slots {s.AvailableChargingSlots} is not valid !!");
            }
            if(String.IsNullOrEmpty(s.Name))
            {
                throw new BLInvalidInputException($"name {s.Name} is not correct !!");
            }
            if(s.StationLocation.Latitude<-5000 || s.StationLocation.Latitude>5000 || s.StationLocation.Longitude <-5000 || s.StationLocation.Longitude >5000)
            {
                throw new BLInvalidInputException($"location {s.StationLocation} is not valid !!");
            }
            s.DroneCharges = null;
            IDAL.DO.Station tmp= new IDAL.DO.Station
            {
                ID = s.Id,
                StationName = s.Name,
                ChargeSlots = s.AvailableChargingSlots,
                Longitude = (long)s.StationLocation.Longitude,
                Lattitude= (long)s.StationLocation.Latitude
            };
            try
            {
                myDal.AddStation(tmp);
            }
            catch(IDAL.DO.ExistingObjectException stationExc)
            {
                throw new ExistingObjectException($"station station {s.Name} allready exists !!");
            }
        }

        /// <summary>
        /// adds a new drone to the drones' list
        /// </summary>
        /// <param name="d">drone to add</param>
        /// <param name="stationId">station number to put the drone in</param>
        public void AddDrone(DroneForList d, int stationId)
        {
            if (d.ID < 1000 || d.ID > 10000)
            {
                throw new BLInvalidInputException($"id {d.ID} is not valid !!");
            }
            if (String.IsNullOrEmpty(d.Model))
            {
                throw new BLInvalidInputException($"name {d.Model} is not correct !!");
            }
            List<IDAL.DO.Station> stations = (List<IDAL.DO.Station>)myDal.CopyStationArray();
            if (!(stations.Exists(station => station.ID == stationId)))
            {
                throw new BLInvalidInputException($"station {stationId} does not exists !!");
            }
            int index = stations.FindIndex(item => item.ID == stationId);
            d.Battery = rand.Next(20, 40);
            d.DroneStatuses =(DroneStatuses)1;
            d.CurrentLocation.Latitude = stations[index].Lattitude;
            d.CurrentLocation.Longitude = stations[index].Longitude;
            drones.Add(d);
            IDAL.DO.Drone tmp = new IDAL.DO.Drone
            {
                ID = d.ID,
                Model = d.Model,
                MaxWeight = (IDAL.DO.WeightCategories)d.MaxWeight
            };
            try
            {
                myDal.AddDrone(tmp);
            }
            catch(IDAL.DO.ExistingObjectException droneExc)
            {
                throw new ExistingObjectException($"station station {s.Name} allready exists !!");
            }
        }
    }

   
}
