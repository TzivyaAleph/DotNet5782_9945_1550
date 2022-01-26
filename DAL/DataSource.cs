using System;
using System.Collections.Generic;
using System.Text;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal class DataSource
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

            internal static double Light { get => 0.00001; }
            internal static double Avalaible { get => 0.00001; }
            internal static double Heavy { get => 0.00004; }
            internal static double Medium { get => 0.00002; }
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
                Id = rand.Next(1000, 2000),
                Model = "maxP",
                Weight = (Weight)2,
            });
            Drones.Add(new Drone
            {
                Id = rand.Next(2001, 3000),
                Model = "maxG",
                Weight = (Weight)2,
            });
            Drones.Add(new Drone
            {
                Id = rand.Next(3001, 4000),
                Model = "maxF",
                Weight = (Weight)2,
            });
            Drones.Add(new Drone
            {
                Id = rand.Next(4001, 5000),
                Model = "maxT",
                Weight = (Weight)2,
            });
            Drones.Add(new Drone
            {
                Id = rand.Next(5001, 10000),
                Model = "maxD",
                Weight = (Weight)2,
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
                Id = rand.Next(1000, 5000),
                Name = "Ramot",
                ChargeSlots = rand.Next(0, 51),
                Lattitude = (long)getRandomDoubleNumber(31.75, 31.9),
                Longitude = (long)getRandomDoubleNumber(35.195, 35.2)
            });
            Stations.Add(new Station
            {
                Id = rand.Next(5001, 10000),
                Name = "Bait Vagan",
                ChargeSlots = rand.Next(0, 50),
                Lattitude = (long)getRandomDoubleNumber(31.75, 31.9),
                Longitude = (long)getRandomDoubleNumber(35.195, 35.2)
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
                Customer toAdd = new Customer
                {
                    Id = rand.Next(100000000, 1000000000),
                    Name = $"{(CustomersName)rand.Next(10)}",
                    PhoneNumber = $"0{rand.Next(50, 60)}-{rand.Next(1000000, 10000000)}",//random numbers according to the israeli number
                    Lattitude = (long)getRandomDoubleNumber(31.75, 31.9),
                    Longtitude = (long)getRandomDoubleNumber(35.195, 35.2),
                    
                };
                if (i % 2 == 0)
                    toAdd.CustomerType = CustomersType.Customer; 
                else
                    toAdd.CustomerType = CustomersType.Manager;
                toAdd.Password = toAdd.Id.ToString();
                //customer is a manager
                //if (toAdd.CustomerType == (CustomersType)0)
                //    toAdd.Password = "123456";
                ////customer is an employee
                //else if (toAdd.CustomerType == (CustomersType)1)
                //    toAdd.Password = "1234";
                ////customer is a customer
                //else
                //    toAdd.Password = toAdd.Id.ToString();
                if (Customers.Exists(item => item.Id == toAdd.Id || item.PhoneNumber == toAdd.PhoneNumber))
                    i--;
                else
                    Customers.Add(toAdd);
            }
        }

        /// <summary>
        /// gets parcels and updates their data randomely.
        /// </summary>
        /// <param Name="NumberOfParcels"></param>
        private static void createParcels(int NumberOfParcels)
        {
            DateTime dateAndTime = new DateTime(2021, 1, 1);
            for (int i = 0, j = 9, m = 0; i < NumberOfParcels; i++, j--, m++)//add new Parcels to the array
            {
                Parcel toAdd = new Parcel();
                toAdd.Id = ++Config.RunningParcelID;
                toAdd.SenderID = Customers[i].Id;
                toAdd.TargetID = Customers[j].Id;
                toAdd.Weight = RandomEnumValue<Weight>();
                toAdd.Priority = RandomEnumValue<Priority>();
                toAdd.Requested = dateAndTime;
                int numDrone = m;
                while ((int)Drones[numDrone].Weight < (int)toAdd.Weight)
                {
                    numDrone += 1;
                    if (numDrone == 4)
                        numDrone = 0;
                }
                toAdd.DroneID = Drones[numDrone].Id;
                if (m == 4)
                    m = -1;
                if (i < 2)
                {
                    toAdd.Scheduled = null;
                    toAdd.PickedUp = null;
                    toAdd.Delivered = null;
                }
                else
                {
                    toAdd.Scheduled = dateAndTime.AddMinutes(rand.Next(10, 1000));
                    toAdd.PickedUp = dateAndTime.AddMinutes(rand.Next(10, 1000)).AddHours(rand.Next(10, 1000));
                    toAdd.Delivered = dateAndTime.AddMinutes(rand.Next(10, 1000)).AddHours(rand.Next(10, 1000)).AddHours(rand.Next(10, 1000));
                }
                Parcels.Add(toAdd);
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
