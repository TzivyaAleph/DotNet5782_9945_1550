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

            

        }
    }
}
