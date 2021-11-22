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
            parcel.CopyPropertyTo(dalParcel);
            try
            {
                IDAL.DO.Customer dalSender = myDal.GetCustomer(dalParcel.SenderID);
            }
            catch (IDAL.DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException(custEx.ToString(), custEx);
            }
            parcel.Sender.CopyPropertyTo(dalSender);
            try
            {
                IDAL.DO.Customer dalRecipient = myDal.GetCustomer(dalParcel.TargetID);
            }
            catch (IDAL.DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException(custEx.ToString(), custEx);
            }
            parcel.Recipient.CopyPropertyTo(dalRecipient);
            if (dalParcel.ID == 0)//the parcel hasnt been atributted
                parcel.DroneInParcel = default;
            else
            {
                try
                {
                    Drone droneInParcel = GetDrone(dalParcel.DroneID);
                }
                catch (IDAL.DO.UnvalidIDException custEx)
                {
                    throw new FailedToGetException(custEx.ToString(), custEx);
                }
                parcel.DroneInParcel.CopyPropertyTo(droneInParcel);
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
            returningDrone.CopyPropertiesTo(droneForList);
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


        }

        private Station GetStation(int stationId)
        {
            throw new NotImplementedException();
        }

    }


}




