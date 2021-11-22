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
        /// return parcel in the list by its recieved id.
        /// </summary>
        /// <param name="parcelId">for finding the parcel</param>
        /// <returns>the parcel from the list</returns>
        public Parcel GetParcel(int parcelId)
        {

            IDAL.DO.Parcel dalParcel = myDal.GetParcel(parcelId);
            Parcel parcel = new();//the parcel to return
            parcel.CopyPropertiesTo(dalParcel);
            IDAL.DO.Customer dalSender = new IDAL.DO.Customer();
            try
            {
                 dalSender = myDal.GetCustomer(dalParcel.SenderID);
            }
            catch (IDAL.DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException(custEx.ToString(), custEx);
            }
            parcel.Sender.CopyPropertiesTo(dalSender);
            IDAL.DO.Customer dalRecipient = new IDAL.DO.Customer();
            try
            {
              dalRecipient = myDal.GetCustomer(dalParcel.TargetID);
            }
            catch (IDAL.DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException(custEx.ToString(), custEx);
            }
            parcel.Recipient.CopyPropertiesTo(dalRecipient);
            if (dalParcel.ID == 0)//the parcel hasnt been atributted
                parcel.DroneInParcel = default;
            else
            {
                Drone droneInParcel = new();
                try
                {
                  droneInParcel = GetDrone(dalParcel.DroneID);
                }
                catch (IDAL.DO.UnvalidIDException custEx)
                {
                    throw new FailedToGetException(custEx.ToString(), custEx);
                }
                parcel.DroneInParcel.CopyPropertiesTo(droneInParcel);
            }
            return parcel;
        }

        /// <summary>
        /// return drone in the list by its recieved id.
        /// </summary>
        /// <param name="droneID">for finding the drone</param>
        /// <returns>the drone from the list</returns>
        public Drone GetDrone(int droneID)
        {
            DroneForList droneForList = drones.Find(item => item.ID == droneID);
            if (droneForList == default)
                throw new InputDoesNotExist($"ID {droneID} does not exist in the drone list");
            Drone returningDrone = new();
            //gets the fields from the drone list
            droneForList.CopyPropertiesTo(returningDrone);
            //checks if there is  parcel atributted to the drone 
            if (droneForList.ParcelId == 0)
                returningDrone.ParcelInDelivery = default;
            else
            {
                IDAL.DO.Parcel dalParcel =myDal.GetParcel(droneForList.ParcelId);
                Parcel parcel = GetParcel(dalParcel.ID);
                returningDrone.ParcelInDelivery.Id = parcel.Id;
                if (parcel.PickedUp ==DateTime.MinValue)
                    returningDrone.ParcelInDelivery.OnTheWay = true;
                else
                    returningDrone.ParcelInDelivery.OnTheWay = false;
                returningDrone.ParcelInDelivery.Weight = parcel.Weight;
                Customer sender = GetCustomer(parcel.Sender.Id);
                Customer reciever = GetCustomer(parcel.Recipient.Id);
                returningDrone.ParcelInDelivery.CustomerSender.Id = parcel.Sender.Id;
                returningDrone.ParcelInDelivery.CustomerSender.Name = parcel.Sender.Name;
                returningDrone.ParcelInDelivery.CustomerReciever.Name = parcel.Recipient.Name;
                returningDrone.ParcelInDelivery.CustomerReciever.Id = parcel.Recipient.Id;
                returningDrone.ParcelInDelivery.Collection = sender.Location;
                returningDrone.ParcelInDelivery.Destination = reciever.Location;                
            }
            return returningDrone;
        }

        public Customer GetCustomer(int customerId)
        {
            Customer returningCustomer = new();
            try
            {
                IDAL.DO.Customer dalCustomer = myDal.GetCustomer(customerId);
                returningCustomer.CopyPropertiesTo(dalCustomer);
                returningCustomer.Location.Latitude = dalCustomer.Lattitude;
                returningCustomer.Location.Longitude = dalCustomer.Longtitude;
            }
            catch (IDAL.DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException(custEx.ToString(), custEx);
            }

            return returningCustomer;
        }

        /// <summary>
        /// gets a station by the id and update the other fields by the fields in dal
        /// </summary>
        /// <param name="stationId">for getting the object</param>
        /// <returns>the object</returns>
        private Station GetStation(int stationId)
        {
            Station returningStation = new();
            /*DAL.DO.Station dalStation = myDal.CopyStationArray().First(item => item.ID == stationId);*/
            IDAL.DO.Station dalStation = new IDAL.DO.Station();
            try
            {
                dalStation = myDal.GetStation(stationId);
            }
            catch (IDAL.DO.UnvalidIDException stationEx)
            {
                throw new FailedToGetException(stationEx.ToString(), stationEx);
            }
            dalStation.CopyPropertiesTo(returningStation);
            //returningStation.StationLocation.Latitude = dalStation.Lattitude;
            //returningStation.StationLocation.Longitude = dalStation.Longitude;
            List<DroneCharge> droneCharges = FindListOfDroneLIstForStation(stationId);
            returningStation.DroneCharges = droneCharges;
            return returningStation;
        }

        /// <summary>
        /// creates list that contains all the charge slots of specific station.
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns>th ecreated station </returns>
        private List<DroneCharge> FindListOfDroneLIstForStation(int stationId)
        {
            DroneCharge droneToAdd = new DroneCharge();
            List<DroneCharge> droneChargesToReturn = new List<DroneCharge>();
            //goes through the drone charges
            foreach (var droneList in myDal.GetDroneChargeList())
            {
                //if the drone charge is in the station
                if(droneList.StationID== stationId)
                {
                    //updates his fields and add to returning list
                    droneToAdd.Id = droneList.DroneID;
                    DroneForList drone = drones.First(item => item.ID == droneList.DroneID);
                    droneToAdd.Battery = drone.Battery;
                    droneChargesToReturn.Add(droneToAdd);
                }
            }
            return droneChargesToReturn;
        }
    }


}




