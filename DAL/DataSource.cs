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
           
            init.drones[0] = new Drone { ID = rnd.Next(0, 1000), model = "xPro", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery=rnd.Next(0,100) };
            init.drones[1] = new Drone { ID = rnd.Next(0, 1000), model = "xMax", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            init.drones[2] = new Drone { ID = rnd.Next(0, 1000), model = "xPlus", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            init.drones[3] = new Drone { ID = rnd.Next(0, 1000), model = "zeo", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            init.drones[4] = new Drone { ID = rnd.Next(0, 1000), model = "xox", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            init.parcels[0] = new Parcel { ID = rnd.Next(0, 1000), senderID = rnd.Next(0, 1000), targetID = 111, weight = (WeightCategories)rnd.Next(0, 2), priority =(Priorities) rnd.Next(0, 2), droneID=11};
            init.parcels[1] = new Parcel { ID = rnd.Next(0, 1000), senderID = rnd.Next(0, 1000), targetID = 222, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 22 };
            init.parcels[2] = new Parcel { ID = rnd.Next(0, 1000), senderID = rnd.Next(0, 1000), targetID = 333, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 33 };
            init.parcels[3] = new Parcel { ID = rnd.Next(0, 1000), senderID = rnd.Next(0, 1000), targetID = 444, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 44 };
            init.parcels[4] = new Parcel { ID = rnd.Next(0, 1000), senderID = rnd.Next(0, 1000), targetID = 555, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 55 };
            init.parcels[5] = new Parcel { ID = rnd.Next(0, 1000), senderID = rnd.Next(0, 1000), targetID = 666, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 66 };
            init.parcels[6] = new Parcel { ID = rnd.Next(0, 1000), senderID = rnd.Next(0, 1000), targetID = 777, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 77 };
            init.parcels[7] = new Parcel { ID = rnd.Next(0, 1000), senderID = rnd.Next(0, 1000), targetID = 888, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 88 };
            init.parcels[8] = new Parcel { ID = rnd.Next(0, 1000), senderID = rnd.Next(0, 1000), targetID = 999, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 99 };
            init.parcels[9] = new Parcel { ID = rnd.Next(0, 1000), senderID = rnd.Next(0, 1000), targetID = 1111, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 115 };

        }
}
}
