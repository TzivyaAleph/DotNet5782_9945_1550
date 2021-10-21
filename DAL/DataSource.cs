using System;
using System.Collections.Generic;
using System.Text;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        internal static Drone[] Drones = new Drone[10];
        internal static Parcel[] Parcels = new Parcel[1000];
        internal static Station[] Stations = new Station[5];
        internal static Customer[] Customers = new Customer[100];
        internal static DroneCharge[] DroneCharges = new DroneCharge[100];
        internal class Config
        {
            internal static int AvailableDrone = 0;
            internal static int availableParcel = 0;
            internal static int availableStation = 0;
            internal static int availableCustomer = 0;
            internal static int availableDroneCharge = 0;
            internal static int RunningParcelID;
            Random rand = new Random();
            Config()
            {
                RunningParcelID = rand.Next(1000, 2000);//creates a random number between 1000 to 2000
            }
        }

        static Random rand = new Random();
        internal static void Initialize()
        {
            createDrones();
            createStation();
            createCustomer(10);
            createParcels(10);
        }
        private static void createDrones()
        {
            Drones[0] = new Drone
            {
                ID = rand.Next(50),
                model = "maxP",
                maxWeight = (WeightCategories)1,
                status = (DroneStatuses)2,
                battery = getRandomDoubleNumber(0, 100)
            };
            Drones[1] = new Drone
            {
                ID = rand.Next(50),
                model = "maxG",
                maxWeight = (WeightCategories)2,
                status = (DroneStatuses)1,
                battery = getRandomDoubleNumber(0, 100)
            };
            Drones[2] = new Drone
            {
                ID = rand.Next(50),
                model = "maxF",
                maxWeight = (WeightCategories)0,
                status = (DroneStatuses)0,
                battery = getRandomDoubleNumber(0, 100)
            };
            Drones[3] = new Drone
            {
                ID = rand.Next(50),
                model = "maxT",
                maxWeight = (WeightCategories)2,
                status = (DroneStatuses)0,
                battery = getRandomDoubleNumber(0, 100)
            };
            Drones[4] = new Drone
                {
                    ID = rand.Next(50),
                model = "maxF",
                maxWeight = (WeightCategories)2,
                status = (DroneStatuses)2,
                battery = getRandomDoubleNumber(0, 100)
            };
            Config.AvailableDrone+=5;
        }
        static double getRandomDoubleNumber(double min, double max)
        {
            return rand.NextDouble() * (max - min) + min;//return a random duble number 
        }
        private static void createStation()
        {
            Stations[0] = new Station
            {
                ID = rand.Next(1000,10000),
                stationName = "Ramot",
                chargeSlots = rand.Next(0, 50),
                lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                longitude = (long)getRandomDoubleNumber(-5000, 5000)
            };
            Stations[1] = new Station
            {
                ID = rand.Next(1000, 10000),
                stationName = "Bait Vagan",
                chargeSlots = rand.Next(0, 50),
                lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                longitude = (long)getRandomDoubleNumber(-5000, 5000)
            };
            Config.availableStation+=2;
        }
       private static void createCustomer(int NumberOfCustumers)
        {
            for(int i=0;i<NumberOfCustumers;i++)//add new customers to the array
            {
                //update their values.
                Customers[Config.availableCustomer] = new Customer
                {
                    ID = rand.Next(100000000, 1000000000),
                    name = $"{(customersName)rand.Next()}",
                    phoneNumber = $"0{rand.Next(50, 60)}-{rand.Next(1000000, 10000000)}",//random numbers according to the israeli number
                    lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                    longtitude = (long)getRandomDoubleNumber(-5000, 5000),
                };
                Config.availableCustomer++;
            }
        }

        private static void createParcels(int NumberOfParcels)
        {
            for (int i = 0; i < NumberOfParcels; i++)//add new Parcels to the array
            {
                Parcels[Config.availableParcel++] = new Parcel
                {
                    ID = rand.Next(10),
                    senderID =Customers[i].ID,
                    targetID= Customers[i].ID,

                };
            }
        }
    }

    public class DalObject
    {
        DataSource dj;
        DalObject() { DataSource.Initialize(); }

       public static int AddStation(string stationNameSearch,int chargeAvailabe)
        {
            DataSource.Stations[DataSource.Config.availableStation] = new Station
            {
                ID = DataSource.Config.availableStation,
                stationName = stationNameSearch,
                chargeSlots = chargeAvailabe
            };//adding a new station to the station array and update his values.
            DataSource.Config.availableStation++;//updates the availabeStation that new station been edit. 
            return DataSource.Config.availableStation - 1;//return the id of the new  station.
        }

        /// <addDrone>
        /// addsa new drone and updates 
        /// </summary>
        /// <returns></returns>
        public static int   AddDrone()
        {
            //add a new drone to the array of drone.
            DataSource.Drones[DataSource.Config.AvailableDrone] = new Drone {ID= DataSource.Config.AvailableDrone};
            DataSource.Config.AvailableDrone++;//updates the availabeDrone that new drone been added. 
            return DataSource.Config.AvailableDrone - 1;//return the id of the new  drone.
        }

        public static int AddCusomer(string customername,string customerphone)
        {
            //gets new cosumer and updates his values.
            DataSource.Customers[DataSource.Config.availableCustomer] = new Customer
            {
                ID = DataSource.Config.availableCustomer,
                phoneNumber = customerphone,
                name = customername
            };
            DataSource.Config.availableCustomer++;//updates the availableCustomer that new customer been added. 
            return DataSource.Config.availableCustomer - 1;//return the id of the new  customer.
        }

            public static int AddParcel(int sender, int target, WeightCategories ParcelWeight, Priorities HisPriority)
            {
                //gets new cosumer and updates his values.
                DataSource.Parcels[DataSource.Config.availableParcel] = new Parcel
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
            foreach (Station s in DataSource.Stations)
                if (s.ID == stationID)
                {
                    StationToReturn= s;
                }
            return StationToReturn;
        }

        public static Drone GetDrone(int droneID)
        {
            Drone DroneToReturn = new Drone();
            foreach (Drone d in DataSource.Drones)//searches for the drone 
                if (d.ID == droneID)
                {
                    DroneToReturn= d;
                }
            return DroneToReturn;
        }

        public static Customer GetCustomer(int customerID)
        {
            Customer CustomerToReturn = new Customer();
            foreach (Customer c in DataSource.Customers)
                if (c.ID == customerID)
                {
                    CustomerToReturn= c;
                }
            return CustomerToReturn;
        }

        public static Parcel GetParcel(int parcelID)
        {
            Parcel ParcelToReturn = new Parcel();
            foreach (Parcel p in DataSource.Parcels)
                if (p.ID == parcelID)
                {
                    ParcelToReturn= p;
                }
             return ParcelToReturn;
        }

        public static Station [] CopyStationArray()
        {
            Station[] arr2 = (Station[])DataSource.Stations.Clone();
            return arr2;
        }

        public static Drone [] CopyDroneArray()
        {
            Drone[] arr2 = (Drone[])DataSource.Drones.Clone();
            return arr2;
        }

        public static Customer [] CopyCustomerArray()
        {
            Customer[] arr2 = (Customer[])DataSource.Customers.Clone();
            return arr2;
        }

        public static Parcel [] CopyParcelArray()
        {
            Parcel[] arr2 = (Parcel[])DataSource.Parcels.Clone();
            return arr2;
        }

        public static Parcel[] FindNotAttributedParcels()
        {
            int i = 0;//for the array's index
            Parcel[] NotAttributed = new Parcel[1000];//new array to hold non attributed parcels
            foreach (Parcel p in DataSource.Parcels)//searches for the non attributed parcels
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
            foreach (Station s in DataSource.Stations)
                if (s.chargeSlots > 0)
                    availableStations[i] = s;
            return availableStations;

        }


    }
}


