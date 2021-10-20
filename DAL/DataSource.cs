using System;
using System.Collections.Generic;
using System.Text;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        internal static Drone[] drones = new Drone[10];//array of 10 drones
        internal static Parcel[] parcels = new Parcel[1000];//array of 1000 parcels
        internal static Station[] stations = new Station[5];//array of 5 stations
        internal static Customer[] customers = new Customer[100];//array of 100 costumers.
        internal class Config
        {
            internal static int availableDrone = 0;
            internal static int availableParcel = 0;
            internal static int availableStation = 0;
            internal static int availableCustomer = 0;
            static int parcelNumber;
            Random rand = new Random();
            Config()
            {
                parcelNumber = rand.Next(1000, 2000);//creates a random number between 1000 to 2000
            }
        }
        internal static void Initialize()
        {

        }

        
    }

    public class DalObject
    {
        DataSource dj;
        DalObject() { DataSource.Initialize(); }

       public static int addStation(string stationNameSearch,int chargeAvailabe)
        {
            DataSource.stations[DataSource.Config.availableStation] = new Station
            {
                ID = DataSource.Config.availableStation,
                stationName = stationNameSearch,
                chargeSlots = chargeAvailabe
            };//adding a new station to the station array and update his values.
            DataSource.Config.availableStation++;//updates the availabeStation that new station been edit. 
            return DataSource.Config.availableStation - 1;//return the id of the new  station.
        }

        /// <addDrone>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int addDrone()
        {
            //add a new drone to the array of drone.
            DataSource.drones[DataSource.Config.availableDrone] = new Drone {ID= DataSource.Config.availableDrone};
            DataSource.Config.availableDrone++;//updates the availabeDrone that new drone been added. 
            return DataSource.Config.availableDrone - 1;//return the id of the new  drone.
        }

        public static int addCusomer(string customername,string customerphone)
        {
            //gets new cosumer and updates his values.
            DataSource.customers[DataSource.Config.availableCustomer] = new Customer
            {
                ID = DataSource.Config.availableCustomer,
                phoneNumber = customerphone,
                name = customername
            };
            DataSource.Config.availableCustomer++;//updates the availableCustomer that new customer been added. 
            return DataSource.Config.availableCustomer - 1;//return the id of the new  customer.
        }

        public static int addParcel(int sender,int target,WeightCategories ParcelWeight, Priorities HisPriority)
        {
            //gets new cosumer and updates his values.
            DataSource.parcels[DataSource.Config.availableParcel] = new Parcel
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


    }
}


