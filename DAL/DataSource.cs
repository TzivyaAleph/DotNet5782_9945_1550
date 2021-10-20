using System;
using System.Collections.Generic;
using System.Text;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        internal static Drone[] drones = new Drone[10];
        internal static Parcel[] parcels = new Parcel[1000];
        internal static Station[] stations = new Station[5];
        internal static Customer[] customers = new Customer[100];
        internal static DroneCharge[] DroneCharges = new DroneCharge[100];
        internal class Config
        {
            internal static int availableDrone = 0;
            internal static int availableParcel = 0;
            internal static int availableStation = 0;
            internal static int availableCustomer = 0;
            internal static int availableDroneCharge = 0;
            static int parcelNumber;
            Random rand = new Random();
            Config()
            {
                parcelNumber = rand.Next(1000, 2000);//creates a random number between 1000 to 2000
            }
        }
        internal static void Initialize()
        {

        }


    }

    public class DalObject
    {
        DataSource dj;
        DalObject() { DataSource.Initialize(); }

        public static int addStation(string stationNameSearch, int chargeAvailabe)
        {
            DataSource.stations[DataSource.Config.availableStation] = new Station
            {
                ID = DataSource.Config.availableStation,
                stationName = stationNameSearch,
                chargeSlots = chargeAvailabe
            };//adding a new station to the station array and update his values.
            DataSource.Config.availableStation++;//updates the availabeStation that new station been edit. 
            return DataSource.Config.availableStation - 1;//return the id of the new  station.
        }

        /// <addDrone>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int addDrone()
        {
            //add a new drone to the array of drone.
            DataSource.drones[DataSource.Config.availableDrone] = new Drone { ID = DataSource.Config.availableDrone };
            DataSource.Config.availableDrone++;//updates the availabeDrone that new drone been added. 
            return DataSource.Config.availableDrone - 1;//return the id of the new  drone.
        }

        public static int addCusomer(string customername, string customerphone)
        {
            //gets new cosumer and updates his values.
            DataSource.customers[DataSource.Config.availableCustomer] = new Customer
            {
                ID = DataSource.Config.availableCustomer,
                phoneNumber = customerphone,
                name = customername
            };
            DataSource.Config.availableCustomer++;//updates the availableCustomer that new customer been added. 
            return DataSource.Config.availableCustomer - 1;//return the id of the new  customer.
        }

        public static int addParcel(int sender, int target, WeightCategories ParcelWeight, Priorities HisPriority)
        {
            //gets new cosumer and updates his values.
            DataSource.parcels[DataSource.Config.availableParcel] = new Parcel
            {
                ID = DataSource.Config.availableParcel,
                senderID = sender,
                targetID = target,
                weight = ParcelWeight,
                priority = HisPriority,
                requested = DateTime.Today,//the parcel has been ready today
                droneID = 0//no drone has been costumed yet
            };
            DataSource.Config.availableParcel++;//updates the availableCustomer that new customer been added. 
            return DataSource.Config.availableParcel - 1;//return the id of the new  customer.
        }

        public static void AttributingParcelToDrone(Parcel p, Drone d)//function that recieves a parcel and a drone and attributes the parcel to the drone
        {
            p.droneID = d.ID;//updates the parcels drone id to the id of the drone that recieved it
            p.scheduled = DateTime.Today;//updates the parcels schedule time
            d.status = (DroneStatuses)2;//updates the drones status to delivery
        }

        public static void PickedUp(Parcel p)//function that recieves a parcel and updates the parcels picked up time
        {
            p.pickedUp = DateTime.Today;//updates the parcels pickedUp time
        }

        public static void Delivered(Parcel p)//function that recieves a parcel and updates the parcels delivered time
        {
            p.delivered = DateTime.Today;//updates the parcels delivered time
        }

        public static void SendDroneToChargeSlot(Drone d, Station s)//function that recieves a drone and a station and sends the drone to a chargeSlot in that staition
        {
            d.status = (DroneStatuses)1;//updates the drone status to charging
            DroneCharge dc = new DroneCharge();//creates a new drone charge struct with the current drone and station
            dc.droneID = d.ID;
            dc.stationID = s.ID;
            DataSource.DroneCharges[DataSource.Config.availableDroneCharge] = dc;
            s.chargeSlots--;//updates the available charge slots in the current staition
        }

        public static void ReleaseDrone(Drone d, Station s, DroneCharge dc)//function that recieves a drone and a station and releses the drone from the chargeSlot 
        {
            d.status = (DroneStatuses)0;//updates the drones status to available
            d.battery = 100;
            s.chargeSlots++;//updates the available charge slots in the current staition
            int index = System.Array.IndexOf(DataSource.DroneCharges, dc);
            DataSource.DroneCharges[index].droneID = 0;
            DataSource.DroneCharges[index].stationID = 0;
        }


        public static Station GetStation(int stationID)
        {
            Station StationToReturn = new Station();
            foreach (Station s in DataSource.stations)
                if (s.ID == stationID)
                {
                    StationToReturn= s;
                }
            return StationToReturn;
        }

        public static Drone GetDrone(int droneID)
        {
            Drone DroneToReturn = new Drone();
            foreach (Drone d in DataSource.drones)//searches for the drone 
                if (d.ID == droneID)
                {
                    DroneToReturn= d;
                }
            return DroneToReturn;
        }

        public static Customer GetCustomer(int customerID)
        {
            Customer CustomerToReturn = new Customer();
            foreach (Customer c in DataSource.customers)
                if (c.ID == customerID)
                {
                    CustomerToReturn= c;
                }
            return CustomerToReturn;
        }

        public static Parcel GetParcel(int parcelID)
        {
            Parcel ParcelToReturn = new Parcel();
            foreach (Parcel p in DataSource.parcels)
                if (p.ID == parcelID)
                {
                    ParcelToReturn= p;
                }
             return ParcelToReturn;
        }

        public static Station [] CopyStationArray()
        {
            Station[] arr2 = (Station[])DataSource.stations.Clone();
            return arr2;
        }

        public static Drone [] CopyDroneArray()
        {
            Drone[] arr2 = (Drone[])DataSource.drones.Clone();
            return arr2;
        }

        public static Customer [] CopyCustomerArray()
        {
            Customer[] arr2 = (Customer[])DataSource.customers.Clone();
            return arr2;
        }

        public static Parcel [] CopyParcelArray()
        {
            Parcel[] arr2 = (Parcel[])DataSource.parcels.Clone();
            return arr2;
        }

        public static Parcel[] FindNotAttributedParcels()
        {
            int i = 0;//for the array's index
            Parcel[] NotAttributed = new Parcel[1000];//new array to hold non attributed parcels
            foreach (Parcel p in DataSource.parcels)//searches for the non attributed parcels
            {
                if (p.droneID != 0)
                {
                    NotAttributed[i] = p;
                    i++;
                }
            }
            return NotAttributed;
        }

        public static Station[] FindAvailableStations()
        {
            int i = 0;//for the array's index
            Station[] availableStations = new Station[50];//new array to hold Available Stations
            foreach (Station s in DataSource.stations)
                if (s.chargeSlots > 0)
                    availableStations[i] = s;
            return availableStations;

        }


    }
}


