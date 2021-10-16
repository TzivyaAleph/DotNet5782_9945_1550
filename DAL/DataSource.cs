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
            for(int i=0;i<2;i++)
            {
                init.stations[i] = new Station {lattitude=1,longitude=2,stationName="Ramot",chargeSlots=3};
            }
        }
    }
}
