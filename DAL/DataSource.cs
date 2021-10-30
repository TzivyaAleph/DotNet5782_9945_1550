using System;
using System.Collections.Generic;
using System.Text;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        internal static Drone[] Drones = new Drone[10];
        internal static Parcel[] Parcels = new Parcel[15];
        internal static Station[] Stations = new Station[5];
        internal static Customer[] Customers = new Customer[100];
        internal static DroneCharge[] DroneCharges = new DroneCharge[100];
        internal class Config
        {
            internal static int AvailableDrone = 0;
            internal static int AvailableParcel = 0;
            internal static int AvailableStation = 0;
            internal static int AvailableCustomer = 0;
            internal static int AvailableDroneCharge = 0;
            internal static int RunningParcelID=200;

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
                Model = "maxP",
                MaxWeight = (WeightCategories)1,
                Status = (DroneStatuses)2,
                Battery = getRandomDoubleNumber(0, 100)
            };
            Drones[1] = new Drone
            {
                ID = rand.Next(1000, 10000),
                Model = "maxG",
                MaxWeight = (WeightCategories)2,
                Status = (DroneStatuses)1,
                Battery = getRandomDoubleNumber(0, 100)
            };
            Drones[2] = new Drone
            {
                ID = rand.Next(1000, 10000),
                Model = "maxF",
                MaxWeight = (WeightCategories)0,
                Status = (DroneStatuses)0,
                Battery = getRandomDoubleNumber(0, 100)
            };
            Drones[3] = new Drone
            {
                ID = rand.Next(1000, 10000),
                Model = "maxT",
                MaxWeight = (WeightCategories)2,
                Status = 0,
                Battery = getRandomDoubleNumber(0, 100)
            };
            Drones[4] = new Drone
            {
                ID = rand.Next(1000, 10000),
                Model = "maxD",
                MaxWeight = (WeightCategories)2,
                Status = (DroneStatuses)2,
                Battery = getRandomDoubleNumber(0, 100)
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
                StationName = "Ramot",
                ChargeSlots = rand.Next(0, 50),
                Lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                Longitude = (long)getRandomDoubleNumber(-5000, 5000)
            };
            Stations[1] = new Station
            {
                ID = rand.Next(1000, 10000),
                StationName = "Bait Vagan",
                ChargeSlots = rand.Next(0, 50),
                Lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                Longitude = (long)getRandomDoubleNumber(-5000, 5000)
            };
            Config.AvailableStation+=2;
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
                Customers[Config.AvailableCustomer++] = new Customer
                {
                    ID = rand.Next(100000000, 1000000000),
                    Name = $"{(CustomersName)rand.Next(10)}",
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
            DateTime dateAndTime = new DateTime(2021, 1, 1);
            for (int i = 0,j=9; i < NumberOfParcels; i++,j--)//add new Parcels to the array
            {
                Parcels[Config.AvailableParcel++] = new Parcel
                {
                    ID = Config.RunningParcelID++,
                    SenderID = Customers[i].ID,
                    TargetID = Customers[j].ID,
                    Weight = (WeightCategories)rand.Next(3),
                    Priority = (Priorities)rand.Next(3),
                    Requested = dateAndTime,
                    DroneID = Drones[rand.Next(5)].ID,
                    Scheduled=dateAndTime.AddMinutes(rand.Next(10,1000)),
                    PickedUp= dateAndTime.AddMinutes(rand.Next(10, 1000)).AddHours(rand.Next(10, 1000)),
                    Delivered = dateAndTime.AddMinutes(rand.Next(10, 1000)).AddHours(rand.Next(10, 1000)).AddHours(rand.Next(10, 1000))
                };
            }
        }
        /// <summary>
        /// gets  2 dates and return a random date between the 2 dates
        /// </summary>
        /// <param Name="start"></param>
        /// <param Name="end"></param>
        /// <returns></returns a random date between the 2 dates>
        //private static DateTime getRandomDateTime(DateTime start, DateTime end)
        //{ 
        //    int range=(end-start).Days;
        //    return start.AddDays(rand.Next(range));
        //}
    }

    public class DalObject
    {
        public DalObject() { DataSource.Initialize(); }//c-tor.
        /// <summary>
        /// gets a station and adds it to the array
        /// </summary>
        /// <param Name="s"></param>
        /// <returns></returns>
       public static void AddStation(Station s)
        {
            Station temp = new Station();
            temp = s;
            DataSource.Stations[DataSource.Config.AvailableStation] = temp;
            DataSource.Config.AvailableStation++;//updates the availabeStation that new station been edit. 
        }

        /// <addDrone>
        /// add new drone and updates 
        /// </summary>
        /// <returns></returns>
        public static void AddDrone(Drone d)
        {
            Drone temp = new Drone();
            temp = d;
            //add a new drone to the array of drone.
            DataSource.Drones[DataSource.Config.AvailableDrone] = temp;
            DataSource.Config.AvailableDrone++;//updates the availabeDrone that new drone been added. 
        }
        /// <summary>
        /// gets a customer and adds it to the array.
        /// </summary>
        /// <param Name="c"></param>
        /// <returns></returns>
        public static void AddCusomer(Customer c)
        {
            Customer temp = new Customer();
            temp = c;
            DataSource.Customers[DataSource.Config.AvailableCustomer] = temp;
            DataSource.Config.AvailableCustomer++;//updates the availableCustomer that new customer been added. 
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
            DataSource.Parcels[DataSource.Config.AvailableParcel] = temp;
            DataSource.Parcels[DataSource.Config.AvailableParcel].ID = DataSource.Config.RunningParcelID++;
            DataSource.Config.AvailableParcel++;//updates the availableCustomer that new customer been added. 
            return DataSource.Config.AvailableParcel - 1;//return the id of the new  customer.
         }
        /// <summary>
        /// recieves a parcel and a drone and attributes the parcel to the drone
        /// </summary>
        /// <param Name="p"></param>
        /// <param Name="d"></param>
        public static void AttributingParcelToDrone(Parcel p, Drone d)
        {
            p.DroneID = d.ID;//updates the parcels drone id to the id of the drone that recieved it
            p.Scheduled = DateTime.Today;//updates the parcels schedule time
            d.Status = (DroneStatuses)2;//updates the drones status to delivery
        }
        /// <summary>
        /// recieves a parcel and updates the parcels picked up time
        /// </summary>
        /// <param Name="p"></param>
        public static void PickedUp(Parcel p, Drone d)
        {
            p.DroneID = d.ID;
            p.PickedUp = DateTime.Today;//updates the parcels pickedUp time
        }
        /// <summary>
        /// function that recieves a parcel and updates the parcels delivered time
        /// </summary>
        /// <param Name="p"></param>
        public static void Delivered(Parcel p)
        {
            p.Delivered = DateTime.Today;//updates the parcels delivered time
            for(int i=0;i<DataSource.Drones.Length;i++)//updates the current drone's status to available
            {
                if (DataSource.Drones[i].ID == p.DroneID)
                    DataSource.Drones[i].Status = 0;
            }
        }
        /// <summary>
        /// recieves a drone and a station and sends the drone to a chargeSlot in that staition
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        public static void SendDroneToChargeSlot(Drone d, Station s)
        {
            d.Status = (DroneStatuses)1;//updates the drone status to charging
            DroneCharge dc = new DroneCharge();//creates a new drone charge object with the current drone and station
            dc.DroneID = d.ID;
            dc.StationID = s.ID;
            DataSource.DroneCharges[DataSource.Config.AvailableDroneCharge++] = dc;
            int index = System.Array.IndexOf(DataSource.Stations, s);
            DataSource.Stations[index].ChargeSlots--;
            //s.ChargeSlots--;//updates the available charge slots in the current staition
        }
        /// <summary>
        /// recieves a drone and a station and releses the drone from the chargeSlot
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        /// <param Name="dc"></param>
        public static void ReleaseDrone(Drone d, Station s, DroneCharge dc) 
        {
            d.Status = 0;//updates the drones status to available
            d.Battery = 100;
            s.ChargeSlots++;//updates the available charge slots in the current staition
            int index = System.Array.IndexOf(DataSource.DroneCharges, dc);
            DataSource.DroneCharges[index].DroneID = 0;
            DataSource.DroneCharges[index].StationID = 0;
        }

        /// <summary>
        /// searches for the droneCharge in the array by the station Id and drone id
        /// </summary>
        /// <param Name="stationID"></param>
        /// <returns></returs the drone charge object were looking for>
        public static DroneCharge GetDroneCharge(int stationID, int droneID)
        {
            DroneCharge droneChargeToReturn = new DroneCharge();
            //searches the station with the recieved id.
            foreach (DroneCharge dc in DataSource.DroneCharges)
                if (dc.StationID == stationID&&dc.DroneID==droneID)
                {
                    droneChargeToReturn = dc;
                }
            return droneChargeToReturn;
        }
        /// <summary>
        /// searches for the station in the array by the Id
        /// </summary>
        /// <param Name="stationID"></param>
        /// <returns></returs the station were looking for>
        public static Station GetStation(int stationID)
        {
            Station stationToReturn = new Station();
            //searches the station with the recieved id.
            foreach (Station s in DataSource.Stations)
                if (s.ID == stationID)
                {
                    stationToReturn= s;
                }
            return stationToReturn;
        }
        /// <summary>
        /// searches for the drone in the array by the Id
        /// </summary>
        /// <param Name="droneID"></param>
        /// <returns></returnsthe drone were looking for>
        public static Drone GetDrone(int droneID)
        {
            Drone droneToReturn = new Drone();
            foreach (Drone d in DataSource.Drones)//searches for the drone 
                if (d.ID == droneID)
                {
                    droneToReturn= d;
                }
            return droneToReturn;
        }
        /// <summary>
        /// searches for the customer in the array by the Id
        /// </summary>
        /// <param Name="customerID"></param>
        /// <returns></returnsthe customer were looking for>
        public static Customer GetCustomer(int customerID)
        {
            Customer customerToReturn = new Customer();
            //searches the customer by the id
            foreach (Customer c in DataSource.Customers)
                if (c.ID == customerID)
                {
                    customerToReturn= c;
                }
            return customerToReturn;
        }
        /// <summary>
        /// searches for the parcel in the array by the Id
        /// </summary>
        /// <param Name="parcelID"></param>
        /// <returns></returns parcel were looking for>
        public static Parcel GetParcel(int parcelID)
        {
            Parcel parcelToReturn = new Parcel();
            foreach (Parcel p in DataSource.Parcels)
                if (p.ID == parcelID)
                {
                    parcelToReturn= p;
                }
             return parcelToReturn;
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
            Parcel[] notAttributed = new Parcel[1000];//new array to hold non attributed parcels
            foreach (Parcel p in DataSource.Parcels)//searches for the non attributed parcels
            {
                if (p.DroneID != 0)
                {
                    notAttributed[i] = p;
                    i++;
                }
            }
            return notAttributed;
        }
        /// <summary>
        /// creates an array by searching for available charge slots in the station array.
        /// </summary>
        /// <returns></returns the new array>
        public static Station[] FindAvailableStations()
        {
            int j = 0;//for the new array's index
            Station[] availableStations = new Station[50];//new array to hold Available Stations
            for(int i = 0; i<DataSource.Stations.Length;i++)
                if (DataSource.Stations[i].ChargeSlots > 0)
                {
                    availableStations[j] = DataSource.Stations[i];
                    j++;
                }
            return availableStations;

        }


    }
}


