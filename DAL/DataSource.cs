using System;
using System.Collections.Generic;
using System.Text;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        internal Drone[] drones = new Drone[10];
        internal Parcel[] parcels = new Parcel[1000];
        internal Station[] stations = new Station[5];
        internal Customer[] customers = new Customer[100];
        internal class Config
        {
            static int availableDrone = 0;
            static int availableParcel = 0;
            static int availableStation = 0;
            static int availableCustomer = 0;
            int parcelNumber;
            Random rand = new Random();
            Config()
            {
                int parcelNumber = rand.Next(1000, 2000);//creates a random number between 1000 to 2000
            }
        }
        public static void  Initialize(DataSource init)
        {
            Random rnd = new Random();
            init.stations[0] = new Station {lattitude=rnd.Next(0,1000),longitude= rnd.Next(0, 1000), stationName= "Ramot", chargeSlots= rnd.Next(0, 50) };
            init.stations[1] = new Station { lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000), stationName = "Har", chargeSlots = rnd.Next(0, 50) };
           
            init.drones[0] = new Drone { ID = 11, model = "xPro", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery=rnd.Next(0,100) };
            init.drones[1] = new Drone { ID = 22, model = "xMax", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            init.drones[2] = new Drone { ID = 33, model = "xPlus", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            init.drones[3] = new Drone { ID = 44, model = "zeo", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            init.drones[4] = new Drone { ID = 55, model = "xox", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };

            init.customers[0] = new Customer { ID = 111, name = "Avi", phoneNumber = rnd.Next(0, 10000000), lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000) };
            init.customers[1] = new Customer { ID = 222, name = "Bennie", phoneNumber = rnd.Next(0, 10000000), lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000) };
            init.customers[2] = new Customer { ID = 333, name = "Sahar", phoneNumber = rnd.Next(0, 10000000), lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000) };
            init.customers[3] = new Customer { ID = 444, name = "David", phoneNumber = rnd.Next(0, 10000000), lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000) };
            init.customers[4] = new Customer { ID = 555, name = "Guy", phoneNumber = rnd.Next(0, 10000000), lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000) };
            init.customers[5] = new Customer { ID = 666, name = "Justin", phoneNumber = rnd.Next(0, 10000000), lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000) };
            init.customers[6] = new Customer { ID = 777, name = "Bob", phoneNumber = rnd.Next(0, 10000000), lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000) };
            init.customers[7] = new Customer { ID = 888, name = "Alice", phoneNumber = rnd.Next(0, 10000000), lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000) };
            init.customers[8] = new Customer { ID = 999, name = "Noa", phoneNumber = rnd.Next(0, 10000000), lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000) };
            init.customers[9] = new Customer { ID = 1111, name = "Sara", phoneNumber = rnd.Next(0, 10000000), lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000) };

        }
    }
}
