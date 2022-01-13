using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;



namespace BL
{
     partial class BL
    {
        /// <summary>
        /// adds a new station to the station list
        /// </summary>
        /// <param name="myID">station id</param>
        /// <param name="name">station name</param>
        /// <param name="numOfSlots">number of available charge slots in station</param>
        /// <param name="location">station's location</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station s)
        {
            if (s.Id < 1000 || s.Id > 10000)
                throw new InvalidInputException($"id {s.Id} is not valid !!");
            if(string.IsNullOrEmpty(s.Name))
                throw new InvalidInputException($"name is not valid !!");
            if(s.ChargeSlots<0||s.ChargeSlots>50)
                throw new InvalidInputException($"number of slots {s.DroneCharges} is not valid !!");
            if (s.StationLocation.Longitude > 35.195 || s.StationLocation.Longitude < 35.2)
                throw new InvalidInputException($"Longitude {s.StationLocation.Longitude} is not valid !!");
            if (s.StationLocation.Latitude > 31.75 || s.StationLocation.Latitude < 31.9)
                throw new InvalidInputException($"Lattitude {s.StationLocation.Latitude} is not valid !!");
            s.DroneCharges = null;
            DO.Station tmp = new DO.Station
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
            catch (DO.ExistingObjectException stationExc)
            {
                throw new FailedToAddException("ERROR", stationExc);
            }
        }

        /// <summary>
        /// adds a new drone to the drones' list
        /// </summary>
        /// <param name="d">drone to add</param>
        /// <param name="stationId">station number to put the drone in</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(DroneForList d, int stationId)
        {
            if(d.Id < 1000 ||d.Id > 10000)
                throw new InvalidInputException($"id {d.Id} is not valid !!");
            if (string.IsNullOrEmpty(d.Model))
                throw new InvalidInputException($"model is not valid !!");
            if (stationId < 1000 || stationId > 10000)
                throw new InvalidInputException($"station Id {stationId} is not valid !!");
            List<DO.Station> stations = myDal.CopyStationArray().ToList();
            //checks if the station exists
            if (!stations.Exists(station => station.Id == stationId))
            {
                throw new InputDoesNotExist($"station {stationId} does not exists !!");
            }
            int index = stations.FindIndex(item => item.Id == stationId);//finds the station that the drone in it.
            DO.Station s = new DO.Station();
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
            DO.Drone dalDrone = new DO.Drone
            {
                Id = d.Id,
                Model = d.Model,
                Weight = (DO.Weight)d.Weight
            };
            myDal.SendDroneToChargeSlot(dalDrone, s);
            try
            {
                myDal.AddDrone(dalDrone);
            }
            catch (DO.ExistingObjectException droneExc)
            {
                throw new FailedToAddException("ERROR", droneExc);
            }
        }

        /// <summary>
        /// adds customer to customers list
        /// </summary>
        /// <param name="customer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer customer)
        {
            if (customer.Id < 100000000 || customer.Id > 1000000000)
                throw new InvalidInputException($"id {customer.Id} is not valid !!");
            if (String.IsNullOrEmpty(customer.Name))
                throw new InvalidInputException($"name is not valid !!");
            if (String.IsNullOrEmpty(customer.PhoneNumber))
                throw new InvalidInputException($"phone number is not valid !!");
            if (customer.PhoneNumber.Length!=10)
                throw new InvalidInputException($"phone number {customer.PhoneNumber} is not valid !!");
            if (customer.Location.Longitude > 35.195 || customer.Location.Longitude < 35.2)
                throw new InvalidInputException($"Longitude {customer.Location.Longitude} is not valid !!");
            if (customer.Location.Latitude > 31.75 || customer.Location.Latitude < 31.9)
                throw new InvalidInputException($"Lattitude {customer.Location.Latitude} is not valid !!");
            DO.Customer newCustomer = new();
            object obj = newCustomer;
            customer.CopyPropertiesTo(obj);
            newCustomer = (DO.Customer)obj;
            customer.CopyPropertiesTo(newCustomer);
            newCustomer.Lattitude = customer.Location.Latitude;
            newCustomer.Longtitude = customer.Location.Longitude;
            try
            {
                myDal.AddCustomer(newCustomer);
                string dirName = AppDomain.CurrentDomain.BaseDirectory; // Starting Dir
                FileInfo fileInfo = new FileInfo(dirName);
                DirectoryInfo parentDir = fileInfo.Directory.Parent.Parent.Parent;
                string parentDirName = parentDir.FullName;
            }
            catch (DO.ExistingObjectException custEx)
            {
                throw new FailedToAddException("ERROR", custEx);
            }

        }

        /// <summary>
        /// adds parcel to parcel list
        /// </summary>
        /// <param name="parcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddParcel(Parcel parcel)
        {
            if (parcel.Sender.Id < 100000000 || parcel.Sender.Id > 1000000000)
                throw new InvalidInputException($"sender id {parcel.Sender.Id} is not valid !!");
            if (parcel.Recipient.Id < 100000000 || parcel.Recipient.Id > 1000000000)
                throw new InvalidInputException($"recipient id {parcel.Recipient.Id} is not valid !!");
            parcel.DroneInParcel = default;
            //the parcel has been made.
            parcel.Requested = DateTime.Now;
            parcel.Scheduled = null;
            parcel.Delivered = null;
            parcel.PickedUp = null;
            DO.Parcel newParcel = new();
            object obj = newParcel;
            parcel.CopyPropertiesTo(obj);
            newParcel = (DO.Parcel)obj;
            parcel.CopyPropertiesTo(newParcel);
            newParcel.DroneID = 0;
            newParcel.SenderID = parcel.Sender.Id;
            newParcel.TargetID = parcel.Recipient.Id;
            newParcel.IsDeleted = false;
            int runningNumber;
            try
            {
                runningNumber=myDal.AddParcel(newParcel);
            }
            catch (DO.ExistingObjectException parEx)
            {
                throw new FailedToAddException("ERROR", parEx);
            }
            return runningNumber;
        }

    }
}



