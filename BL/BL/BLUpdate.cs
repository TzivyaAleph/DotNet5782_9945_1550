﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    /// <summary>
    /// bl operation
    /// </summary>
    partial class BL
    {
        /// <summary>
        /// updates a drones name
        /// </summary>
        /// <param name="drone">the drone to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int droneId, string newModel)
        {
            lock (myDal)
            {

            }
            if (droneId < 1000 || droneId > 10000)//hello
            {
                throw new InvalidInputException($"id {droneId} is not valid !!");
            }
            DO.Drone droneTemp = new DO.Drone();
            try
            {
                droneTemp = myDal.CopyDroneArray().First(drone => drone.Id == droneId);//finds the drone to update in the dal drones list
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the drone does not exist !!");
            }
            droneTemp.Model = newModel;//updates the drone's name
            try
            {
                myDal.UpdateDrone(droneTemp);//updates the drone in idal drones list
            }
            catch (DO.ExistingObjectException custEx)
            {
                throw new FailedToUpdateException("ERROR", custEx);
            }
            int blDroneIndex = drones.FindIndex(blDrone => blDrone.Id == droneId);//finds the drone in the bl drones list
            drones[blDroneIndex].Model = newModel;//updates the drone in the bl drones list
        }

        /// <summary>
        /// updates a stations name and number of available charging slots
        /// </summary>
        /// <param name="stationToUpdate">station with the values to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(int stationId, string stationName, int numOfChargingSlots)
        {
            if (stationId < 1000 || stationId > 10000)
            {
                throw new InvalidInputException($"Station ID {stationId} is not valid\n");
            }
            if (String.IsNullOrEmpty(stationName))
            {
                throw new InvalidInputException($"model {stationName} is not valid !!");
            }
            DO.Station stationTemp = new DO.Station();
            try
            {
                stationTemp = myDal.CopyStationArray().First(station => station.Id == stationId);//finds the station to update in the dal station list
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the station does not exist !!");
            }
            if (!string.IsNullOrEmpty(stationName))//checkes if the function recieved a station name
                stationTemp.Name = stationName;//updates the stations name
            //updates the number of charging slots if the func recieved user's input
            if (numOfChargingSlots != 0)
            {
                List<DO.DroneCharge> droneCharges = new List<DO.DroneCharge>();//recieves the dal droneCharge list
                droneCharges = myDal.GetDroneChargeList().ToList();
                // counts the drones that are charging in the current station                                                                                              
                int countNumOfDronesInStation = 0;
                foreach (var dc in droneCharges)
                {
                    if (dc.StationID == stationId)
                        countNumOfDronesInStation++;
                }
                if ((numOfChargingSlots - countNumOfDronesInStation) < 0)
                    throw new InvalidInputException($"number of charging slots {numOfChargingSlots} is not valid\n");
                stationTemp.ChargeSlots = numOfChargingSlots - countNumOfDronesInStation;//updates the number of available charging slots in the current station
            }
            try
            {
                myDal.UpdateStation(stationTemp);//update station in dal stations list 
            }
            catch (DO.ExistingObjectException stEx)
            {
                throw new FailedToUpdateException("ERROR", stEx);
            }
        }

        /// <summary>
        /// updates a customer's id, phone number and name
        /// </summary>
        /// <param name="customer">customer to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(int customerId, string customerName, string customerPhone)
        {
            if (customerId < 100000000 || customerId > 1000000000)
            {
                throw new InvalidInputException($"Customer ID {customerId} is not valid\n");
            }
            if (String.IsNullOrEmpty(customerName))
            {
                throw new InvalidInputException($"model {customerName} is not valid !!");
            }
            if (customerPhone.Length != 11 && customerPhone.Length != 10)
            {
                throw new InvalidInputException("Invalid phone number!!\n");
            }
            DO.Customer customerTemp = new DO.Customer();
            try
            {
                customerTemp = myDal.CopyCustomerArray().First(customer => customer.Id == customerId);//finds the customer to update in the dal customers list
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the customer does not exist !!");
            }
            if (!(string.IsNullOrEmpty(customerName)))//checkes if the user put in a name to update
                customerTemp.Name = customerName;
            if (!(string.IsNullOrEmpty(customerPhone)))//checkes if the user put in a phone number to update
                customerTemp.PhoneNumber = customerPhone;
            try
            {
                myDal.UpdateCustomer(customerTemp);//update the customer in the dal customers list
            }
            catch (DO.ExistingObjectException cusEx)
            {
                throw new FailedToUpdateException("ERROR", cusEx);
            }
        }

        /// <summary>
        /// send drone to chargh slots by updating fields.
        /// </summary>
        /// <param name="d"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendDroneToChargeSlot(Drone d)
        {
            if (d.Id < 1000 || d.Id > 10000)
            {
                throw new InvalidInputException($"id {d.Id} is not valid !!");
            }
            DroneForList droneForList = new();
            try
            {
                droneForList = drones.First(item => item.Id == d.Id);
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the drone does not exist !!");
            }
            //only send to charge slots when drone available
            if (droneForList.DroneStatuses != DroneStatuses.Available)
            {
                throw new FailedToUpdateException($"Drone {droneForList.Id} is not available");
            }
            //finding all the available charging slots in station.
            List<DO.Station> stations = myDal.CopyStationArray(x => x.ChargeSlots > 0).ToList();
            DO.Station clossestStation = new DO.Station();
            //finds the clossest station to the current location of the drone
            clossestStation = myDal.GetClossestStation(droneForList.CurrentLocation.Latitude, droneForList.CurrentLocation.Longitude, stations);
            //finds the battery use for sending the drone to the clossest station
            double batteryUse = myDal.getDistanceFromLatLonInKm(clossestStation.Lattitude, clossestStation.Longitude, droneForList.CurrentLocation.Latitude, droneForList.CurrentLocation.Longitude) * myDal.GetElectricityUse()[0];
            //send the drone only if the drone has enough battery and the station has available charging slots.
            bool eoungh = enoughBatteryForCharging(droneForList);
            if (!eoungh || clossestStation.ChargeSlots == 0)
                throw new FailedToUpdateException($"There are no available charge slots in station {clossestStation.Id}");
          
            double[] electricity = myDal.GetElectricityUse();
            droneForList.CurrentLocation = new();
            if (eoungh==true)
                droneForList.Battery -= batteryUse;
            else
                droneForList.Battery = 0;
            droneForList.CurrentLocation.Latitude = clossestStation.Lattitude;
            droneForList.CurrentLocation.Longitude = clossestStation.Longitude;
            droneForList.DroneStatuses = DroneStatuses.Maintenance;
            DO.Drone dalDrone = new DO.Drone()
            {
                Id = droneForList.Id,
                Model = droneForList.Model,
                Weight = (DO.Weight)droneForList.Weight,
            };
            try
            {
                myDal.SendDroneToChargeSlot(dalDrone, clossestStation);
            }
            catch (DO.ExistingObjectException custEx)
            {
                throw new FailedToUpdateException("ERROR", custEx);
            }
        }

        /// <summary>
        /// checks if theres enough battery for charging
        /// </summary>
        /// <param name="droneForList"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool enoughBatteryForCharging(DroneForList droneForList)
        {
            //finding all the available charging slots in station.
            List<DO.Station> stations = myDal.CopyStationArray(x => x.ChargeSlots > 0).ToList();
            DO.Station clossestStation = new DO.Station();
            //finds the clossest station to the current location of the drone
            clossestStation = myDal.GetClossestStation(droneForList.CurrentLocation.Latitude, droneForList.CurrentLocation.Longitude, stations);
            //finds the battery use for sending the drone to the clossest station
            double batteryUse = myDal.getDistanceFromLatLonInKm(clossestStation.Lattitude, clossestStation.Longitude, droneForList.CurrentLocation.Latitude, droneForList.CurrentLocation.Longitude) * myDal.GetElectricityUse()[0];
            if (droneForList.Battery - batteryUse < 0)
                return false;
            return true;
        }

        /// <summary>
        /// recieves a drone ID and attributes a parcel to the drone
        /// </summary>
        /// <param name="droneId">id of the drone we are attributing to the parcel</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AttributingParcelToDrone(int droneId)
        {
            while (droneId < 1000 || droneId > 10000)
            {
                throw new InvalidInputException($"id {droneId} is not valid !!");
            }
            List<DO.Parcel> parcels = myDal.CopyParcelArray(par =>par.IsDeleted==false&& par.DroneID == 0&& par.Delivered==null).ToList();//gets the non attributed parcels list that aren't deleted and weren't delivered yet
            int index = drones.FindIndex(item => item.Id == droneId);//searches for the index of the drone in the drones list
            DroneForList droneToAttribute = new();
            droneToAttribute = drones[index];
            if (droneToAttribute.DroneStatuses != DroneStatuses.Available)//checkes if the drone is available
            {
                throw new InvalidInputException($"drone {droneId} is not available !!");
            }
            //parcels.Sort((p1, p2) => p1.Priority.CompareTo(p2.Priority));//sorts the non attributed parcels list by the priority 
            parcels.RemoveAll(p => (int)p.Weight > (int)droneToAttribute.Weight);
            var emergencyParcels = parcels.Where(p => p.Priority == DO.Priority.emergency).OrderBy(p => (int)p.Weight).ToList(); //sorts the emergency parcels by their weight
            var fastParcels = parcels.Where(p => p.Priority == DO.Priority.fast).OrderBy(p => (int)p.Weight).ToList(); //sorts the fast parcels by their weight
            var normalParcels = parcels.Where(p => p.Priority == DO.Priority.normal).OrderBy(p => (int)p.Weight).ToList(); //sorts the normal parcels by their weight
            parcels = emergencyParcels;
            parcels.AddRange(fastParcels);
            parcels.AddRange(normalParcels);
            if (parcels.Count == 0)
                throw new FailedToUpdateException("there is no parcel to attribute !!");
            //removes all the parcels who the drone doesnt have enough battery to attribute to them
            parcels.RemoveAll(p => enoughBattery(p, droneToAttribute));
            if (parcels.Count == 0)
                throw new FailedToUpdateException("there is no parcel to attribute !!");
            double minDistance = getDroneParcelDistance(droneToAttribute, parcels[0]);
            DO.Parcel minParcel = parcels[0];
            double distance;
            //finds the parcel thats clossest to the drone
            foreach (var p in parcels)
            {
                distance = getDroneParcelDistance(droneToAttribute, p);
                if (distance < minDistance)
                {
                    int minIndex = parcels.FindIndex(item => item.Id == minParcel.Id);
                    minParcel = parcels[minIndex];
                }
            }
            drones[index].DroneStatuses = DroneStatuses.Delivered;
            drones[index].ParcelId = minParcel.Id;
            DO.Parcel dalParcel = new();
            try
            {
                dalParcel = myDal.CopyParcelArray(par => par.DroneID == 0).First(par => par.Id == minParcel.Id);
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the parcel does not exist !!");
            }
            List<DO.Drone> dalDrones = myDal.CopyDroneArray().ToList();
            try
            {
                DO.Drone drone = dalDrones.FirstOrDefault(x => x.Id == droneId);
                myDal.AttributingParcelToDrone(dalParcel, drone);
            }
            catch (DO.ExistingObjectException parEx)
            {
                throw new FailedToUpdateException("ERROR", parEx);
            }
        }

        /// <summary>
        /// checks if theres parcels to attribute
        /// </summary>
        /// <param name="droneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool ThereAreParcelsToAttribute(int droneId)
        {
            int index = drones.FindIndex(item => item.Id == droneId);//searches for the index of the drone in the drones list
            DroneForList droneToAttribute = new();
            droneToAttribute = drones[index];
            List<DO.Parcel> parcels = myDal.CopyParcelArray(par => par.IsDeleted == false && par.DroneID == 0 && par.Delivered == null).ToList();//gets the non attributed parcels list that aren't deleted and weren't delivered yet
            //parcels.Sort((p1, p2) => p1.Priority.CompareTo(p2.Priority));//sorts the non attributed parcels list by the priority 
            parcels.RemoveAll(p => (int)p.Weight > (int)droneToAttribute.Weight);
            var emergencyParcels = parcels.Where(p => p.Priority == DO.Priority.emergency).OrderBy(p => (int)p.Weight).ToList(); //sorts the emergency parcels by their weight
            var fastParcels = parcels.Where(p => p.Priority == DO.Priority.fast).OrderBy(p => (int)p.Weight).ToList(); //sorts the fast parcels by their weight
            var normalParcels = parcels.Where(p => p.Priority == DO.Priority.normal).OrderBy(p => (int)p.Weight).ToList(); //sorts the normal parcels by their weight
            parcels = emergencyParcels;
            parcels.AddRange(fastParcels);
            parcels.AddRange(normalParcels);
            if (parcels.Count == 0)
                return false;
            //removes all the parcels who the drone doesnt have enough battery to attribute to them
            parcels.RemoveAll(p => enoughBattery(p, droneToAttribute));
            if (parcels.Count == 0)
                return false;
            return true;
        }

        /// <summary>
        /// checks if thre drone has enough battery  to attributted to specific parcel
        /// </summary>
        /// <param name="p"></param>
        /// <param name="droneToAttribute"></param>
        /// <returns></returns>
        private bool enoughBattery(DO.Parcel p, DroneForList droneToAttribute)
        {
            double minDistance = getDroneParcelDistance(droneToAttribute, p);
            DO.Parcel minParcel = p;
            double batteryUseForPickUp = minDistance * myDal.GetElectricityUse()[0];
            DO.Customer dalSender = new DO.Customer();
            try
            {
                dalSender = myDal.CopyCustomerArray().First(customer => customer.Id == minParcel.SenderID);//finds the parcel's sender
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("Can not attribute-the sender does not exist !!");
            }
            DO.Customer dalTarget = new DO.Customer();
            try
            {
                dalTarget = myDal.CopyCustomerArray().First(customer => customer.Id == minParcel.TargetID);//finds the target
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("Can not attribute-the reciepient does not exist !!");
            }
            double batteryUseForDelivery = myDal.getDistanceFromLatLonInKm(dalSender.Lattitude, dalSender.Longtitude, dalTarget.Lattitude, dalTarget.Longtitude) * electricityByWeight((Weight)minParcel.Weight);
            DO.Station clossestStation = new DO.Station();
            List<DO.Station> stations = myDal.CopyStationArray().ToList();
            clossestStation = myDal.GetClossestStation(dalTarget.Lattitude, dalTarget.Longtitude, stations);
            double batteryUseForCharging = myDal.getDistanceFromLatLonInKm(clossestStation.Lattitude, clossestStation.Longitude, dalTarget.Lattitude, dalTarget.Longtitude) * myDal.GetElectricityUse()[0];
            if ((droneToAttribute.Battery - (batteryUseForPickUp + batteryUseForDelivery + batteryUseForCharging)) < 0)
                return true;
            return false;
        }

        /// <summary>
        /// finds the drone thats attributed to a parcel and updates him and the parcel to be picked up
        /// </summary>
        /// <param name="droneId">id of the pickup drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void pickedUp(int droneId)
        {
            if (droneId < 1000 || droneId > 10000)
            {
                throw new InvalidInputException($"id {droneId} is not valid !!");
            }
            IEnumerable<DO.Parcel> parcels = myDal.CopyParcelArray();//gets the non attributed parcels list
            IEnumerable<DO.Drone> idalDrones = myDal.CopyDroneArray();//gets the drones list
            DO.Parcel parcelToPickUp = new DO.Parcel();
            try
            {
                parcelToPickUp = parcels.First(parcel => parcel.DroneID == droneId && parcel.PickedUp == null);//finds the parcel thats attributed to the drone
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the parcel does not exist !!");
            }
            DO.Drone idalPickUpDrone = new DO.Drone();
            try
            {
                idalPickUpDrone = idalDrones.First(drone => drone.Id == droneId);//finds the pick up drone in idal drones list
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the drone does not exist !!");
            }
            if (parcelToPickUp.PickedUp != null)//checkes if the parcel was already picked up
            {
                throw new InvalidInputException($"parcel {parcelToPickUp.Id} was already picked up !!");
            }
            DroneForList pickUpDrone = new();
            pickUpDrone = drones.Find(drone => drone.Id == droneId);//finds the drone thats picking up the parcel in the bl drones list
            IEnumerable<DO.Customer> customers = myDal.CopyCustomerArray();//gets the customers list
            double[] electricity = myDal.GetElectricityUse();
            DO.Customer parcelSender = new DO.Customer();
            try
            {
                parcelSender = customers.First(customer => customer.Id == parcelToPickUp.SenderID);//finds the parcels sender (for finding the parcels location)
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the sender does not exist !!");
            }
            double electricityForPickUp = electricity[0] * myDal.getDistanceFromLatLonInKm(pickUpDrone.CurrentLocation.Latitude, pickUpDrone.CurrentLocation.Longitude, parcelSender.Lattitude, parcelSender.Longtitude);//calculates the battery use from drones current location to the parcel 
            pickUpDrone.CurrentLocation = new();
            if ((pickUpDrone.Battery - electricityForPickUp) >= 0)
                pickUpDrone.Battery -= electricityForPickUp;//updates the drones battery 
            else
                pickUpDrone.Battery = 0;
            pickUpDrone.CurrentLocation.Latitude = parcelSender.Lattitude;//updates the drones location to where he picked up the parcel 
            pickUpDrone.CurrentLocation.Longitude = parcelSender.Longtitude;
            int droneBlIndex = drones.FindIndex(item => item.Id == droneId);//finds the index of the pickup drone in the bl drones list
            drones[droneBlIndex] = pickUpDrone;//puts the updated drone into the bl drones list
            try
            {
                myDal.PickedUp(parcelToPickUp, idalPickUpDrone);//updates the parcel in the idal parcels list
            }
            catch (DO.UnvalidIDException exc)
            {
                throw new FailedToUpdateException("ERROR", exc);
            }

        }

        /// <summary>
        /// updates a parcel to be delivered and the drone that delivered the parcel to be available 
        /// </summary>
        /// <param name="droneId">id of the drone who picked up the parcel</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Delivered(int droneId)
        {
            if (droneId < 1000 || droneId > 10000)
            {
                throw new InvalidInputException($"id {droneId} is not valid !!");
            }
            IEnumerable<DO.Parcel> parcels = myDal.CopyParcelArray();//gets the non attributed parcels list
            IEnumerable<DO.Drone> idalDrones = myDal.CopyDroneArray();//gets the drones list
            DO.Parcel parcelToDeliver = new DO.Parcel();
            try
            {
                parcelToDeliver = parcels.First(parcel => parcel.DroneID == droneId&&parcel.Delivered==null);//finds the parcel thats attributed to the drone
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the parcel does not exist !!");
            }
            DO.Drone idalDeliveryDrone = new DO.Drone();
            try
            {
                idalDeliveryDrone = idalDrones.First(drone => drone.Id == droneId);//finds the pick up drone in idal drones list
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the parcel does not exist !!");
            }
            DroneForList blDeliveryDrone = new();
            blDeliveryDrone = drones.Find(drone => drone.Id == droneId);//finds the drone thats picking up the parcel in the bl drones list
            if (parcelToDeliver.PickedUp == null)//checkes if the parcel wasnt picked up yet
            {
                throw new FailedToUpdateException($"parcel {parcelToDeliver.Id} wasn't picked up yet !!");
            }
            if (parcelToDeliver.Delivered != null)//checkes if the parcel has been delivered 
            {
                throw new FailedToUpdateException($"parcel {parcelToDeliver.Id} was already delivered !!");
            }
            double[] electricity = myDal.GetElectricityUse();
            IEnumerable<DO.Customer> customers = myDal.CopyCustomerArray();//gets the customers list
            DO.Customer parcelReciever = new DO.Customer();
            try
            {
                parcelReciever = customers.First(customer => customer.Id == parcelToDeliver.TargetID);//finds the parcels target customer (for finding the parcels location)
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the customer does not exist !!");
            }
            double electricityForDelivery = electricityByWeight((Weight)parcelToDeliver.Weight) * myDal.getDistanceFromLatLonInKm(blDeliveryDrone.CurrentLocation.Latitude, blDeliveryDrone.CurrentLocation.Longitude, parcelReciever.Lattitude, parcelReciever.Longtitude);
            blDeliveryDrone.CurrentLocation = new();
            if ((blDeliveryDrone.Battery - electricityForDelivery) >= 0)
                blDeliveryDrone.Battery = blDeliveryDrone.Battery - electricityForDelivery;//updates the drones battery 
            else
                blDeliveryDrone.Battery = 0;
            blDeliveryDrone.CurrentLocation.Latitude = parcelReciever.Lattitude;//updates the drones location to where he picked up the parcel 
            blDeliveryDrone.CurrentLocation.Longitude = parcelReciever.Longtitude;
            blDeliveryDrone.DroneStatuses = DroneStatuses.Available;//update the delivery drone status to available
            blDeliveryDrone.ParcelId = 0;
            int droneBlIndex = drones.FindIndex(item => item.Id == droneId);//finds the index of the delivery drone in the bl drones list
            drones[droneBlIndex] = blDeliveryDrone;//puts the updated drone into the bl drones list
            try
            {
                myDal.Delivered(parcelToDeliver);
            }
            catch (DO.UnvalidIDException exc)
            {
                throw new FailedToUpdateException("ERROR", exc);
            }
        }

        /// <summary>
        /// finds the distance between drone and parcel
        /// </summary>
        /// <param name="d"></param>
        /// <param name="p"></param>
        /// <returns>the distance</returns>
        private double getDroneParcelDistance(DroneForList d, DO.Parcel p)
        {
            IEnumerable<DO.Customer> customers = myDal.CopyCustomerArray();//gets the customers list
            DO.Customer parcelSender = new DO.Customer();
            try
            {
                parcelSender = customers.First(customer => customer.Id == p.SenderID);//finds the parcels sender
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the sender does not exist !!");
            }
            double distance = Math.Sqrt(Math.Pow(d.CurrentLocation.Latitude - parcelSender.Lattitude, 2) + Math.Pow(d.CurrentLocation.Longitude - parcelSender.Longtitude, 2));//finds the distance between the drone and the parcel
            return distance;
        }

        /// <summary>
        /// a method for finding the battery use by the parcel weight.
        /// </summary>
        /// <param name="maxWeight">weight of parcel</param>
        /// <returns></returns>
        private double electricityByWeight(Weight maxWeight)
        {
            if (maxWeight == Weight.Light)
                return myDal.GetElectricityUse()[1];
            if (maxWeight == Weight.Medium)
                return myDal.GetElectricityUse()[2];
            return myDal.GetElectricityUse()[3];
        }

        /// <summary>
        /// release drone from charge slot by update the fields
        /// </summary>
        /// <param name="d">the dron to release</param>
        /// <param name="timeInCharge">for the hour its been charging</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleasedroneFromeChargeSlot(Drone d)
        {
            if (d.Id < 1000 || d.Id > 10000)
            {
                throw new InvalidInputException($"id {d.Id} is not valid !!");
            }
            DroneForList droneForList = new();
            try
            {
                droneForList = drones.First(item => item.Id == d.Id);
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the drone does not exist !!");
            }
            if (d.DroneStatuses != DroneStatuses.Maintenance)
                throw new FailedToUpdateException($"cant realese drone from charge if its not charging");
            DO.Station dalStation = new DO.Station();
            try
            {
                dalStation = myDal.CopyStationArray().First(item => item.Lattitude == d.CurrentLocation.Latitude && item.Longitude == d.CurrentLocation.Longitude);
            }
            catch (InvalidOperationException)
            {
                throw new InputDoesNotExist("the station does not exist !!");
            }
            TimeSpan timeInCharging = (TimeSpan)(DateTime.Now - myDal.GetDroneCharge(dalStation.Id, d.Id).SentToCharge);
            double batteryCharge = timeInCharging.TotalHours * myDal.GetElectricityUse()[4];
            //cant charge more than 100
            if ((droneForList.Battery + batteryCharge) > 100)
                droneForList.Battery = 100;
            else
                droneForList.Battery += batteryCharge;
            droneForList.DroneStatuses = DroneStatuses.Available;
            int droneBlIndex = drones.FindIndex(item => item.Id == droneForList.Id);//finds the index of the charging drone in the bl drones list
            drones[droneBlIndex] = droneForList;//puts the updated drone into the bl drones list
            DO.Drone dalDrone = new DO.Drone()
            {
                Id = d.Id,
                Model = d.Model,
                Weight = (DO.Weight)d.Weight,
            };
            try
            {
                myDal.ReleaseDrone(dalDrone, dalStation);
            }
            catch (DO.UnvalidIDException exc)
            {
                throw new FailedToUpdateException("ERROR", exc);
            }
        }

        /// <summary>
        /// deletes parcel from list
        /// </summary>
        /// <param name="parcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(Parcel parcel)
        {
            DO.Parcel dalParcel = new DO.Parcel();
            object obj = dalParcel;
            parcel.CopyPropertiesTo(obj);
            dalParcel = (DO.Parcel)obj;
            parcel.CopyPropertiesTo(dalParcel);
            dalParcel.DroneID = parcel.DroneInParcel.Id;
            dalParcel.SenderID = parcel.Sender.Id;
            dalParcel.TargetID = parcel.Recipient.Id;
            try
            {
                myDal.UpdateParcel(dalParcel);
            }
            catch (DO.ExistingObjectException ex)
            {
                throw new FailedToUpdateException("ERROR", ex);
            }
        }

        /// <summary>
        /// deletes parcel from list
        /// </summary>
        /// <param name="parcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(Customer customer)
        {
            DO.Customer dalCustomer = new DO.Customer();
            object obj = dalCustomer;
            customer.CopyPropertiesTo(obj);
            dalCustomer = (DO.Customer)obj;
            customer.CopyPropertiesTo(dalCustomer);
            dalCustomer.Lattitude = customer.Location.Latitude;
            dalCustomer.Longtitude = customer.Location.Longitude;
            try
            {
                myDal.UpdateCustomer(dalCustomer);
            }
            catch (DO.ExistingObjectException ex)
            {
                throw new FailedToUpdateException("ERROR", ex);
            }
        }

        /// <summary>
        /// deletes drone from list
        /// </summary>
        /// <param name="drone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(Drone drone)
        {
            DO.Drone dalDrone = new DO.Drone();
            object obj = dalDrone;
            drone.CopyPropertiesTo(obj);
            dalDrone = (DO.Drone)obj;
            drone.CopyPropertiesTo(dalDrone);
            int index = drones.FindIndex(item => item.Id == drone.Id);
            drones[index].IsDeleted = true;
            try
            {
                myDal.UpdateDrone(dalDrone);
            }
            catch (DO.ExistingObjectException ex)
            {
                throw new FailedToUpdateException("ERROR", ex);
            }
        }

        /// <summary>
        /// deletes station from list
        /// </summary>
        /// <param name="station"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(Station station)
        {
            DO.Station dalStation = new DO.Station();
            object obj = dalStation;
            station.CopyPropertiesTo(obj);
            dalStation = (DO.Station)obj;
            station.CopyPropertiesTo(dalStation);
            dalStation.Lattitude = (long)station.StationLocation.Latitude;
            dalStation.Longitude = (long)station.StationLocation.Longitude;
            try
            {
                myDal.UpdateStation(dalStation);
            }
            catch (DO.ExistingObjectException ex)
            {
                throw new FailedToUpdateException("ERROR", ex);
            }
        }
    }


}
