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
            internal static int AvailableParcel = 0;
            internal static int AvailableStation = 0;
            internal static int AvailableCustomer = 0;
            internal static int AvailableDroneCharge = 0;
            internal static int RunningParcelID = 200;

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
                ID = rand.Next(1000, 10000),
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
            Config.AvailableDrone += 5;//updates the next available index.
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
                ID = rand.Next(1000, 10000),
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
            Config.AvailableStation += 2;
        }
        /// <summary>
        /// creates 10 customers with random data
        /// </summary>
        /// <param Name="NumberOfCustumers"></param>the amount of new costumers
        private static void createCustomer(int NumberOfCustumers)
        {
            for (int i = 0; i < NumberOfCustumers; i++)//add new customers to the array
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
            for (int i = 0, j = 9; i < NumberOfParcels; i++, j--)//add new Parcels to the array
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
                    Scheduled = dateAndTime.AddMinutes(rand.Next(10, 1000)),
                    PickedUp = dateAndTime.AddMinutes(rand.Next(10, 1000)).AddHours(rand.Next(10, 1000)),
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

}
