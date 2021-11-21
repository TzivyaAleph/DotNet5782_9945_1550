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

        public void UpdateDrone(Drone drone)
        {
            IDAL.DO.Drone droneTemp = new IDAL.DO.Drone();
            if (drone.ID < 1000 || drone.ID > 10000)
                throw new InvalidInputException($"Drone ID {drone.ID} is not valid\n");
            droneTemp.ID = drone.ID;
            if (String.IsNullOrEmpty(drone.Model))
                throw new InvalidInputException($"Drone model {drone.Model} is not valid\n");
            droneTemp.Model = drone.Model;
            try
            {
              myDal.UpdateDrone(droneTemp);
            }
            catch (IDAL.DO.ExistingObjectException custEx)
            {
                //throw new Exception($"Customer id {id} was not found", custEx);
            }
        }

        public void SendDroneToChargeSlot(Drone d)
        {
            if (d.DroneStatuses != DroneStatuses.Available)//
                throw new FailedToUpdateException($"Drone {d.ID} is not available");
            //if(d.Battery)
            List<IDAL.DO.Station> stations =(List<IDAL.DO.Station>) myDal.FindAvailableStations();
            IDAL.DO.Station clossestStation = new IDAL.DO.Station();
            clossestStation = myDal.GetClossestStation(d.CurrentLocation.Latitude, d.CurrentLocation.Longitude, stations);
            if (clossestStation.ChargeSlots == 0)
                throw new FailedToUpdateException($"There are no available charge slots in station {clossestStation.ID}");
            double[] electricity = myDal.GetElectricityUse();
            double distance= Math.Sqrt((Math.Pow(d.CurrentLocation.Latitude - clossestStation.Lattitude, 2) + Math.Pow(d.CurrentLocation.Longitude - clossestStation.Longitude, 2)));
            if (d.MaxWeight==Weight.Light)
            {
            }
            d.CurrentLocation.Latitude = clossestStation.Lattitude;
            d.CurrentLocation.Latitude = clossestStation.Longitude;
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
            IDAL.DO.WeightCategories droneWeight = (IDAL.DO.WeightCategories)droneToAttribute.MaxWeight;
            double dronesDistance;
            //searches for the parcel to attribute
            List<IDAL.DO.Parcel> tmp = new List<IDAL.DO.Parcel>();
            //creates a new parcels list sorted by their priority
            foreach (var p in parcels)
            {

            }




                foreach (var p in parcels)
            {
                if (p.Priority == IDAL.DO.Priorities.emergency)//if p has highest priority
                {
                    if(p.Weight== IDAL.DO.WeightCategories.heavy)//if p has highest weight to carry
                    {

                    }
                    tmp.Insert(0, p); //add parcel to beggining of list
                }
                else
                {
                    if (p.Priority == IDAL.DO.Priorities.fast)
                    {
                        if (tmp.FindLastIndex(parcel => parcel.Priority == IDAL.DO.Priorities.emergency) != -1)//find the index of the last parcel with emergancy priority 
                        {
                            tmp[tmp.FindLastIndex(parcel => parcel.Priority == IDAL.DO.Priorities.emergency) + 1] = p;//add fast priority parcels after parcels with emergancy priority
                        }
                        else
                            tmp.Insert(0, p);//if there are no emergancy priority parcels, add the fast priority parcel to beggining of list
                    }
                    else
                        tmp.Add(p);
                }
            }
            
        }

        /// <summary>
        /// finds the drone thats attributed to a parcel and updates him and the parcel to be picked up
        /// </summary>
        /// <param name="droneId">id of the pickup drone</param>
        public void pickedUp(int droneId)
        {
            if (droneId < 1000 || droneId > 10000)//checks if id is valid
            {
                throw new BLInvalidInputException($"id {droneId} is not valid !!");
            }
            IEnumerable<IDAL.DO.Parcel> parcels = myDal.CopyParcelArray();//gets the non attributed parcels list
            IEnumerable<IDAL.DO.Drone> idalDrones = myDal.CopyDroneArray();//gets the drones list
            IDAL.DO.Parcel parcelToPickUp = parcels.First(parcel => parcel.DroneID == droneId);//finds the parcel thats attributed to the drone
            IDAL.DO.Drone idalPickUpDrone = idalDrones.First(drone => drone.ID == droneId);//finds the pick up drone in idal drones list
            if (parcelToPickUp.PickedUp!=DateTime.MinValue)//checkes if the parcel was already picked up
            {
                throw new BLParcelException($"parcel {parcelToPickUp.ID} was already picked up !!");
            }
            DroneForList pickUpDrone = drones.Find(drone => drone.ID == droneId);//finds the drone thats picking up the parcel in the bl drones list
            double [] electricity= myDal.GetElectricityUse();
            double electricityForPickUp = 0;//לתקן!!!
            pickUpDrone.Battery = (int)(pickUpDrone.Battery - electricityForPickUp);//updates the drones battery 
            IEnumerable<IDAL.DO.Customer> customers = myDal.CopyCustomerArray();//gets the customers list
            IDAL.DO.Customer parcelSender = customers.First(customer => customer.ID == parcelToPickUp.SenderID);//finds the parcels sender (for finding the parcels location)
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
                throw new BLInvalidInputException($"id {droneId} is not valid !!");
            }
            IEnumerable<IDAL.DO.Parcel> parcels = myDal.CopyParcelArray();//gets the non attributed parcels list
            IEnumerable<IDAL.DO.Drone> idalDrones = myDal.CopyDroneArray();//gets the drones list
            IDAL.DO.Parcel parcelToDeliver = parcels.First(parcel => parcel.DroneID == droneId);//finds the parcel thats attributed to the drone
            IDAL.DO.Drone idalDeliveryDrone = idalDrones.First(drone => drone.ID == droneId);//finds the pick up drone in idal drones list
            DroneForList blDeliveryDrone = drones.Find(drone => drone.ID == droneId);//finds the drone thats picking up the parcel in the bl drones list
            if (parcelToDeliver.PickedUp == DateTime.MinValue)//checkes if the parcel wasnt picked up yet
            {
                throw new BLParcelException($"parcel {parcelToDeliver.ID} wasn't picked up yet !!");
            }
            if (parcelToDeliver.Delivered != DateTime.MinValue)//checkes if the parcel has been delivered 
            {
                throw new BLParcelException($"parcel {parcelToDeliver.ID} was already delivered !!");
            }
            double[] electricity = myDal.GetElectricityUse();
            double electricityForDelivery = 0;//לתקן!!!
            blDeliveryDrone.Battery = (int)(blDeliveryDrone.Battery - electricityForDelivery);//updates the drones battery 
            IEnumerable<IDAL.DO.Customer> customers = myDal.CopyCustomerArray();//gets the customers list
            IDAL.DO.Customer parcelReciever = customers.First(customer => customer.ID == parcelToDeliver.TargetID);//finds the parcels target customer (for finding the parcels location)
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
        public double GetDroneParcelDistance(DroneForList d, Parcel p)
        {
            IEnumerable<IDAL.DO.Customer> customers = myDal.CopyCustomerArray();//gets the customers list
            IDAL.DO.Customer parcelSender = customers.First(customer => customer.ID == p.Sender.Id);//finds the parcels sender
            double distance = Math.Sqrt((Math.Pow(d.CurrentLocation.Latitude - parcelSender.Lattitude, 2) + Math.Pow(d.CurrentLocation.Longitude - parcelSender.Longtitude, 2)));//finds the distance between the drone and the parcel
            return distance;
        }

        public List<IDAL.DO.Parcel> sortParcels()
        {
            IEnumerable<IDAL.DO.Parcel> parcels = myDal.FindNotAttributedParcels();//gets the non attributed parcels list
            List<IDAL.DO.Parcel> tmp = new List<IDAL.DO.Parcel>();
            foreach (var p in parcels)
            {
                if (p.Priority == IDAL.DO.Priorities.emergency)//if p has highest priority
                {
                    tmp[0] = p;//put highst priority parcels in beggining of list 
                }
                else
                {
                    if (p.Priority == IDAL.DO.Priorities.fast)//if p has fast priority
                    {
                        if (tmp.FindLastIndex(parcel => parcel.Priority == IDAL.DO.Priorities.emergency) != -1)//find the index of the last parcel with emergancy priority 
                            tmp[tmp.FindLastIndex(parcel => parcel.Priority == IDAL.DO.Priorities.emergency) + 1] = p;//add fast priority parcels after parcels with emergancy priority
                        else
                            tmp[0] = p;
                    }
                    else
                        tmp.Add(p);//always add normal priority parcels to end of list
                }
            }
        }
    }

    
}
