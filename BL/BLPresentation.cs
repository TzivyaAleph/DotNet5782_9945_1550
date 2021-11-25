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
            if (parcelId < 200)
                throw new InvalidInputException($"parcel id {parcelId} is not valid !!");
            IDAL.DO.Parcel dalParcel = new IDAL.DO.Parcel();
            //gets parcel from dal
            try
            {
               dalParcel = myDal.GetParcel(parcelId);
            }
            catch (IDAL.DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException("ERROR", custEx);
            }
            Parcel parcel = new();//the parcel to return
            dalParcel.CopyPropertiesTo(parcel);
            IDAL.DO.Customer dalSender = new IDAL.DO.Customer();
            dalSender = new IDAL.DO.Customer();
            try
            {
                 dalSender = myDal.GetCustomer(dalParcel.SenderID);
            }
            catch (IDAL.DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException("ERROR", custEx);
            }
            parcel.Sender.CopyPropertiesTo(dalSender);
            IDAL.DO.Customer dalRecipient = new IDAL.DO.Customer();
            try
            {
              dalRecipient = myDal.GetCustomer(dalParcel.TargetID);
            }
            catch (IDAL.DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException("ERROR", custEx);
            }
            parcel.Recipient = new();
            parcel.Recipient.CopyPropertiesTo(dalRecipient);
            if (dalParcel.Id == 0)//the parcel hasnt been atributted
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
                    throw new FailedToGetException("ERROR", custEx);
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
            if (droneID < 1000 || droneID > 10000)
            {
                throw new InvalidInputException($"id {droneID} is not valid !!");
            }
            DroneForList droneForList = drones.Find(item => item.Id == droneID);
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
                IDAL.DO.Parcel dalParcel = new IDAL.DO.Parcel();
                try
                {
                    dalParcel = myDal.GetParcel(droneForList.ParcelId);
                    //finds the parcel in bl
                    Parcel parcel = new();
                    parcel = GetParcel(dalParcel.Id);
                    returningDrone.ParcelInDelivery.Id = parcel.Id;
                    //checks if the parcel wasnt picked up
                    if (parcel.PickedUp == DateTime.MinValue)
                        returningDrone.ParcelInDelivery.OnTheWay = false;
                    else
                        returningDrone.ParcelInDelivery.OnTheWay = true;
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
                catch (IDAL.DO.UnvalidIDException DroneEx)
                {
                    throw new FailedToGetException("ERROR", DroneEx);
                }
            }
            return returningDrone;
        }

        /// <summary>
        /// gets a customer by its id and from dal data.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>the customer</returns>
        public Customer GetCustomer(int customerId)
        {
            if (customerId < 100000000 || customerId > 999999999)
                throw new InvalidInputException($"ID {customerId} is not valid !!");
            Customer returningCustomer = new();
            try
            {
                IDAL.DO.Customer dalCustomer = myDal.GetCustomer(customerId);
                dalCustomer.CopyPropertiesTo(returningCustomer);
                returningCustomer.Location.Latitude = dalCustomer.Lattitude;
                returningCustomer.Location.Longitude = dalCustomer.Longtitude;
                List<ParcelCustomer> Parcels = new List<ParcelCustomer>();
                Parcels = getParcelToSend(returningCustomer);
                returningCustomer.SentParcels = Parcels;
                Parcels = getParcelToTarget(returningCustomer);
                returningCustomer.ReceiveParcels = Parcels;
            }
            catch (IDAL.DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException("ERROR", custEx);
            }

            return returningCustomer;
        }

        /// <summary>
        /// creates list with all the parcels that a specific customer recieve.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private List<ParcelCustomer> getParcelToTarget(Customer customer)
        {
            List<ParcelCustomer> listToReturn = new List<ParcelCustomer>();
            //goes through the parcel list.
            foreach (var par in myDal.CopyParcelArray())
            {
                ParcelCustomer parcelToAdd = new();
                //checks if its  the senders parcel 
                if (par.TargetID == customer.Id)
                {
                    parcelToAdd.Id = par.Id;
                    parcelToAdd.Weight = (Weight)par.Weight;
                    parcelToAdd.Priority = (Priority)par.Priority;
                    //checks the parcel status and updates the field.
                    parcelToAdd.Status = getStatus(par);
                    //update the fields in customerParcel=the taget of the parcel data
                    parcelToAdd.CustomerParcel.Id = par.TargetID;
                    IDAL.DO.Customer dalTarget = new IDAL.DO.Customer();
                    dalTarget = myDal.CopyCustomerArray().First(item => item.Id == par.TargetID);
                    parcelToAdd.CustomerParcel.Name = dalTarget.Name;
                    listToReturn.Add(parcelToAdd);
                }

            }
            return listToReturn;
        }

        /// <summary>
        /// creates list with all the parcels that a specific customer send.
        /// </summary>
        /// <param name="customer">the sender</param>
        /// <returns>the created list</returns>
        private List<ParcelCustomer> getParcelToSend(Customer customer)
        {
            List<ParcelCustomer> listToReturn = new List<ParcelCustomer>();
            //goes through the parcel list.
            foreach (var par in myDal.CopyParcelArray())
            {
                ParcelCustomer parcelToAdd = new();
                //checks if its  the senders parcel 
                if (par.SenderID== customer.Id)
                {
                    parcelToAdd.Id = par.Id;
                    parcelToAdd.Weight =(Weight) par.Weight;
                    parcelToAdd.Priority =(Priority) par.Priority;
                    //checks the parcel status and updates the field.
                    if (par.Delivered != DateTime.MinValue)
                        parcelToAdd.Status = Status.Delivered;
                    else if (par.PickedUp != DateTime.MinValue)
                        parcelToAdd.Status = Status.Picked;
                    else if (par.Scheduled != DateTime.MinValue)
                        parcelToAdd.Status = Status.Assigned;
                    else
                        parcelToAdd.Status = Status.Created;
                    //update the fields in customerParcel=the target of the parcel data
                    parcelToAdd.CustomerParcel.Id = par.TargetID;
                    IDAL.DO.Customer dalTarget = new IDAL.DO.Customer();
                    dalTarget = myDal.CopyCustomerArray().First(item => item.Id == par.TargetID);
                    parcelToAdd.CustomerParcel.Name = dalTarget.Name;
                    listToReturn.Add(parcelToAdd);
                }

            }
            return listToReturn;
        }

        /// <summary>
        /// gets a station by the id and update the other fields by the fields in dal
        /// </summary>
        /// <param name="stationId">for getting the object</param>
        /// <returns>the object</returns>
        public Station GetStation(int stationId)
        {
            if (stationId < 1000 || stationId > 10000)
            {
                throw new InvalidInputException($"id {stationId} is not valid !!");
            }
            Station returningStation = new();
            /*DAL.DO.Station dalStation = myDal.CopyStationArray().First(item => item.ID == stationId);*/
            IDAL.DO.Station dalStation = new IDAL.DO.Station();
            try
            {
                dalStation = myDal.GetStation(stationId);
            }
            catch (IDAL.DO.UnvalidIDException stationEx)
            {
                throw new FailedToGetException("ERROR", stationEx);
            }
            returningStation.CopyPropertiesTo(dalStation);
            returningStation.StationLocation.Latitude = dalStation.Lattitude;
            returningStation.StationLocation.Longitude = dalStation.Longitude;
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
            List<DroneCharge> droneChargesToReturn = new List<DroneCharge>();
            //goes through the drone charges
            foreach (var droneList in myDal.GetDroneChargeList())
            {
                DroneCharge droneToAdd = new DroneCharge();
                //if the drone charge is in the station
                if (droneList.StationID== stationId)
                {
                    //updates his fields and add to returning list
                    droneToAdd.Id = droneList.DroneID;
                    DroneForList drone = new();
                    try
                    {
                      drone = drones.First(item => item.Id == droneList.DroneID);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new InputDoesNotExist("the drone does not exist!!");
                    }
                    droneToAdd.Battery = drone.Battery;
                    droneChargesToReturn.Add(droneToAdd);
                }
            }
            return droneChargesToReturn;
        }
    }


}




