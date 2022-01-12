using System;
using System.Collections.Generic;
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
        /// return parcel in the list by its recieved id.
        /// </summary>
        /// <param name="parcelId">for finding the parcel</param>
        /// <returns>the parcel from the list</returns>
        public Parcel GetParcel(int parcelId)
        {
            if (parcelId < 200)
                throw new InvalidInputException($"parcel id {parcelId} is not valid !!");
            DO.Parcel dalParcel = new DO.Parcel();
            //gets parcel from dal
            try
            {
                dalParcel = myDal.GetParcel(parcelId);
            }
            catch (DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException("ERROR", custEx);
            }
            Parcel parcel = new();//the parcel to return
            parcel.Recipient = new();
            parcel.Sender = new();
            parcel.DroneInParcel = new();
            dalParcel.CopyPropertiesTo(parcel);
            DO.Customer dalSender = new DO.Customer();
            dalSender = new DO.Customer();
            try
            {
                dalSender = myDal.GetCustomer(dalParcel.SenderID);
            }
            catch (DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException("ERROR", custEx);
            }
            parcel.Sender = new();
            dalSender.CopyPropertiesTo(parcel.Sender);
            DO.Customer dalRecipient = new DO.Customer();
            try
            {
                dalRecipient = myDal.GetCustomer(dalParcel.TargetID);
            }
            catch (DO.UnvalidIDException custEx)
            {
                throw new FailedToGetException("ERROR", custEx);
            }
            parcel.Recipient = new();
            dalRecipient.CopyPropertiesTo(parcel.Recipient);
            parcel.DroneInParcel = new();
            if (dalParcel.Id == 0)//the parcel hasnt been atributted
                parcel.DroneInParcel = default;
            else
            {
                DroneForList droneInParcel = new();
                droneInParcel = drones.FirstOrDefault(item => item.ParcelId == dalParcel.Id);
                if (droneInParcel != null)
                {
                    parcel.DroneInParcel = new();
                    droneInParcel.CopyPropertiesTo(parcel.DroneInParcel);
                    parcel.DroneInParcel.Location = new();
                    parcel.DroneInParcel.Location.Latitude = droneInParcel.CurrentLocation.Latitude;
                    parcel.DroneInParcel.Location.Longitude = droneInParcel.CurrentLocation.Longitude;

                }
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
                throw new InvalidInputException($"drone id {droneID} is not valid !!");
            }
            DroneForList droneForList = drones.Find(item => item.Id == droneID);
            if (droneForList == default)
                throw new InputDoesNotExist($"ID {droneID} does not exist in the drone list");
            Drone returningDrone = new();
            //gets the fields from the drone list
            returningDrone.CurrentLocation = new();
            droneForList.CopyPropertiesTo(returningDrone);
            returningDrone.CurrentLocation.Latitude = droneForList.CurrentLocation.Latitude;
            returningDrone.CurrentLocation.Longitude = droneForList.CurrentLocation.Longitude;
            //checks if there is  parcel atributted to the drone 
            if (droneForList.ParcelId == 0)
                returningDrone.ParcelInDelivery = default;
            else
            {
                DO.Parcel dalParcel = new DO.Parcel();
                try
                {
                    dalParcel = myDal.GetParcel(droneForList.ParcelId);
                    //finds the parcel in bl
                    Parcel parcel = new();
                    parcel = GetParcel(dalParcel.Id);
                    returningDrone.ParcelInDelivery = new();
                    returningDrone.ParcelInDelivery.Id = parcel.Id;
                    //checks if the parcel wasnt picked up
                    if (parcel.PickedUp == null)
                        returningDrone.ParcelInDelivery.OnTheWay = false;
                    else
                        returningDrone.ParcelInDelivery.OnTheWay = true;
                    returningDrone.ParcelInDelivery.Weight = parcel.Weight;
                    Customer sender = GetCustomer(parcel.Sender.Id);
                    Customer reciever = GetCustomer(parcel.Recipient.Id);
                    returningDrone.ParcelInDelivery.CustomerSender = new();
                    returningDrone.ParcelInDelivery.CustomerReciever = new();
                    returningDrone.ParcelInDelivery.CustomerSender.Id = parcel.Sender.Id;
                    returningDrone.ParcelInDelivery.CustomerSender.Name = parcel.Sender.Name;
                    returningDrone.ParcelInDelivery.CustomerReciever.Name = parcel.Recipient.Name;
                    returningDrone.ParcelInDelivery.CustomerReciever.Id = parcel.Recipient.Id;
                    returningDrone.ParcelInDelivery.Collection = new();
                    returningDrone.ParcelInDelivery.Collection = sender.Location;
                    returningDrone.ParcelInDelivery.Transportation = new();
                    returningDrone.ParcelInDelivery.Transportation = (int)myDal.getDistanceFromLatLonInKm(sender.Location.Latitude, sender.Location.Longitude, reciever.Location.Latitude, reciever.Location.Longitude) * 1000;
                    returningDrone.ParcelInDelivery.Destination = reciever.Location;
                }
                catch (DO.UnvalidIDException DroneEx)
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
            {
                throw new InvalidInputException($"id {customerId} is not valid !!");
            }
            Customer returningCustomer = new();
            try
            {
                DO.Customer dalCustomer = myDal.GetCustomer(customerId);
                dalCustomer.CopyPropertiesTo(returningCustomer);
                returningCustomer.Location = new();
                returningCustomer.Location.Latitude = dalCustomer.Lattitude;
                returningCustomer.Location.Longitude = dalCustomer.Longtitude;
                List<ParcelCustomer> Parcels = new List<ParcelCustomer>();
                Parcels = getParcelToSend(returningCustomer);
                returningCustomer.SentParcels = new();
                returningCustomer.SentParcels = Parcels;
                Parcels = getParcelToTarget(returningCustomer);
                returningCustomer.ReceiveParcels = new();
                returningCustomer.ReceiveParcels = Parcels;
            }
            catch (DO.UnvalidIDException custEx)
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
                    parcelToAdd.CustomerParcel = new();
                    parcelToAdd.CustomerParcel.Id = par.TargetID;
                    DO.Customer dalTarget = new DO.Customer();
                    try
                    {
                        dalTarget = myDal.CopyCustomerArray().First(item => item.Id == par.TargetID);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new InputDoesNotExist("the recipient does not exist !!");
                    }
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
                if (par.SenderID == customer.Id)
                {
                    parcelToAdd.Id = par.Id;
                    parcelToAdd.Weight = (Weight)par.Weight;
                    parcelToAdd.Priority = (Priority)par.Priority;
                    //checks the parcel status and updates the field.
                    if (par.Delivered != null)
                        parcelToAdd.Status = Status.Delivered;
                    else if (par.PickedUp != null)
                        parcelToAdd.Status = Status.Picked;
                    else if (par.Scheduled != null)
                        parcelToAdd.Status = Status.Assigned;
                    else
                        parcelToAdd.Status = Status.Created;
                    //update the fields in customerParcel=the target of the parcel data
                    parcelToAdd.CustomerParcel = new();
                    parcelToAdd.CustomerParcel.Id = par.TargetID;
                    DO.Customer dalTarget = new DO.Customer();
                    try
                    {
                        dalTarget = myDal.CopyCustomerArray().First(item => item.Id == par.TargetID);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new InputDoesNotExist("the recipient does not exist !!");
                    }
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
                throw new InvalidInputException($"id {stationId} is not valid !!");
            Station returningStation = new();
            DO.Station dalStation = new DO.Station();
            try
            {
                dalStation = myDal.GetStation(stationId);
            }
            catch (DO.UnvalidIDException stationEx)
            {
                throw new FailedToGetException("ERROR", stationEx);
            }
            dalStation.CopyPropertiesTo(returningStation);
            returningStation.StationLocation = new();
            returningStation.StationLocation.Latitude = dalStation.Lattitude;
            returningStation.StationLocation.Longitude = dalStation.Longitude;
            List<DroneCharge> droneCharges = FindListOfDronesInStation(stationId);
            returningStation.DroneCharges = new();
            returningStation.DroneCharges = droneCharges;
            return returningStation;
        }

        /// <summary>
        /// creates list that contains all the charge slots of specific station.
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns>th ecreated station </returns>
        private List<DroneCharge> FindListOfDronesInStation(int stationId)
        {
            List<DroneCharge> droneChargesToReturn = new List<DroneCharge>();
            foreach (var (droneCharge, droneToAdd) in
            //goes through the drone charges
            from droneCharge in myDal.GetDroneChargeList()
            let droneToAdd = new DroneCharge()//if the drone charge is in the station
            where droneCharge.StationID == stationId
            select (droneCharge, droneToAdd))
            {
                //updates his fields and add to returning list
                droneToAdd.Id = droneCharge.DroneID;
                DroneForList drone = new();
                try
                {
                    drone = drones.First(item => item.Id == droneCharge.DroneID);
                }
                catch (InvalidOperationException)
                {
                    throw new InputDoesNotExist("the drone does not exist!!");
                }

                droneToAdd.Battery = drone.Battery;
                droneChargesToReturn.Add(droneToAdd);
            }

            return droneChargesToReturn;
        }
    }


}




