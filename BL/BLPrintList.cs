using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL
    {
        /// <summary>
        /// createse list of stations and update each fields by dal data.
        /// </summary>
        /// <returns>the creates list</returns>
        public IEnumerable<StationForList> GetStationList()
        {
            List<StationForList> stationsToReturn = new List<StationForList>();
            List<IDAL.DO.DroneCharge> droneCharges = new();
            droneCharges = myDal.GetDroneChargeList().ToList();
            foreach (var stat in myDal.CopyStationArray())
            {
                StationForList stationToAdd = new();
                stationToAdd.Id = stat.Id;
                stationToAdd.Name = stat.Name;
                stationToAdd.AvailableChargingSlots = stat.ChargeSlots;
                int countNumOfDronesInStation= droneCharges.Count(item => item.StationID == stat.Id);
                Station station = new();
                stationToAdd.UnAvailableChargingSlots = countNumOfDronesInStation - stat.ChargeSlots;
                stationsToReturn.Add(stationToAdd);
            }
            return stationsToReturn;
        }

        /// <summary>
        /// creates new list and copies all the fields from the drone list in bl
        /// </summary>
        /// <returns>the created list</returns>
        public IEnumerable<DroneForList> GetDroneList()
        {
            List<DroneForList> dronesForList = new List<DroneForList>(drones);
            return dronesForList;
        }

        /// <summary>
        /// creates new list with data frome the customer list and parcels from dal
        /// </summary>
        /// <returns>the created list</returns>
        public IEnumerable<CustomerForList> GetCustomerList()
        {
            List<CustomerForList> customersForList = new List<CustomerForList>();
            foreach(var cust in myDal.CopyCustomerArray())
            {
                CustomerForList customerToAdd = new();
                customerToAdd.Id = cust.Id;
                customerToAdd.Name = cust.Name;
                customerToAdd.Phone = cust.PhoneNumber;
                //counts all the parcel which he sends and been delievered.
                int count = myDal.CopyParcelArray().Count(item => item.SenderID==cust.Id && item.Delivered!=DateTime.MinValue);
                customerToAdd.ParcelProvided = count;
                //counts all the parcel which he sends and not been delievered.
                count = myDal.CopyParcelArray().Count(item => item.SenderID == cust.Id && item.Delivered == DateTime.MinValue);
                customerToAdd.ParcelNotProvided = count;
                //counts all the parcel which he gets.
                count = myDal.CopyParcelArray().Count(item => item.TargetID == cust.Id && item.Delivered!=DateTime.MinValue);
                customerToAdd.ParcelRecieved = count;
                //counts all the parcel that on the way to him.
                count = myDal.CopyParcelArray().Count(item => item.TargetID == cust.Id && item.Delivered == DateTime.MinValue&&item.PickedUp!=DateTime.MinValue);
                customerToAdd.ParcelOnTheWay = count;
                customersForList.Add(customerToAdd);
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
                ParcelForList parcelToAdd = new();
                parcelToAdd.Id = par.Id;
                IDAL.DO.Customer dalCustomer = new IDAL.DO.Customer();
                //finds the sender in customer list for getting his name
                try
                {
                    dalCustomer = myDal.CopyCustomerArray().First(item => item.Id == par.SenderID);
                }
                catch (InvalidOperationException)
                {
                    throw new InputDoesNotExist("cant print the parcel-sender id is missing!!");
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
                parcelToAdd.Status = (Status)getStatus(par);
                parcelsForList.Add(parcelToAdd);
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
            foreach(var par in myDal.CopyParcelArray())
            {
                ParcelForList parcelToAdd = new();
                if (par.DroneID==0)
                {
                    parcelToAdd.Id = par.Id;
                    IDAL.DO.Customer dalCustomer = new IDAL.DO.Customer();
                    //finds the sender in customer list for getting his name
                    dalCustomer = myDal.CopyCustomerArray().First(item => item.Id == par.SenderID);
                    parcelToAdd.Sender = dalCustomer.Name;
                    //finds the reciepient in customer list for getting his name
                    dalCustomer = myDal.CopyCustomerArray().First(item => item.Id == par.TargetID);
                    parcelToAdd.Reciever = dalCustomer.Name;
                    parcelToAdd.Weight = (Weight)par.Weight;
                    parcelToAdd.Priority = (Priority)par.Priority;
                    parcelToAdd.Status = (Status)getStatus(par);
                    parcelsForList.Add(parcelToAdd);
                }
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
            foreach (var stat in myDal.CopyStationArray())
            {
                StationForList stationToAdd = new();
                if (stat.ChargeSlots!=0)
                {
                    stationToAdd.Id = stat.Id;
                    stationToAdd.Name = stat.Name;
                    stationToAdd.AvailableChargingSlots = stat.ChargeSlots;
                    // counts the drones that are charging in the current station
                    int countNumOfDronesInStation = 0;
                    foreach (var dc in myDal.GetDroneChargeList())
                    {
                        if (dc.StationID == stat.Id)
                            countNumOfDronesInStation++;
                    }
                    Station station = new();
                    stationToAdd.UnAvailableChargingSlots = countNumOfDronesInStation - stat.ChargeSlots;
                    stationsToReturn.Add(stationToAdd);
                }
            }
            return stationsToReturn;
        }

        /// <summary>
        /// checks the parcel status and updates the field.
        /// </summary>
        /// <param name="par"></param>
        /// <returns>the status of the parcel</returns>
        private Status getStatus(IDAL.DO.Parcel par)
        {
            if (par.Delivered != DateTime.MinValue)
                return Status.Delivered;
            else if (par.PickedUp != DateTime.MinValue)
                return Status.Picked;
            else if (par.Scheduled != DateTime.MinValue)
                return Status.Assigned;
            else
                return Status.Created;
        }
    }
}
