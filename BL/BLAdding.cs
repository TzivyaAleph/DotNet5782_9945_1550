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
                throw new BLIdException($"id {s.Id} is not valid !!");
            }
            if(s.AvailableChargingSlots<0 || s.AvailableChargingSlots>50)
            {
                throw new BLInvalidNumberException($"number of slots {s.AvailableChargingSlots} is not valid !!");
            }
            if(String.IsNullOrEmpty(s.Name))
            {
                throw new BLInvalidStringException($"name {s.Name} is not correct !!");
            }
            if(s.StationLocation.Latitude<-5000 || s.StationLocation.Latitude>5000 || s.StationLocation.Longitude <-5000 || s.StationLocation.Longitude >5000)
            {
                throw new InvalidLocationException($"location {s.StationLocation} is not valid !!");
            }
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
                
            }
        }

        public void AddDrone(Drone d)
        {
            if (d.ID < 1000 || d.ID > 10000)
            {
                throw new BLIdException($"id {d.ID} is not valid !!");
            }
            if (String.IsNullOrEmpty(d.Model))
            {
                throw new BLInvalidStringException($"name {d.Model} is not correct !!");
            }
            IDAL.DO.Drone tmp = new IDAL.DO.Drone
            {
                
            };
        }
    }
}
