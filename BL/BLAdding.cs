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
                throw new InvalidInputException($"id {s.Id} is not valid !!");
            }
            if(s.AvailableChargingSlots<0 || s.AvailableChargingSlots>50)
            {
                throw new InvalidInputException($"number of slots {s.AvailableChargingSlots} is not valid !!");
            }
            if(String.IsNullOrEmpty(s.Name))
            {
                throw new InvalidInputException($"name {s.Name} is not correct !!");
            }
            if(s.StationLocation.Latitude<-5000 || s.StationLocation.Latitude>5000 || s.StationLocation.Longitude <-5000 || s.StationLocation.Longitude >5000)
            {
                throw new InvalidInputException($"location {s.StationLocation} is not valid !!");
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
                throw new InvalidInputException($"id {d.ID} is not valid !!");
            }
            if (String.IsNullOrEmpty(d.Model))
            {
                throw new InvalidInputException($"name {d.Model} is not correct !!");
            }
            IDAL.DO.Drone tmp = new IDAL.DO.Drone
            {
                
            };
        }

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
            customer.CopyPropertyTo(newCustomer);
            try
            {
                myDal.AddCustomer(newCustomer);
            }
            catch (IDAL.DO.ExistingObjectException custEx)
            {
                throw new FailedToAddException($"");
            }
        }

        public void AddParcel(Parcel parcel)
        {
            if (parcel.Sender.Id < 100000000 || parcel.Sender.Id > 999999999)
                throw new InvalidInputException($"The sender ID {parcel.Sender.Id} is not valid !!");
            if (parcel.Recipient.Id < 100000000 || parcel.Recipient.Id > 999999999)
                throw new InvalidInputException($"The Recipient ID {parcel.Recipient.Id} is not valid !!");
            IDAL.DO.Parcel newParcel = new();
            newParcel.CopyPropertyTo(newParcel);
        }


    }


}
