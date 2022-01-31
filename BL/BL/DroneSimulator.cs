using System;
using BO;
using System.Threading;
using static BL.BL;
using System.Linq;
using static System.Math;
using System.Collections.Generic;

namespace BL
{
    internal class Simulator
    {
        BL bl;

        private const double kmh = 3600;//כל קילומטר זה שנייה כי בשעה יש 3600 שניות
        private const int delay = 1000;

        public Simulator(BL _bl, int droneID, Action ReportProgressInSimultor, Func<bool> IsTimeRun)
        {
            bl = _bl;
            double distance;
            double battery;

            DroneForList drone = bl.GetDroneList().First(x => x.Id == droneID);
            Drone blDrone;
            bool thereAreParcels = bl.ThereAreParcelsToAttribute(droneID);
            bool enoughBattery = bl.enoughBatteryForCharging(drone);

            while (!IsTimeRun() && drone.Battery !=100 && enoughBattery || thereAreParcels)
            {
                switch (drone.DroneStatuses)
                {
                    case DroneStatuses.Available:
                        thereAreParcels = bl.ThereAreParcelsToAttribute(droneID);
                        if (!thereAreParcels)
                        {
                            enoughBattery = bl.enoughBatteryForCharging(drone);
                            if (drone.Battery < 100 && enoughBattery)
                            {
                                battery = drone.Battery;
                                List<DO.Station> stations1 = myDal.CopyStationArray().ToList();
                                DO.Station baseStation = myDal.GetClossestStation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, stations1);
                                //gets the distance from the drone to the closest station
                                distance = myDal.getDistanceFromLatLonInKm(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, baseStation.Lattitude, baseStation.Longitude);
                                while (distance > 0)
                                {
                                    drone.Battery -= myDal.GetElectricityUse()[0];//the drone is available
                                    ReportProgressInSimultor();
                                    distance -= 1;
                                    Thread.Sleep(delay);
                                }
                                blDrone = bl.GetDrone(drone.Id);
                                drone.Battery = battery;//restarting the battery
                                bl.SendDroneToChargeSlot(blDrone);//here it will change it to the correct battery
                                ReportProgressInSimultor();
                            }
                        }
                        else
                        {
                            bl.AttributingParcelToDrone(droneID);
                            ReportProgressInSimultor();
                        }

                        break;

                    case DroneStatuses.Maintenance:
                        blDrone = bl.GetDrone(drone.Id);
                        List<DO.Station> stations = myDal.CopyStationArray().ToList();
                        DO.Station station = stations.FirstOrDefault(item => item.Lattitude == blDrone.CurrentLocation.Latitude && item.Longitude == blDrone.CurrentLocation.Longitude);
                        DO.DroneCharge droneCharge = myDal.GetDroneCharge(station.Id, droneID);
                        TimeSpan timeCharge = (TimeSpan)(DateTime.Now - droneCharge.SentToCharge);
                        double hoursnInCahrge = timeCharge.Hours + (((double)timeCharge.Minutes) / 60) + (((double)timeCharge.Seconds) / 3600);
                        double batrryCharge = (timeCharge.TotalHours * myDal.GetElectricityUse()[4]) + drone.Battery; //DroneLoadingRate == 10000

                        while (drone.Battery < 100)
                        {
                            if (drone.Battery + 10 > 100)
                            {
                                bl.GetDroneList().First(x => x.Id == droneID).Battery = 100;
                            }
                            else
                            {
                                bl.GetDroneList().First(x => x.Id == droneID).Battery += 10;
                            }
                            ReportProgressInSimultor();
                            Thread.Sleep(delay);
                        }
                        blDrone = bl.GetDrone(drone.Id);
                        bl.ReleasedroneFromeChargeSlot(blDrone);//release drone from charging when battery is full 
                        ReportProgressInSimultor();
                        break;
                    case DroneStatuses.Delivered:
                        blDrone = bl.GetDrone(drone.Id);
                        //checks if the parcel has been picked up
                        if (bl.GetParcel(blDrone.ParcelInDelivery.Id).PickedUp == null)
                        {
                            battery = drone.Battery;
                            Location location = new Location { Longitude = drone.CurrentLocation.Longitude, Latitude = drone.CurrentLocation.Latitude };
                            distance = myDal.getDistanceFromLatLonInKm
                                (blDrone.CurrentLocation.Latitude, blDrone.CurrentLocation.Longitude, blDrone.ParcelInDelivery.Collection.Latitude, blDrone.ParcelInDelivery.Collection.Longitude);
                            //distance = blDrone.ParcelInDelivery.Transportation;
                            while (distance > 1)
                            {
                                drone.Battery -= myDal.GetElectricityUse()[0];//the drone is available
                                distance -= 1;
                                UpdateLocationDrone(bl.GetCustomer(blDrone.ParcelInDelivery.CustomerSender.Id).Location, blDrone);
                                drone.CurrentLocation = blDrone.CurrentLocation;
                                bl.GetDroneList().First(item => item.Id == drone.Id).CurrentLocation = drone.CurrentLocation;
                                ReportProgressInSimultor();
                                Thread.Sleep(delay);
                            }
                            drone.CurrentLocation = location;//restart the location
                            drone.Battery = battery;
                            bl.pickedUp(blDrone.Id);
                            ReportProgressInSimultor();
                        }
                        else // PickedUp != null
                        {
                            battery = drone.Battery;
                            DO.Customer parcelReciever = new DO.Customer();
                            IEnumerable<DO.Customer> customers = myDal.CopyCustomerArray();//gets the customers list
                            parcelReciever = customers.FirstOrDefault(customer => customer.Id == blDrone.ParcelInDelivery.CustomerReciever.Id);//finds the parcels target customer (for finding the parcels location)
                            distance = myDal.getDistanceFromLatLonInKm(blDrone.CurrentLocation.Latitude, blDrone.CurrentLocation.Longitude, parcelReciever.Lattitude, parcelReciever.Longtitude);//the distance between the sender and the resever
                            while (distance > 1)
                            {
                                switch (blDrone.ParcelInDelivery.Weight)
                                {
                                    case Weight.Light:
                                        drone.Battery -= myDal.GetElectricityUse()[1];//light
                                        break;
                                    case Weight.Medium:
                                        drone.Battery -= myDal.GetElectricityUse()[2];//medium
                                        break;
                                    case Weight.Heavy:
                                        drone.Battery -= myDal.GetElectricityUse()[3];//heavy
                                        break;
                                    default:
                                        break;
                                }
                                ReportProgressInSimultor();
                                distance -= 100;
                                Thread.Sleep(delay);
                            }
                            drone.Battery = battery;//restart battery
                            bl.Delivered(blDrone.Id);
                            ReportProgressInSimultor();
                        }
                        break;
                    default:
                        break;
                }
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        ///updates the location of the drone 
        /// </summary>
        /// <param name="targetLocation">where the drone is heading to</param>
        /// <param name="drone">the drone we want to update</param>
        private void UpdateLocationDrone(Location targetLocation, Drone drone)
        {
            double droneLatitude = drone.CurrentLocation.Latitude;
            double droneLongitude = drone.CurrentLocation.Longitude;

            double targetLocationLatitude = targetLocation.Latitude;
            double targetLocationLongitude = targetLocation.Longitude;

            double transportDistance = drone.ParcelInDelivery.Transportation;
            if (droneLatitude < targetLocationLatitude)
            {
                double step = (targetLocationLatitude - droneLatitude) / transportDistance;
                drone.CurrentLocation.Latitude += step;
            }
            else
            {
                double step = (droneLatitude - targetLocationLatitude) / transportDistance;
                drone.CurrentLocation.Latitude -= step;
            }

            if (droneLongitude < targetLocationLongitude)
            {
                double step = (targetLocationLongitude - droneLongitude) / transportDistance;
                drone.CurrentLocation.Longitude += step;
            }
            else
            {
                double step = (droneLongitude - targetLocationLongitude) / transportDistance;
                drone.CurrentLocation.Longitude -= step;
            }
        }
    }
}



