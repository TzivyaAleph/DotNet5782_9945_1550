using System;
using System.Collections.Generic;
using System.Text;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        internal static Drone[] drones = new Drone[10];
        internal static Parcel[] parcels = new Parcel[1000];
        internal static Station[] stations = new Station[5];
        internal static Customer[] customers = new Customer[100];
        DataSource() { Initialize(this); }
        internal class Config
        {
            static int availableDrone = 0;
            static int availableParcel = 0;
            static int availableStation = 0;
            static int availableCustomer = 0;
            static int parcelNumber=1020;
        }
        public static void  Initialize(DataSource init)
        {
            Random rnd = new Random();
            init.stations[0] = new Station { lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000), stationName = "Ramot", chargeSlots = rnd.Next(0, 50) };
            init.stations[1] = new Station { lattitude = rnd.Next(0, 1000), longitude = rnd.Next(0, 1000), stationName = "Har", chargeSlots = rnd.Next(0, 50) };
            
            init.drones[0] = new Drone { ID = 11, model = "xPro", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            init.drones[1] = new Drone { ID = 22, model = "xMax", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            init.drones[2] = new Drone { ID = 33, model = "xPlus", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            init.drones[3] = new Drone { ID = 44, model = "zeo", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            init.drones[4] = new Drone { ID = 55, model = "xox", maxWeight = (WeightCategories)rnd.Next(0, 2), status = (DroneStatuses)rnd.Next(0, 2), battery = rnd.Next(0, 100) };
            
           
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


            init.parcels[0] = new Parcel { ID = 10000, senderID = 325, targetID = 111, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 11 };
            init.parcels[1] = new Parcel { ID = 10001, senderID = 442, targetID = 222, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 22 };
            init.parcels[2] = new Parcel { ID = 10002, senderID = 589, targetID = 333, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 33 };
            init.parcels[3] = new Parcel { ID = 10003, senderID = 601, targetID = 444, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 44 };
            init.parcels[4] = new Parcel { ID = 10004, senderID = 641, targetID = 555, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 55 };
            init.parcels[5] = new Parcel { ID = 10005, senderID = 682, targetID = 666, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 66 };
            init.parcels[6] = new Parcel { ID = 10006, senderID = 716, targetID = 777, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 77 };
            init.parcels[7] = new Parcel { ID = 10007, senderID = 885, targetID = 888, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 88 };
            init.parcels[8] = new Parcel { ID = 10008, senderID = 965, targetID = 999, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 99 };
            init.parcels[9] = new Parcel { ID = 10009, senderID = 1001, targetID = 1111, weight = (WeightCategories)rnd.Next(0, 2), priority = (Priorities)rnd.Next(0, 2), droneID = 115 };

        }
    }

    public class DalObject
    {
        public static void AttributingParcelToDrone(Parcel p, Drone d)//function that recieves a parcel and a drone and attributes the parcel to the drone
        {
            p.droneID = d.ID;//updates the parcels drone id to the id of the drone that recieved it
            p.scheduled = DateTime.Today;//updates the parcels schedule time
            d.status = (DroneStatuses)2;//updates the drones status to delivery
        }

        public static void PickedUp(Parcel p)//function that recieves a parcel and updates the parcels picked up time
        {
            p.pickedUp= DateTime.Today;//updates the parcels pickedUp time
        }

        public static void Delivered(Parcel p)//function that recieves a parcel and updates the parcels delivered time
        {
            p.delivered = DateTime.Today;//updates the parcels delivered time
        }

        public static void SendDroneToChargeSlot(Drone d, Station s)//function that recieves a drone and a station and sends the drone to a chargeSlot in that staition
        {
            d.status = (DroneStatuses)1;//updates the drone status to charging
            DroneCharge dc=new DroneCharge();//creates a new drone charge struct with the current drone and station
            dc.droneID = d.ID;
            dc.stationID = s.ID;
            s.chargeSlots--;//updates the available charge slots in the current staition
        }

        public static void ReleaseDrone(Drone d, Station s, DroneCharge dc)//function that recieves a drone and a station and releses the drone from the chargeSlot 
        {
            d.status = (DroneStatuses)0;//updates the drones status to available
            d.battery = 100;
            s.chargeSlots++;//updates the available charge slots in the current staition
        }

   


       

    }
}


