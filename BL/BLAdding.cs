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
        public void AddStation(Station s)
        {
            s.DroneCharges = null;
            IDAL.DO.Station tmp = new IDAL.DO.Station
            {
                Id = s.Id,
                Name = s.Name,
                ChargeSlots = s.ChargeSlots,
                Longitude = (long)s.StationLocation.Longitude,
                Lattitude = (long)s.StationLocation.Latitude
            };
            try
            {
                myDal.AddStation(tmp);
            }
            catch (IDAL.DO.ExistingObjectException stationExc)
            {
                throw new FailedToAddException("ERROR", stationExc);
            }
        }

        /// <summary>
        /// adds a new drone to the drones' list
        /// </summary>
        /// <param name="d">drone to add</param>
        /// <param name="stationId">station number to put the drone in</param>
        public void AddDrone(DroneForList d, int stationId)
        {
            List<IDAL.DO.Station> stations = (List<IDAL.DO.Station>)myDal.CopyStationArray();
            //checks if the station exists
            //if (!(stations.Exists(station => station.Id == stationId)))
            //{
            //    throw new InputDoesNotExist($"station {stationId} does not exists !!");
            //}
            int index = stations.FindIndex(item => item.Id == stationId);//finds the station that the drone in it.
            IDAL.DO.Station s = new IDAL.DO.Station();
            s = stations[index];
            d.CurrentLocation = new();
            d.Battery = rand.Next(20, 40);
            d.DroneStatuses = DroneStatuses.Maintenance;
            d.CurrentLocation.Latitude = stations[index].Lattitude;
            d.CurrentLocation.Longitude = stations[index].Longitude;
            DroneCharge droneCharge = new();
            droneCharge.Id = d.Id;
            droneCharge.Battery = d.Battery;
            Station blStation = new();
            blStation = GetStation(stationId);
            blStation.DroneCharges.Add(droneCharge);
            drones.Add(d);
            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = (IDAL.DO.Weight)d.Weight
            };
            myDal.SendDroneToChargeSlot(dalDrone, s);
            try
            {
                myDal.AddDrone(dalDrone);
            }
            catch (IDAL.DO.ExistingObjectException droneExc)
            {
                throw new FailedToAddException("ERROR", droneExc);
            }
        }

        /// <summary>
        /// adds customer to customers list
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomer(Customer customer)
        {          
            IDAL.DO.Customer newCustomer = new();
            object obj = newCustomer;
            customer.CopyPropertiesTo(obj);
            newCustomer = (IDAL.DO.Customer)obj;
            customer.CopyPropertiesTo(newCustomer);
            newCustomer.Lattitude = customer.Location.Latitude;
            newCustomer.Longtitude = customer.Location.Longitude;
            try
            {
                myDal.AddCustomer(newCustomer);
            }
            catch (IDAL.DO.ExistingObjectException custEx)
            {
                throw new FailedToAddException("ERROR", custEx);
            }

        }

        /// <summary>
        /// adds parcel to parcel list
        /// </summary>
        /// <param name="parcel"></param>
        public int AddParcel(Parcel parcel)
        {
            parcel.DroneInParcel = default;
            //the parcel has been made.
            parcel.Requested = DateTime.Now;
            parcel.Scheduled = DateTime.MinValue;
            parcel.Delivered = DateTime.MinValue;
            parcel.PickedUp = DateTime.MinValue;
            IDAL.DO.Parcel newParcel = new();
            object obj = newParcel;
            parcel.CopyPropertiesTo(obj);
            newParcel = (IDAL.DO.Parcel)obj;
            parcel.CopyPropertiesTo(newParcel);
            newParcel.DroneID = 0;
            newParcel.SenderID = parcel.Sender.Id;
            newParcel.TargetID = parcel.Recipient.Id;
            int runningNumber;
            try
            {
                runningNumber=myDal.AddParcel(newParcel);
            }
            catch (IDAL.DO.ExistingObjectException parEx)
            {
                throw new FailedToAddException("ERROR", parEx);
            }
            return runningNumber;
        }

    }



}



