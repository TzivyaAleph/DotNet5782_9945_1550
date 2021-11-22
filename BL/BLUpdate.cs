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
        /// updates a drones name
        /// </summary>
        /// <param name="drone">the drone to update</param>
        public void UpdateDrone(int droneId, string newModel)
        {
            if (droneId < 1000 || droneId > 10000)
                throw new InvalidInputException($"Drone ID {droneId} is not valid\n");
            if (String.IsNullOrEmpty(newModel))
                throw new InvalidInputException($"Drone model {newModel} is not valid\n");
            IDAL.DO.Drone droneTemp = myDal.CopyDroneArray().First(drone => drone.ID == droneId);//finds the drone to update in the dal drones list
            droneTemp.Model = newModel;//updates the drone's name
            try
            {
              myDal.UpdateDrone(droneTemp);//updates the drone in idal drones list
            }
            catch (IDAL.DO.ExistingObjectException custEx)
            {
                throw new FailedToUpdateException(custEx.ToString(), custEx);
            }
            int blDroneIndex= drones.FindIndex(blDrone => blDrone.ID == droneId);//finds the drone in the bl drones list
            drones[blDroneIndex].Model= newModel;//updates the drone in the bl drones list
        }

        /// <summary>
        /// updates a stations name and number of available charging slots
        /// </summary>
        /// <param name="stationToUpdate">station with the values to update</param>
        public void UpdateStation(Station stationToUpdate, int numOfChargingSlots)//לשאול את יהודה איזה תחנה צריך לעדכן
        {
            IDAL.DO.Station stationTemp = myDal.CopyStationArray().First(station => station.ID == stationToUpdate.Id);//finds the station to update in the dal station list
            if (stationToUpdate .Id< 1000|| stationToUpdate.Id> 10000)
                throw new InvalidInputException($"Station ID {stationToUpdate.Id} is not valid\n");
            if (!(string.IsNullOrEmpty(stationToUpdate.Name)))//checkes if the function recieved a station name
                stationTemp.StationName = stationToUpdate.Name;
            if(numOfChargingSlots<0||numOfChargingSlots>50)
                throw new InvalidInputException($"number of charging slots {numOfChargingSlots} is not valid\n");
            List<IDAL.DO.DroneCharge> droneCharges = (List<IDAL.DO.DroneCharge>)myDal.GetDroneChargeList();//recieves the dal droneCharge list
            // counts the drones that are charging in the current station
            int countNumOfDronesInStation = 0;
            foreach(var dc in droneCharges)
            {
                if (dc.StationID == stationToUpdate.Id)
                    countNumOfDronesInStation++;
            }
            stationTemp.ChargeSlots = numOfChargingSlots - countNumOfDronesInStation;//updates the number of available charging slots in the current station
            try
            {
                myDal.UpdateStation(stationTemp);//update station in dal stations list 
            }
            catch (IDAL.DO.ExistingObjectException stEx)
            {
                throw new FailedToUpdateException(stEx.ToString(), stEx);
            }
        }

        /// <summary>
        /// updates a customer's id, phone number and name
        /// </summary>
        /// <param name="customer">customer to update</param>
        public void UpdateCustomer(Customer customer)
        {
            IDAL.DO.Customer customerTemp = myDal.CopyCustomerArray().First(customer => customer.ID == customer.ID);//finds the customer to update in the dal customers list
            if (customer.Id<100000000||customer.Id>1000000000)
                throw new InvalidInputException($"customer ID {customer.Id} is not valid\n");
            if (!(string.IsNullOrEmpty(customer.Name)))//checkes if the user put in a name to update
                customerTemp.Name = customer.Name;
            if (!(string.IsNullOrEmpty(customer.Phone)))//checkes if the user put in a phone number to update
                customerTemp.PhoneNumber = customer.Phone;
            try
            {
                myDal.UpdateCustomer(customerTemp);//update the customer in the dal customers list
            }
            catch (IDAL.DO.ExistingObjectException cusEx)
            {
                throw new FailedToUpdateException(cusEx.ToString(), cusEx);
            }
        }

        /// <summary>
        /// send drone to chargh slots by updating fields.
        /// </summary>
        /// <param name="d"></param>
        public void SendDroneToChargeSlot(Drone d)
        {
            //only send to charge slots when drone available
            if (d.DroneStatuses != DroneStatuses.Available)
                throw new FailedToUpdateException($"Drone {d.ID} is not available");
            //finding all the available charging slots in station.
            List<IDAL.DO.Station> stations =(List<IDAL.DO.Station>) myDal.FindAvailableStations();
            IDAL.DO.Station clossestStation = new IDAL.DO.Station();
            //finds the clossest station to the current location of the drone
            clossestStation = myDal.GetClossestStation(d.CurrentLocation.Latitude, d.CurrentLocation.Longitude, stations);
            //finds the battery use for sending the drone to the clossest station
            double batteryUse = myDal.getDistanceFromLatLonInKm(clossestStation.Lattitude, clossestStation.Longitude, d.CurrentLocation.Latitude, d.CurrentLocation.Longitude) * myDal.GetElectricityUse()[0];
            //send the drone only if the drone has enough battery and the station has available charging slots.
            if ((d.Battery- batteryUse)<0|| clossestStation.ChargeSlots == 0)
                throw new FailedToUpdateException($"There are no available charge slots in station {clossestStation.ID}");
            double[] electricity = myDal.GetElectricityUse();
            double distance= Math.Sqrt((Math.Pow(d.CurrentLocation.Latitude - clossestStation.Lattitude, 2) + Math.Pow(d.CurrentLocation.Longitude - clossestStation.Longitude, 2)));
            d.Battery -= batteryUse;
            d.CurrentLocation.Latitude = clossestStation.Lattitude;
            d.CurrentLocation.Latitude = clossestStation.Longitude;
            d.DroneStatuses = DroneStatuses.Maintenance;
            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone()
            {
                ID = d.ID,
                Model = d.Model,
                MaxWeight=(IDAL.DO.WeightCategories)d.MaxWeight,
            };
            try
            {
                myDal.SendDroneToChargeSlot(dalDrone, clossestStation);
            }
            catch (IDAL.DO.ExistingObjectException custEx)
            {
                throw new FailedToAddException(custEx.ToString(), custEx);
            }
        }

        /// <summary>
        /// recieves a drone ID and attributes a parcel to the drone
        /// </summary>
        /// <param name="droneId">id of the drone we are attributing to the parcel</param>
        public void AttributingParcelToDrone(int droneId)
        {
            List<IDAL.DO.Drone> dalDrones = (List<IDAL.DO.Drone>)myDal.CopyDroneArray();
            List<IDAL.DO.Parcel> parcels = (List<IDAL.DO.Parcel>)myDal.FindNotAttributedParcels();//gets the non attributed parcels list
            if (droneId < 1000 || droneId > 10000)//checks if id is valid
            {
                throw new InvalidInputException($"id {droneId} is not valid !!");
            }
            int index = drones.FindIndex(item => item.ID == droneId);//searches for the index of the drone in the drones list
            DroneForList droneToAttribute = drones[index];
            if(droneToAttribute.DroneStatuses!= DroneStatuses.Available)//checkes if the drone is available
            {
                throw new InvalidInputException($"drone {droneId} is not available !!");
            }
            IDAL.DO.WeightCategories droneWeight = (IDAL.DO.WeightCategories)droneToAttribute.MaxWeight;//
            double dronesDistance;
            parcels.Sort((p1, p2) => p1.Priority.CompareTo(p2.Priority));//sorts the non attributed parcels list by the priority 
            parcels.Where(p => p.Priority == IDAL.DO.Priorities.emergency).OrderBy(p => (int)p.Weight); //sorts the emergency parcels by their weight
            parcels.Where(p => p.Priority == IDAL.DO.Priorities.fast).OrderBy(p => (int)p.Weight); //sorts the fast parcels by their weight
            parcels.Where(p => p.Priority == IDAL.DO.Priorities.normal).OrderBy(p => (int)p.Weight); //sorts the normal parcels by their weight
            parcels.Reverse();
            //finds the parcel thats clossest to the drone
            foreach (var p in parcels)
            {

            }
            //searches for the parcel to attribute
            List<IDAL.DO.Parcel> tmp = new List<IDAL.DO.Parcel>();
            
        }

        /// <summary>
        /// finds the drone thats attributed to a parcel and updates him and the parcel to be picked up
        /// </summary>
        /// <param name="droneId">id of the pickup drone</param>
        public void pickedUp(int droneId)
        {
            if (droneId < 1000 || droneId > 10000)//checks if id is valid
            {
                throw new InvalidInputException($"id {droneId} is not valid !!");
            }
            IEnumerable<IDAL.DO.Parcel> parcels = myDal.CopyParcelArray();//gets the non attributed parcels list
            IEnumerable<IDAL.DO.Drone> idalDrones = myDal.CopyDroneArray();//gets the drones list
            IDAL.DO.Parcel parcelToPickUp = parcels.First(parcel => parcel.DroneID == droneId);//finds the parcel thats attributed to the drone
            IDAL.DO.Drone idalPickUpDrone = idalDrones.First(drone => drone.ID == droneId);//finds the pick up drone in idal drones list
            if (parcelToPickUp.PickedUp!=DateTime.MinValue)//checkes if the parcel was already picked up
            {
                throw new InvalidInputException($"parcel {parcelToPickUp.ID} was already picked up !!");
            }
            DroneForList pickUpDrone = drones.Find(drone => drone.ID == droneId);//finds the drone thats picking up the parcel in the bl drones list
            IEnumerable<IDAL.DO.Customer> customers = myDal.CopyCustomerArray();//gets the customers list
            double[] electricity = myDal.GetElectricityUse();
            IDAL.DO.Customer parcelSender = customers.First(customer => customer.ID == parcelToPickUp.SenderID);//finds the parcels sender (for finding the parcels location)
            double electricityForPickUp = electricity[0] * myDal.getDistanceFromLatLonInKm(pickUpDrone.CurrentLocation.Latitude, pickUpDrone.CurrentLocation.Longitude, parcelSender.Lattitude, parcelSender.Longtitude);//calculates the battery use from drones current location to the parcel 
            pickUpDrone.Battery = pickUpDrone.Battery - electricityForPickUp;//updates the drones battery 
            pickUpDrone.CurrentLocation.Latitude = parcelSender.Lattitude;//updates the drones location to where he picked up the parcel 
            pickUpDrone.CurrentLocation.Longitude = parcelSender.Longtitude;
            int droneBlIndex= drones.FindIndex(item => item.ID == droneId);//finds the index of the pickup drone in the bl drones list
            drones[droneBlIndex] = pickUpDrone;//puts the updated drone into the bl drones list
            try
            {
                myDal.PickedUp(parcelToPickUp, idalPickUpDrone);//updates the parcel in the idal parcels list
            }
            catch(IDAL.DO.UnvalidIDException exc)
            {
                throw new FailedToUpdateException(exc.ToString(), exc);
            }
        }

        /// <summary>
        /// updates a parcel to be delivered and the drone that delivered the parcel to be available 
        /// </summary>
        /// <param name="droneId">id of the drone who picked up the parcel</param>
        public void Delivered(int droneId)
        {
            if (droneId < 1000 || droneId > 10000)//checks if id is valid
            {
                throw new InvalidInputException($"id {droneId} is not valid !!");
            }
            IEnumerable<IDAL.DO.Parcel> parcels = myDal.CopyParcelArray();//gets the non attributed parcels list
            IEnumerable<IDAL.DO.Drone> idalDrones = myDal.CopyDroneArray();//gets the drones list
            IDAL.DO.Parcel parcelToDeliver = parcels.First(parcel => parcel.DroneID == droneId);//finds the parcel thats attributed to the drone
            IDAL.DO.Drone idalDeliveryDrone = idalDrones.First(drone => drone.ID == droneId);//finds the pick up drone in idal drones list
            DroneForList blDeliveryDrone = drones.Find(drone => drone.ID == droneId);//finds the drone thats picking up the parcel in the bl drones list
            if (parcelToDeliver.PickedUp == DateTime.MinValue)//checkes if the parcel wasnt picked up yet
            {
                throw new FailedToUpdateException($"parcel {parcelToDeliver.ID} wasn't picked up yet !!");
            }
            if (parcelToDeliver.Delivered != DateTime.MinValue)//checkes if the parcel has been delivered 
            {
                throw new FailedToUpdateException($"parcel {parcelToDeliver.ID} was already delivered !!");
            }
            double[] electricity = myDal.GetElectricityUse();
            IEnumerable<IDAL.DO.Customer> customers = myDal.CopyCustomerArray();//gets the customers list
            IDAL.DO.Customer parcelReciever = customers.First(customer => customer.ID == parcelToDeliver.TargetID);//finds the parcels target customer (for finding the parcels location)
            double electricityForDelivery = electricityByWeight((Weight)parcelToDeliver.Weight)* myDal.getDistanceFromLatLonInKm(blDeliveryDrone.CurrentLocation.Latitude, blDeliveryDrone.CurrentLocation.Longitude, parcelReciever.Lattitude, parcelReciever.Longtitude);
            blDeliveryDrone.Battery = blDeliveryDrone.Battery - electricityForDelivery;//updates the drones battery 
            blDeliveryDrone.CurrentLocation.Latitude = parcelReciever.Lattitude;//updates the drones location to where he picked up the parcel 
            blDeliveryDrone.CurrentLocation.Longitude = parcelReciever.Longtitude;
            blDeliveryDrone.DroneStatuses = DroneStatuses.Available;//update the delivery drone status to available
            int droneBlIndex = drones.FindIndex(item => item.ID == droneId);//finds the index of the delivery drone in the bl drones list
            drones[droneBlIndex] = blDeliveryDrone;//puts the updated drone into the bl drones list
            try
            {
                myDal.Delivered(parcelToDeliver);
            }
            catch (IDAL.DO.UnvalidIDException exc)
            {
                throw new FailedToUpdateException(exc.ToString(), exc);
            }
        }

        /// <summary>
        /// finds the distance between drone and parcel
        /// </summary>
        /// <param name="d"></param>
        /// <param name="p"></param>
        /// <returns>the distance</returns>
        private double getDroneParcelDistance(DroneForList d, Parcel p)
        {
            IEnumerable<IDAL.DO.Customer> customers = myDal.CopyCustomerArray();//gets the customers list
            IDAL.DO.Customer parcelSender = customers.First(customer => customer.ID == p.Sender.Id);//finds the parcels sender
            double distance = Math.Sqrt((Math.Pow(d.CurrentLocation.Latitude - parcelSender.Lattitude, 2) + Math.Pow(d.CurrentLocation.Longitude - parcelSender.Longtitude, 2)));//finds the distance between the drone and the parcel
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
        public void ReleasedroneFromeChargeSlot(Drone d,int timeInCharge)
        {
            if (d.DroneStatuses != DroneStatuses.Maintenance)
                throw new FailedToUpdateException($"Cant realese drone frome charge if its not charging");
            double batteryCharge = timeInCharge * myDal.GetElectricityUse()[4];
            //cant charge more than 100
            if ((d.Battery + batteryCharge) > 100)
                d.Battery = 100;
            else
                d.Battery += batteryCharge;
            d.DroneStatuses = DroneStatuses.Available;
            IDAL.DO.Station dalStation = new IDAL.DO.Station();
            dalStation = myDal.CopyStationArray().First(item => item.Lattitude == d.CurrentLocation.Latitude && item.Longitude == d.CurrentLocation.Longitude);
            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone();
            d.CopyPropertiesTo(dalDrone);
            try
            {
                myDal.ReleaseDrone(dalDrone, dalStation);
            }
            catch (IDAL.DO.UnvalidIDException exc)
            {
                throw new FailedToUpdateException(exc.ToString(), exc);
            }
        }
    }

    
}
