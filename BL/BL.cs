﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using IDAL;


namespace BL
{
    public partial class BL : IBL
    {

        IDAL.DO.IDal myDal;
        internal static Random rand = new Random();
        internal List<DroneForList> drones;

        /// <summary>
        /// initializing list of drones.
        /// </summary>
        public BL()
        {
            myDal = new DalObject.DalObject();
            drones = new List<DroneForList>();
            double[] electricityUse = myDal.GetElectricityUse();
            double availableElectricityUse = electricityUse[0];
            double lightElectricityUse = electricityUse[1];
            double standardElectricityUse = electricityUse[2];
            double heavyElectricityUse = electricityUse[3];
            double chargePerHour = electricityUse[4];
            //goes throuhgh the da list drones
            List<IDAL.DO.Parcel> dalParcels = myDal.CopyParcelArray().ToList();
            foreach (var item in myDal.CopyDroneArray())
            {
                DroneForList droneToAdd = new DroneForList();//to add to the list
                droneToAdd.Id = item.Id;
                droneToAdd.Model = item.Model;
                droneToAdd.Weight = new Weight();
                droneToAdd.Weight = (Weight)item.MaxWeight;
                droneToAdd.CurrentLocation = new Location();
                //goes through the parcels list in dal for the exstract fields from drone for list
                IDAL.DO.Parcel par = new IDAL.DO.Parcel();
                try
                {
                    par = myDal.CopyParcelArray().First(parcel => parcel.DroneID == item.Id);
                }
                catch (InvalidOperationException)
                {
                    throw new InputDoesNotExist("the parcel does not exist!!");
                }
                IDAL.DO.Station clossestStation = new IDAL.DO.Station();
                // the drone has been attributted but the parcel was not delievred.
                if (par.Delivered == null)
                {
                    droneToAdd.DroneStatuses = DroneStatuses.Delivered;
                    droneToAdd.ParcelId=par.Id;
                    //finds the customer who send the parcel.
                    IDAL.DO.Customer dalSender = new IDAL.DO.Customer();
                    IDAL.DO.Customer dalTarget = new IDAL.DO.Customer();

                    try
                    {
                        dalSender = myDal.CopyCustomerArray().First(item => item.Id == par.SenderID);//finds the parcels sender
                        dalTarget = myDal.CopyCustomerArray().First(item => item.Id == par.TargetID);//finds the parcels target
                    }
                    catch (InvalidOperationException)
                    {
                        throw new InputDoesNotExist("the customer does not exist!!");
                    }
                    clossestStation = myDal.GetClossestStation(dalSender.Lattitude, dalSender.Longtitude, myDal.CopyStationArray().ToList());
                    double batteryUseFromSenderToTarget = myDal.getDistanceFromLatLonInKm(dalTarget.Lattitude, dalTarget.Longtitude, dalSender.Lattitude, dalSender.Longtitude) * batteryByWeight(droneToAdd.Weight);
                    IDAL.DO.Station clossestStationToTarget = new IDAL.DO.Station();
                    clossestStationToTarget = myDal.GetClossestStation(dalTarget.Lattitude, dalTarget.Longtitude, myDal.CopyStationArray().ToList());//finds the clossest station to the target.
                    double batteryUseFromTargetrToStation = myDal.getDistanceFromLatLonInKm(dalTarget.Lattitude, dalTarget.Longtitude, clossestStationToTarget.Lattitude, clossestStationToTarget.Longitude) * availableElectricityUse;
                    //the drone has been attributted but wasnt picked up
                    if (par.PickedUp == null)
                    {
                        //the current location is the clossest station to the sender
                        droneToAdd.CurrentLocation.Latitude = clossestStation.Lattitude;
                        droneToAdd.CurrentLocation.Longitude = clossestStation.Longitude;
                        double batteryUseFromStationToSender = myDal.getDistanceFromLatLonInKm(clossestStation.Lattitude, clossestStation.Longitude, dalSender.Lattitude, dalSender.Longtitude) * availableElectricityUse;
                        double minBatteryForUnpickUp = batteryUseFromStationToSender + batteryUseFromSenderToTarget + batteryUseFromTargetrToStation;
                        if (minBatteryForUnpickUp > 100)
                            minBatteryForUnpickUp = 100;
                        droneToAdd.Battery = getRandomDoubleNumber(minBatteryForUnpickUp, 100);
                    }
                    //the parcel has been picked up
                    else
                    {
                        droneToAdd.CurrentLocation.Latitude = dalSender.Lattitude;
                        droneToAdd.CurrentLocation.Longitude = dalSender.Longtitude;
                        double minBatteryForPickUp = batteryUseFromSenderToTarget + batteryUseFromTargetrToStation;
                        droneToAdd.Battery = getRandomDoubleNumber(minBatteryForPickUp, 100);
                    }

                }
                //the drone has been attributted but is not executing a delievery.
                else
                {
                    int num = rand.Next(0, 2) * 2;
                    if (num == 0)
                        droneToAdd.DroneStatuses = DroneStatuses.Available;
                    else if (num == 2)
                        droneToAdd.DroneStatuses = DroneStatuses.Maintenance;
                }
                //the drone is maintanse
                if (droneToAdd.DroneStatuses == DroneStatuses.Maintenance)
                {
                    //the location is the location of a random station.
                    int num = rand.Next(0, myDal.CopyStationArray().Count());
                    IDAL.DO.Station randomStation = new IDAL.DO.Station();
                    randomStation = myDal.CopyStationArray().ElementAt(num);//finds the station by the random number
                    droneToAdd.CurrentLocation.Latitude = randomStation.Lattitude;
                    droneToAdd.CurrentLocation.Longitude = randomStation.Longitude;
                    droneToAdd.Battery = rand.Next(0, 21);
                    IDAL.DO.Drone dalDrone = new IDAL.DO.Drone();
                    dalDrone.Id = droneToAdd.Id;
                    dalDrone.Model = droneToAdd.Model;
                    dalDrone.MaxWeight = (IDAL.DO.Weight)droneToAdd.Weight;
                    try
                    {
                        myDal.SendDroneToChargeSlot(dalDrone, randomStation);
                    }
                    catch (IDAL.DO.ExistingObjectException custEx)
                    {
                        throw new FailedToUpdateException("ERROR", custEx);
                    }
                }
                //the drone is available.
                if (droneToAdd.DroneStatuses == DroneStatuses.Available)
                {
                    //the cuurent location of the drone is the location of a random
                    //customer who has attributted parcel who hasnt been delieverd yet.
                    List<IDAL.DO.Customer> CustomersWithDelieverdParcel = new List<IDAL.DO.Customer>();
                    CustomersWithDelieverdParcel = myDal.CopyCustomerArray(x => myDal.CopyParcelArray().ToList().FindIndex(par => par.TargetID == x.Id && par.Delivered != null) == -1).ToList();
                    int num = rand.Next(0, CustomersWithDelieverdParcel.Count());
                    IDAL.DO.Customer randomCustomer = new IDAL.DO.Customer();
                    randomCustomer = CustomersWithDelieverdParcel.ElementAt(num);//finds the customer by the random number
                    droneToAdd.CurrentLocation.Latitude = randomCustomer.Lattitude;
                    droneToAdd.CurrentLocation.Longitude = randomCustomer.Longtitude;
                    //battery status will be a random number between the min battery to 100.
                    clossestStation = myDal.GetClossestStation(droneToAdd.CurrentLocation.Latitude, droneToAdd.CurrentLocation.Longitude, (List<IDAL.DO.Station>)myDal.CopyStationArray());
                    double minBatteryUseForAvailable = myDal.getDistanceFromLatLonInKm(clossestStation.Lattitude, clossestStation.Longitude, droneToAdd.CurrentLocation.Latitude, droneToAdd.CurrentLocation.Longitude) * availableElectricityUse;
                    droneToAdd.Battery = getRandomDoubleNumber(minBatteryUseForAvailable, 101);
                }

                drones.Add(droneToAdd);
            }
            //a method for finding the battery use by the parcel weight.
            double batteryByWeight(Weight maxWeight)
            {
                if (maxWeight == Weight.Light)
                    return lightElectricityUse;
                if (maxWeight == Weight.Medium)
                    return standardElectricityUse;
                return heavyElectricityUse;
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
        }






    }
}
