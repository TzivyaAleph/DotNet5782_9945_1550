using System;
using System.Collections.Generic;
using System.Text;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<Station> Stations = new List<Station>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();

        /// <summary>
        /// class for static variables for each array.
        /// </summary>
        internal class Config
        {
            internal static int RunningParcelID = 200;

            internal static double Light { get => 10; }
            internal static double Avalaible { get => 5; }
            internal static double Heavy { get => 25; }
            internal static double Medium { get => 20; }
            internal static double ChargingRate { get => 30; }//per hour
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
        /// 
        private static void createDrones()
        {
            Drones.Add(new Drone
            {
                ID = rand.Next(1000, 10000),
                Model = "maxP",
                MaxWeight = (WeightCategories)1,
            });
            Drones.Add(new Drone
            {
                ID = rand.Next(1000, 10000),
                Model = "maxG",
                MaxWeight = (WeightCategories)2,
            });
            Drones.Add(new Drone
            {
                ID = rand.Next(1000, 10000),
                Model = "maxF",
                MaxWeight = (WeightCategories)0,
            });
            Drones.Add(new Drone
            {
                ID = rand.Next(1000, 10000),
                Model = "maxT",
                MaxWeight = (WeightCategories)2,
            });
            Drones.Add(new Drone
            {
                ID = rand.Next(1000, 10000),
                Model = "maxD",
                MaxWeight = (WeightCategories)2,
            });
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
            Stations.Add(new Station
            {
                ID = rand.Next(1000, 10000),
                StationName = "Ramot",
                ChargeSlots = rand.Next(0, 50),
                Lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                Longitude = (long)getRandomDoubleNumber(-5000, 5000)
            });
            Stations.Add( new Station
            {
                ID = rand.Next(1000, 10000),
                StationName = "Bait Vagan",
                ChargeSlots = rand.Next(0, 50),
                Lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                Longitude = (long)getRandomDoubleNumber(-5000, 5000)
            });

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
                Customers.Add(
                    new Customer
                    {
                        ID = rand.Next(100000000, 1000000000),
                        Name = $"{(CustomersName)rand.Next(10)}",
                        PhoneNumber = $"0{rand.Next(50, 60)}-{rand.Next(1000000, 10000000)}",//random numbers according to the israeli number
                        Lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                        Longtitude = (long)getRandomDoubleNumber(-5000, 5000),
                    });
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
                Parcels.Add(new Parcel
                {
                    ID = ++Config.RunningParcelID,
                    SenderID = Customers[i].ID,
                    TargetID = Customers[j].ID,
                    Weight = RandomEnumValue<WeightCategories>(),
                    Priority = RandomEnumValue<Priorities>(),
                    Requested = dateAndTime,
                    DroneID = Drones[rand.Next(5)].ID,
                    Scheduled = dateAndTime.AddMinutes(rand.Next(10, 1000)),
                    PickedUp = dateAndTime.AddMinutes(rand.Next(10, 1000)).AddHours(rand.Next(10, 1000)),
                    Delivered = dateAndTime.AddMinutes(rand.Next(10, 1000)).AddHours(rand.Next(10, 1000)).AddHours(rand.Next(10, 1000))
                });
            }
        }

        /// <summary>
        /// function for random enums.
        /// https://stackoverflow.com/questions/3132126/how-do-i-select-a-random-value-from-an-enumeration?noredirect=1&lq=1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(rand.Next(v.Length));
        }

    }

}
