﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        public DalObject() { DataSource.Initialize(); }//c-tor.
        /// <summary>
        /// gets a station and adds it to the array
        /// </summary>
        /// <param Name="s"></param>
        /// <returns></returns>
        public void AddStation(Station s)
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
        public void AddDrone(Drone d)
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
        public void AddCusomer(Customer c)
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
        public int AddParcel(Parcel p)
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
        public void AttributingParcelToDrone(Parcel p, Drone d)
        {
            int index = System.Array.IndexOf(DataSource.Parcels, p);//find the index of the parcel were searching
            DataSource.Parcels[index].DroneID = d.ID;//updates the parcels drone id to the id of the drone that recieved it
            DataSource.Parcels[index].Scheduled = DateTime.Today;//updates the parcels schedule time
        }
        /// <summary>
        /// recieves a parcel and updates the parcels picked up time
        /// </summary>
        /// <param Name="p"></param>
        public void PickedUp(Parcel p, Drone d)
        {
            int index = System.Array.IndexOf(DataSource.Parcels, p);//find the index of the parcel were searching
            DataSource.Parcels[index].DroneID = d.ID;
            DataSource.Parcels[index].PickedUp = DateTime.Today;//updates the parcels pickedUp time
            int indexOfDrones = System.Array.IndexOf(DataSource.Drones, d);//find the index of the drone were searching
            DataSource.Drones[indexOfDrones].Status = (DroneStatuses)2;//changes the status to deliverd.
        }
        /// <summary>
        /// function that recieves a parcel and updates the parcels delivered time
        /// </summary>
        /// <param Name="p"></param>
        public void Delivered(Parcel p)
        {
            int index = System.Array.IndexOf(DataSource.Parcels, p);//find the index of the parcel were searching
            DataSource.Parcels[index].Delivered = DateTime.Today;//updates the parcels delivered time
        }
        /// <summary>
        /// recieves a drone and a station and sends the drone to a chargeSlot in that staition
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        public void SendDroneToChargeSlot(Drone d, Station s)
        {
            int indexOfDrones = System.Array.IndexOf(DataSource.Drones, d);//find the index of the parcel were searching
            DataSource.Drones[indexOfDrones].Status = (DroneStatuses)1;//updates the drone status to charging
            DroneCharge dc = new DroneCharge();//creates a new drone charge object with the current drone and station
            dc.DroneID = d.ID;
            dc.StationID = s.ID;
            DataSource.DroneCharges[DataSource.Config.AvailableDroneCharge++] = dc;
            int index = System.Array.IndexOf(DataSource.Stations, s);
            DataSource.Stations[index].ChargeSlots--;//updates the available charge slots in the current staition
        }
        /// <summary>
        /// recieves a drone and a station and releses the drone from the chargeSlot
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        /// <param Name="dc"></param>
        public void ReleaseDrone(Drone d, Station s, DroneCharge dc)
        {
            int indexOfDrones = System.Array.IndexOf(DataSource.Drones, d);//find the index of the parcel were searching
            DataSource.Drones[indexOfDrones].Status = 0;//updates the drones status to available
            DataSource.Drones[indexOfDrones].Battery = 100;
            int indexOfStations = System.Array.IndexOf(DataSource.Stations, s);//find the index of the parcel were searching
            DataSource.Stations[indexOfStations].ChargeSlots++;//updates the available charge slots in the current staition
            int index = System.Array.IndexOf(DataSource.DroneCharges, dc);
            DataSource.DroneCharges[index].DroneID = 0;
            DataSource.DroneCharges[index].StationID = 0;
        }

        /// <summary>
        /// searches for the droneCharge in the array by the station Id and drone id
        /// </summary>
        /// <param Name="stationID"></param>
        /// <returns></returs the drone charge object were looking for>
        public DroneCharge GetDroneCharge(int stationID, int droneID)
        {
            DroneCharge droneChargeToReturn = new DroneCharge();
            //searches the station with the recieved id.
            foreach (DroneCharge dc in DataSource.DroneCharges)
                if (dc.StationID == stationID && dc.DroneID == droneID)
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
        public Station GetStation(int stationID)
        {
            Station stationToReturn = new Station();
            //searches the station with the recieved id.
            foreach (Station s in DataSource.Stations)
                if (s.ID == stationID)
                {
                    stationToReturn = s;
                }
            return stationToReturn;
        }
        /// <summary>
        /// searches for the drone in the array by the Id
        /// </summary>
        /// <param Name="droneID"></param>
        /// <returns></returnsthe drone were looking for>
        public Drone GetDrone(int droneID)
        {
            Drone droneToReturn = new Drone();
            foreach (Drone d in DataSource.Drones)//searches for the drone 
                if (d.ID == droneID)
                {
                    droneToReturn = d;
                }
            return droneToReturn;
        }
        /// <summary>
        /// searches for the customer in the array by the Id
        /// </summary>
        /// <param Name="customerID"></param>
        /// <returns></returnsthe customer were looking for>
        public Customer GetCustomer(int customerID)
        {
            Customer customerToReturn = new Customer();
            //searches the customer by the id
            foreach (Customer c in DataSource.Customers)
                if (c.ID == customerID)
                {
                    customerToReturn = c;
                }
            return customerToReturn;
        }
        /// <summary>
        /// searches for the parcel in the array by the Id
        /// </summary>
        /// <param Name="parcelID"></param>
        /// <returns></returns parcel were looking for>
        public Parcel GetParcel(int parcelID)
        {
            Parcel parcelToReturn = new Parcel();
            foreach (Parcel p in DataSource.Parcels)
                if (p.ID == parcelID)
                {
                    parcelToReturn = p;
                }
            return parcelToReturn;
        }
        /// <summary>
        /// coppies the station array
        /// </summary>
        /// <returns></returns the coppied array>
        public Station[] CopyStationArray()
        {
            Station[] arr2 = (Station[])DataSource.Stations.Clone();
            return arr2;
        }
        /// <summary>
        /// coppies the drone array
        /// </summary>
        /// <returns></returns the coppied array>
        public Drone[] CopyDroneArray()
        {
            Drone[] arr2 = (Drone[])DataSource.Drones.Clone();
            return arr2;
        }
        /// <summary>
        /// coppies the customer array
        /// </summary>
        /// <returns></returns the coppied array>
        public Customer[] CopyCustomerArray()
        {
            Customer[] arr2 = (Customer[])DataSource.Customers.Clone();
            return arr2;
        }
        /// <summary>
        /// coppies the parcel array
        /// </summary>
        /// <returns></returns the coppied array>
        public Parcel[] CopyParcelArray()
        {
            Parcel[] arr2 = (Parcel[])DataSource.Parcels.Clone();
            return arr2;
        }
        /// <summary>
        /// searches for the non atributted parcels and coppies them into a new array.
        /// </summary>
        /// <returns></returns the new array>
        public Parcel[] FindNotAttributedParcels()
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
        public Station[] FindAvailableStations()
        {
            int j = 0;//for the new array's index
            Station[] availableStations = new Station[50];//new array to hold Available Stations
            for (int i = 0; i < DataSource.Stations.Length; i++)
                if (DataSource.Stations[i].ChargeSlots > 0)
                {
                    availableStations[j] = DataSource.Stations[i];
                    j++;
                }
            return availableStations;
        }

    }
}



