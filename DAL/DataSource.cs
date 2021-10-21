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
            internal static int RunningParcelID=200;
            //Random rand = new Random();
            //Config()
            //{
            //    RunningParcelID = rand.Next(1000, 2000);//creates a random number between 1000 to 2000
            //}
        }

        static Random rand = new Random();
        /// <summary>
        /// initializes the arrays with the entities.
        /// </summary>
        internal static void Initialize()
        {
            createDrones();
            createStation();
            createCustomer(10);
            createParcels(10);
        }
        /// <summary>
        /// /creates 5 drones with random datas
        /// </summary>
        private static void createDrones()
        {
            Drones[0] = new Drone
            {
                ID = rand.Next(1000,10000),
                model = "maxP",
                maxWeight = (WeightCategories)1,
                status = (DroneStatuses)2,
                battery = getRandomDoubleNumber(0, 100)
            };
            Drones[1] = new Drone
            {
                ID = rand.Next(1000, 10000),
                model = "maxG",
                maxWeight = (WeightCategories)2,
                status = (DroneStatuses)1,
                battery = getRandomDoubleNumber(0, 100)
            };
            Drones[2] = new Drone
            {
                ID = rand.Next(1000, 10000),
                model = "maxF",
                maxWeight = (WeightCategories)0,
                status = (DroneStatuses)0,
                battery = getRandomDoubleNumber(0, 100)
            };
            Drones[3] = new Drone
            {
                ID = rand.Next(1000, 10000),
                model = "maxT",
                maxWeight = (WeightCategories)2,
                status = (DroneStatuses)0,
                battery = getRandomDoubleNumber(0, 100)
            };
            Drones[4] = new Drone
            {
                ID = rand.Next(1000, 10000),
                model = "maxD",
                maxWeight = (WeightCategories)2,
                status = (DroneStatuses)2,
                battery = getRandomDoubleNumber(0, 100)
            };
            Config.AvailableDrone+=5;//updates the next available index.
        }
        /// <summary>
        /// gets a maximum and minimum numbers and returns a random double number 
        /// </summary>
        /// <param Name="min"></param>
        /// <param Name="max"></param>
        /// <returns></returns a random double number>
        static double getRandomDoubleNumber(double min, double max)
        {
            return rand.NextDouble() * (max - min) + min;//return a random duble number 
        }
        /// <summary>
        /// creates 2 stations and puts random data. 
        /// </summary>
        private static void createStation()
        {
            Stations[0] = new Station
            {
                ID = rand.Next(1000,10000),
                stationName = "Ramot",
                chargeSlots = rand.Next(0, 50),
                Lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                Longitude = (long)getRandomDoubleNumber(-5000, 5000)
            };
            Stations[1] = new Station
            {
                ID = rand.Next(1000, 10000),
                stationName = "Bait Vagan",
                chargeSlots = rand.Next(0, 50),
                Lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                Longitude = (long)getRandomDoubleNumber(-5000, 5000)
            };
            Config.availableStation+=2;
        }
        /// <summary>
        /// creates 10 customers with random data
        /// </summary>
        /// <param Name="NumberOfCustumers"></param>the amount of new costumers
       private static void createCustomer(int NumberOfCustumers)
        {
            for(int i=0;i<NumberOfCustumers;i++)//add new customers to the array
            {
                //update their values.
                Customers[Config.availableCustomer++] = new Customer
                {
                    ID = rand.Next(100000000, 1000000000),
                    Name = $"{(customersName)rand.Next(10)}",
                    PhoneNumber = $"0{rand.Next(50, 60)}-{rand.Next(1000000, 10000000)}",//random numbers according to the israeli number
                    Lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                    Longtitude = (long)getRandomDoubleNumber(-5000, 5000),
                };
            }
        }
        /// <summary>
        /// gets parcels and updates their data randomely.
        /// </summary>
        /// <param Name="NumberOfParcels"></param>
        private static void createParcels(int NumberOfParcels)
        {
            DateTime start = new DateTime(1, 1, 2020);
            DateTime end = new DateTime(1, 2, 2020);
            for (int i = 0,j=9; i < NumberOfParcels; i++,j--)//add new Parcels to the array
            {
                Parcels[Config.availableParcel++] = new Parcel
                {
                    ID = Config.RunningParcelID,
                    senderID = Customers[i].ID,
                    targetID = Customers[j].ID,
                    weight = (WeightCategories)rand.Next(3),
                    priority = (Priorities)rand.Next(3),
                    requested = getRandomDateTime(start,end),
                    droneID= Drones[rand.Next(5)].ID,
                    scheduled= getRandomDateTime(start.AddDays(31), end.AddDays(31)),
                    pickedUp= getRandomDateTime(start.AddDays(62), end.AddDays(62)),
                    delivered = getRandomDateTime(start.AddDays(93), end.AddDays(93))
                };
            }
        }
        /// <summary>
        /// gets  2 dates and return a random date between the 2 dates
        /// </summary>
        /// <param Name="start"></param>
        /// <param Name="end"></param>
        /// <returns></returns a random date between the 2 dates>
        private static DateTime getRandomDateTime(DateTime start, DateTime end)
        { 
            int range=(end-start).Days;
            return start.AddDays(rand.Next(range));
        }
    }

    public class DalObject
    {
        DataSource dj;
        DalObject() { DataSource.Initialize(); }//c-tor.
        /// <summary>
        /// gets a station and adds it to the array
        /// </summary>
        /// <param Name="s"></param>
        /// <returns></returns>
       public static int AddStation(Station s)
        {
            Station temp = new Station();
            temp = s;
            DataSource.Stations[DataSource.Config.availableStation] = temp;
            DataSource.Config.availableStation++;//updates the availabeStation that new station been edit. 
            return DataSource.Config.availableStation - 1;//return the id of the new  station.
        }

        /// <addDrone>
        /// add new drone and updates 
        /// </summary>
        /// <returns></returns>
        public static int AddDrone(Drone d)
        {
            Drone temp = new Drone();
            temp = d;
            //add a new drone to the array of drone.
            DataSource.Drones[DataSource.Config.AvailableDrone] = temp;
            DataSource.Config.AvailableDrone++;//updates the availabeDrone that new drone been added. 
            return DataSource.Config.AvailableDrone - 1;//return the id of the new  drone.
        }
        /// <summary>
        /// gets a customer and adds it to the array.
        /// </summary>
        /// <param Name="c"></param>
        /// <returns></returns>
        public static int AddCusomer(Customer c)
        {
            Customer temp = new Customer();
            temp = c;
            DataSource.Customers[DataSource.Config.availableCustomer] = temp;
            DataSource.Config.availableCustomer++;//updates the availableCustomer that new customer been added. 
            return DataSource.Config.availableCustomer - 1;//return the id of the new  customer.
        }
        /// <summary>
        /// gets a customer and adds it to the array
        /// </summary>
        /// <param Name="p"></param>
        /// <returns></returns>
        public static int AddParcel(Parcel p)
         {
            Parcel temp = new Parcel();
            p.ID = DataSource.Config.RunningParcelID++;
            temp = p;
            DataSource.Parcels[DataSource.Config.availableParcel] = temp;
            DataSource.Parcels[DataSource.Config.availableParcel].ID = DataSource.Config.RunningParcelID++;
            DataSource.Config.availableParcel++;//updates the availableCustomer that new customer been added. 
            return DataSource.Config.availableParcel - 1;//return the id of the new  customer.
         }
        /// <summary>
        /// recieves a parcel and a drone and attributes the parcel to the drone
        /// </summary>
        /// <param Name="p"></param>
        /// <param Name="d"></param>
        public static void AttributingParcelToDrone(Parcel p, Drone d)
        {
            p.droneID = d.ID;//updates the parcels drone id to the id of the drone that recieved it
            p.scheduled = DateTime.Today;//updates the parcels schedule time
            d.status = (DroneStatuses)2;//updates the drones status to delivery
        }
        /// <summary>
        /// recieves a parcel and updates the parcels picked up time
        /// </summary>
        /// <param Name="p"></param>
        public static void PickedUp(Parcel p)
        {
            p.pickedUp = DateTime.Today;//updates the parcels pickedUp time
        }
        /// <summary>
        /// function that recieves a parcel and updates the parcels delivered time
        /// </summary>
        /// <param Name="p"></param>
        public static void Delivered(Parcel p)
        {
            p.delivered = DateTime.Today;//updates the parcels delivered time
        }
        /// <summary>
        /// recieves a drone and a station and sends the drone to a chargeSlot in that staition
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        public static void SendDroneToChargeSlot(Drone d, Station s)
        {
            d.status = (DroneStatuses)1;//updates the drone status to charging
            DroneCharge dc = new DroneCharge();//creates a new drone charge object with the current drone and station
            dc.droneID = d.ID;
            dc.stationID = s.ID;
            DataSource.DroneCharges[DataSource.Config.availableDroneCharge++] = dc;
            s.chargeSlots--;//updates the available charge slots in the current staition
        }
        /// <summary>
        /// recieves a drone and a station and releses the drone from the chargeSlot
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        /// <param Name="dc"></param>
        public static void ReleaseDrone(Drone d, Station s, DroneCharge dc) 
        {
            d.status = (DroneStatuses)0;//updates the drones status to available
            d.battery = 100;
            s.chargeSlots++;//updates the available charge slots in the current staition
            int index = System.Array.IndexOf(DataSource.DroneCharges, dc);
            DataSource.DroneCharges[index].droneID = 0;
            DataSource.DroneCharges[index].stationID = 0;
        }

        /// <summary>
        /// searches for the station in the array by the Id
        /// </summary>
        /// <param Name="stationID"></param>
        /// <returns></returs the station were looking for>
        public static Station GetStation(int stationID)
        {
            Station StationToReturn = new Station();
            //searches the station with the recieved id.
            foreach (Station s in DataSource.Stations)
                if (s.ID == stationID)
                {
                    StationToReturn= s;
                }
            return StationToReturn;
        }
        /// <summary>
        /// searches for the drone in the array by the Id
        /// </summary>
        /// <param Name="droneID"></param>
        /// <returns></returnsthe drone were looking for>
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
        /// <summary>
        /// searches for the customer in the array by the Id
        /// </summary>
        /// <param Name="customerID"></param>
        /// <returns></returnsthe customer were looking for>
        public static Customer GetCustomer(int customerID)
        {
            Customer CustomerToReturn = new Customer();
            //searches the customer by the id
            foreach (Customer c in DataSource.Customers)
                if (c.ID == customerID)
                {
                    CustomerToReturn= c;
                }
            return CustomerToReturn;
        }
        /// <summary>
        /// searches for the parcel in the array by the Id
        /// </summary>
        /// <param Name="parcelID"></param>
        /// <returns></returns parcel were looking for>
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
        /// <summary>
        /// coppies the station array
        /// </summary>
        /// <returns></returns the coppied array>
        public static Station [] CopyStationArray()
        {
            Station[] arr2 = (Station[])DataSource.Stations.Clone();
            return arr2;
        }
        /// <summary>
        /// coppies the drone array
        /// </summary>
        /// <returns></returns the coppied array>
        public static Drone [] CopyDroneArray()
        {
            Drone[] arr2 = (Drone[])DataSource.Drones.Clone();
            return arr2;
        }
        /// <summary>
        /// coppies the customer array
        /// </summary>
        /// <returns></returns the coppied array>
        public static Customer [] CopyCustomerArray()
        {
            Customer[] arr2 = (Customer[])DataSource.Customers.Clone();
            return arr2;
        }
        /// <summary>
        /// coppies the parcel array
        /// </summary>
        /// <returns></returns the coppied array>
        public static Parcel [] CopyParcelArray()
        {
            Parcel[] arr2 = (Parcel[])DataSource.Parcels.Clone();
            return arr2;
        }
        /// <summary>
        /// searches for the non atributted parcels and coppies them into a new array.
        /// </summary>
        /// <returns></returns the new array>
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
        /// <summary>
        /// creates an array by searching for available charge slots in the station array.
        /// </summary>
        /// <returns></returns the new array>
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


////gets new cosumer and updates his values.
//DataSource.Customers[DataSource.Config.availableCustomer] = new Customer
//{
//    ID = DataSource.Config.availableCustomer,
//    PhoneNumber = customerphone,
//    Name = customername
//};

//gets new cosumer and updates his values.
//DataSource.Parcels[DataSource.Config.availableParcel] = new Parcel
//{
//    ID = DataSource.Config.availableParcel,
//    senderID = sender,
//    targetID = target,
//    weight = ParcelWeight,
//    priority = HisPriority,
//    requested = DateTime.Today,//the parcel has been ready today
//    droneID = 0//no drone has been costumed yet
//};