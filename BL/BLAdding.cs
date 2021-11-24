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
            if (s.Id < 1000 || s.Id > 10000)
            {
                throw new InvalidInputException($"id {s.Id} is not valid !!");
            }
            if (s.AvailableChargingSlots < 0 || s.AvailableChargingSlots > 50)
            {
                throw new InvalidInputException($"number of slots {s.AvailableChargingSlots} is not valid !!");
            }
            if (String.IsNullOrEmpty(s.Name))
            {
                throw new InvalidInputException($"name {s.Name} is not correct !!");
            }
            if (s.StationLocation.Latitude < -5000 || s.StationLocation.Latitude > 5000 || s.StationLocation.Longitude < -5000 || s.StationLocation.Longitude > 5000)
            {
                throw new InvalidInputException($"location {s.StationLocation} is not valid !!");
            }
            s.DroneCharges = null;
            IDAL.DO.Station tmp = new IDAL.DO.Station
            {
                ID = s.Id,
                StationName = s.Name,
                ChargeSlots = s.AvailableChargingSlots,
                Longitude = (long)s.StationLocation.Longitude,
                Lattitude = (long)s.StationLocation.Latitude
            };
            try
            {
                myDal.AddStation(tmp);
            }
            catch (IDAL.DO.ExistingObjectException stationExc)
            {
                throw new FailedToAddException(stationExc.ToString(), stationExc);
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
                throw new InvalidInputException($"id {d.ID} is not valid !!");
            }
            if (String.IsNullOrEmpty(d.Model))
            {
                throw new InvalidInputException($"name {d.Model} is not correct !!");
            }
            List<IDAL.DO.Station> stations = (List<IDAL.DO.Station>)myDal.CopyStationArray();
            //checks if the station exists
            if (!(stations.Exists(station => station.ID == stationId)))
            {
                throw new InputDoesNotExist($"station {stationId} does not exists !!");
            }
            int index = stations.FindIndex(item => item.ID == stationId);//finds the station that the drone in it.
            IDAL.DO.Station s = new IDAL.DO.Station();
            s = stations[index];
            d.Battery = rand.Next(20, 40);
            d.DroneStatuses = DroneStatuses.Maintenance;
            d.CurrentLocation.Latitude = stations[index].Lattitude;
            d.CurrentLocation.Longitude = stations[index].Longitude;
            DroneCharge droneCharge = new();
            droneCharge.Id = d.ID;
            droneCharge.Battery = d.Battery;
            Station blStation = new();
            blStation = GetStation(stationId);
            blStation.DroneCharges.Add(droneCharge);
            drones.Add(d);
            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone
            {
                ID = d.ID,
                Model = d.Model,
                MaxWeight = (IDAL.DO.WeightCategories)d.MaxWeight
            };
            myDal.SendDroneToChargeSlot(dalDrone, s);
            try
            {
                myDal.AddDrone(dalDrone);
            }
            catch (IDAL.DO.ExistingObjectException droneExc)
            {
                throw new FailedToAddException(droneExc.ToString(), droneExc);
            }
        }

        /// <summary>
        /// adds customer to customers list
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomer(Customer customer)
        {
            if (customer.Id < 100000000 || customer.Id > 999999999)
                throw new InvalidInputException($"ID {customer.Id} is not valid !!");
            if (String.IsNullOrEmpty(customer.Name))
                throw new InvalidInputException($"Name {customer.Name} is not correct !!");
            if (customer.Phone.Length != 10)
                throw new InvalidInputException($"Phone number {customer.Phone} is too short!");
            if (customer.Location.Latitude < -5000 || customer.Location.Latitude > 5000 || customer.Location.Longitude < -5000 || customer.Location.Longitude > 5000)
            {
                throw new InvalidInputException($"location data: {customer.Location} is not valid !!");
            }
            IDAL.DO.Customer newCustomer = new();
            customer.CopyPropertiesTo(newCustomer);
            try
            {
                myDal.AddCustomer(newCustomer);
            }
            catch (IDAL.DO.ExistingObjectException custEx)
            {
                throw new FailedToAddException(custEx.ToString(), custEx);
            }

        }

        /// <summary>
        /// adds parcel to parcel list
        /// </summary>
        /// <param name="parcel"></param>
        public int AddParcel(Parcel parcel)
        {
            if (parcel.Sender.Id < 100000000 || parcel.Sender.Id > 999999999)
                throw new InvalidInputException($"The sender ID {parcel.Sender.Id} is not valid !!");
            if (parcel.Recipient.Id < 100000000 || parcel.Recipient.Id > 999999999)
                throw new InvalidInputException($"The Recipient ID {parcel.Recipient.Id} is not valid !!");
            parcel.DroneInParcel = default;
            //the parcel has been made.
            parcel.Requested = DateTime.Now;
            parcel.Scheduled = DateTime.MinValue;
            parcel.Delivered = DateTime.MinValue;
            parcel.PickedUp = DateTime.MinValue;
            IDAL.DO.Parcel newParcel = new();
            parcel.CopyPropertiesTo(newParcel);
            newParcel.DroneID = 0;
            int runningNumber;
            try
            {
                runningNumber=myDal.AddParcel(newParcel);
            }
            catch (IDAL.DO.ExistingObjectException parEx)
            {
                throw new FailedToAddException(parEx.ToString(), parEx);
            }
            return runningNumber;
        }

    }



}


