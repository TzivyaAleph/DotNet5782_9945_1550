using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DalApi;
using System.Runtime.CompilerServices;


namespace BL
{
    partial class BL
    {
        /// <summary>
        /// createse list of stations and update each fields by dal data.
        /// </summary>
        /// <returns>the creates list</returns>
        public IEnumerable<StationForList> GetStationList()
        {
            List<StationForList> stationsToReturn = new List<StationForList>();
            List<DO.DroneCharge> droneCharges = new();
            droneCharges = myDal.GetDroneChargeList().ToList();
            foreach (var stat in myDal.CopyStationArray())
            {


                StationForList stationToAdd = new();
                stationToAdd.Id = stat.Id;
                stationToAdd.Name = stat.Name;
                int countNumOfDronesInStation = droneCharges.Count(item => item.StationID == stat.Id);
                Station station = new();
                stationToAdd.AvailableChargingSlots = stat.ChargeSlots - countNumOfDronesInStation;
                stationToAdd.UnAvailableChargingSlots = countNumOfDronesInStation;
                if (!stat.IsDeleted || countNumOfDronesInStation > 0)
                    stationsToReturn.Add(stationToAdd);

            }
            return stationsToReturn;
        }

        /// <summary>
        /// creates new list and copies all the fields from the drone list in bl
        /// </summary>
        /// <returns>the created list</returns>
        public IEnumerable<DroneForList> GetDroneList(Func<DroneForList, bool> predicate = null)
        {
            List<DroneForList> dronesForList = new List<DroneForList>(drones);
            if (predicate == null)
            {
                return dronesForList.Where(d => !d.IsDeleted || d.ParcelId != 0);
            }
            return dronesForList.Where(predicate);
        }

        /// <summary>
        /// creates new list with data frome the customer list and parcels from dal
        /// </summary>
        /// <returns>the created list</returns>
        public IEnumerable<CustomerForList> GetCustomerList()
        {
            List<CustomerForList> customersForList = new List<CustomerForList>();
            foreach (var cust in myDal.CopyCustomerArray())
            {
                if (cust.IsDeleted == false)
                {
                    CustomerForList customerToAdd = new();
                    customerToAdd.Id = cust.Id;
                    customerToAdd.Name = cust.Name;
                    customerToAdd.Phone = cust.PhoneNumber;
                    //counts all the parcel which he sends and been delievered.
                    int count = myDal.CopyParcelArray().Count(item => item.SenderID == cust.Id && item.Delivered != null);
                    customerToAdd.ParcelProvided = count;
                    //counts all the parcel which he sends and not been delievered.
                    count = myDal.CopyParcelArray().Count(item => item.SenderID == cust.Id && item.Delivered == null);
                    customerToAdd.ParcelNotProvided = count;
                    //counts all the parcel which he gets.
                    count = myDal.CopyParcelArray().Count(item => item.TargetID == cust.Id && item.Delivered != null);
                    customerToAdd.ParcelRecieved = count;
                    //counts all the parcel that on the way to him.
                    count = myDal.CopyParcelArray().Count(item => item.TargetID == cust.Id && item.Delivered == null && item.PickedUp != null);
                    customerToAdd.ParcelOnTheWay = count;
                    customersForList.Add(customerToAdd);
                }
            }
            return customersForList;
        }

        /// <summary>
        /// creates new list of parcels with data in fields.
        /// </summary>
        /// <returns>the created parcel</returns>
        public IEnumerable<ParcelForList> GetParcelList()
        {

            List<ParcelForList> parcelsForList = new List<ParcelForList>();
            foreach (var par in myDal.CopyParcelArray())
            {
                if (par.IsDeleted == false)
                {
                    ParcelForList parcelToAdd = new();
                    parcelToAdd.Id = par.Id;
                    DO.Customer dalCustomer = new DO.Customer();
                    //finds the sender in customer list for getting his name
                    try
                    {
                        dalCustomer = myDal.CopyCustomerArray().First(item => item.Id == par.SenderID);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new InputDoesNotExist("sender id is missing!!");
                    }
                    parcelToAdd.Sender = dalCustomer.Name;
                    //finds the reciepient in customer list for getting his name
                    try
                    {
                        dalCustomer = myDal.CopyCustomerArray().First(item => item.Id == par.TargetID);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new InputDoesNotExist("cant print the parcel-reciepient id is missing!!");
                    }
                    parcelToAdd.Reciever = dalCustomer.Name;
                    parcelToAdd.Weight = (Weight)par.Weight;
                    parcelToAdd.Priority = (Priority)par.Priority;
                    parcelToAdd.Status = getStatus(par);
                    parcelsForList.Add(parcelToAdd);
                }
            }
            return parcelsForList;
        }

        /// <summary>
        /// creates list of parcels who  does not attribute to drone
        /// </summary>
        /// <returns>the created list</returns>
        public IEnumerable<ParcelForList> GetUnAtributtedParcels()
        {
            List<ParcelForList> parcelsForList = new List<ParcelForList>();
            List<DO.Parcel> unAttributed = new List<DO.Parcel>();
            unAttributed = myDal.CopyParcelArray(par => par.DroneID == 0).ToList();
            foreach (var par in unAttributed)
            {
                ParcelForList parcelToAdd = new();
                parcelToAdd.Id = par.Id;
                DO.Customer dalCustomer = new DO.Customer();
                //finds the sender in customer list for getting his name
                dalCustomer = myDal.CopyCustomerArray().First(item => item.Id == par.SenderID);
                parcelToAdd.Sender = dalCustomer.Name;
                //finds the reciepient in customer list for getting his name
                dalCustomer = myDal.CopyCustomerArray().First(item => item.Id == par.TargetID);
                parcelToAdd.Reciever = dalCustomer.Name;
                parcelToAdd.Weight = (Weight)par.Weight;
                parcelToAdd.Priority = (Priority)par.Priority;
                parcelToAdd.Status = getStatus(par);
                parcelsForList.Add(parcelToAdd);
            }
            return parcelsForList;
        }

        /// <summary>
        /// creates list with all the station that has available charging slots.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StationForList> GetAvailableChargingSlotsStations()
        {
            List<StationForList> stationsToReturn = new List<StationForList>();
            List<DO.Station> dalStations = new List<DO.Station>();
            dalStations = myDal.CopyStationArray(x => x.ChargeSlots > 0).ToList();
            foreach (var stat in dalStations)
            {
                StationForList stationToAdd = new();
                stationToAdd.Id = stat.Id;
                stationToAdd.Name = stat.Name;
                // counts the drones that are charging in the current station
                int countNumOfDronesInStation = 0;
                foreach (var dc in myDal.GetDroneChargeList())
                {
                    if (dc.StationID == stat.Id)
                        countNumOfDronesInStation++;
                }
                stationToAdd.AvailableChargingSlots = stat.ChargeSlots - countNumOfDronesInStation;
                stationToAdd.UnAvailableChargingSlots = countNumOfDronesInStation;
                stationsToReturn.Add(stationToAdd);

            }
            return stationsToReturn;
        }

        /// <summary>
        /// checks the parcel status and updates the field.
        /// </summary>
        /// <param name="par"></param>
        /// <returns>the status of the parcel</returns>
        private Status getStatus(DO.Parcel par)
        {
            if (par.Delivered != null)
                return Status.Delivered;
            else if (par.PickedUp != null)
                return Status.Picked;
            else if (par.Scheduled != null)
                return Status.Assigned;
            else
                return Status.Created;
        }
    }
}
