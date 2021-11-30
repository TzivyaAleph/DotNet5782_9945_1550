

using System;
using System.Collections.Generic;

namespace IDAL.DO
{
    public interface IDal
    {
        /// <summary>
        /// gets a customer and adds it to the array.
        /// </summary>
        /// <param Name="c"></param>
        /// <returns></returns>
        void AddCustomer(Customer c);
        /// <addDrone>
        /// add new drone and updates 
        /// </summary>
        /// <returns></returns>
        void AddDrone(Drone d);
        /// <summary>
        /// gets a parcel and adds it to the list
        /// </summary>
        /// <param Name="p"></param>
        /// <returns></returns>
        int AddParcel(Parcel p);
        /// <summary>
        /// gets a station and adds it to the array
        /// </summary>
        /// <param Name="s">Station to add</param>
        void AddStation(Station s);
        /// <summary>
        /// recieves a parcel and a drone and attributes the parcel to the drone
        /// </summary>
        /// <param Name="p"></param>
        /// <param Name="d"></param>
        void AttributingParcelToDrone(Parcel p, Drone d);
        /// <summary>
        /// coppies the customer array
        /// </summary>
        /// <returns></returns the coppied array>
        IEnumerable<Customer> CopyCustomerArray(Func<Customer, bool> predicate = null);
        /// <summary>
        /// coppies the drone array
        /// </summary>
        /// <returns></returns the coppied array>
        IEnumerable<Drone> CopyDroneArray();
        /// <summary>
        /// coppies the parcel array
        /// </summary>
        /// <returns></returns the coppied array>
        IEnumerable<Parcel> CopyParcelArray();
        /// <summary>
        /// coppies the station array
        /// </summary>
        /// <returns></returns the coppied array>
        IEnumerable<Station> CopyStationArray(Func<Station,bool> predicate=null);
        /// <summary>
        /// creates an array by searching for available charge slots in the station list.
        /// </summary>
        /// <returns></returns the new list>
        //IEnumerable<Station> FindAvailableStations();
        /// <summary>
        /// searches for the non atributted parcels and coppies them into a new list.
        /// </summary>
        /// <returns></returns the new array>
        IEnumerable<Parcel> FindNotAttributedParcels();
        /// <summary>
        /// searches for the customer in the array by the Id
        /// </summary>
        /// <param Name="customerID"></param>
        /// <returns></returnsthe customer were looking for>
        Customer GetCustomer(int customerID);
        /// <summary>
        /// searches for the drone in the array by the Id
        /// </summary>
        /// <param Name="droneID"></param>
        /// <returns></returnsthe drone were looking for>
        Drone GetDrone(int droneID);
        /// <summary>
        /// searches for the droneCharge in the array by the station Id and drone id
        /// </summary>
        /// <param Name="stationID"></param>
        /// <returns></returs the drone charge object were looking for>
        DroneCharge GetDroneCharge(int stationID, int droneID);
        /// <summary>
        /// searches for the parcel in the array by the Id
        /// </summary>
        /// <param Name="parcelID"></param>
        /// <returns></returns parcel were looking for>
        Parcel GetParcel(int id);
        /// <summary>
        /// searches for the station in the array by the Id
        /// </summary>
        /// <param Name="stationID"></param>
        /// <returns></returs the station were looking for>
        Station GetStation(int stationID);
        /// <summary>
        /// recieves a parcel and updates the parcels picked up time
        /// </summary>
        /// <param Name="p"></param>
        void PickedUp(Parcel p, Drone d);
        /// <summary>
        /// recieves a parcel and updates the parcels delivery time
        /// </summary>
        /// <param name="p"></param>
        void Delivered(Parcel p);
        /// <summary>
        /// recieves a drone and a station and releses the drone from the chargeSlot
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        /// <param Name="dc"></param>
        void ReleaseDrone(Drone d, Station s);
        /// <summary>
        /// recieves a drone and a station and sends the drone to a chargeSlot in that station
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        void SendDroneToChargeSlot(Drone d, Station s);
        /// <summary>
        /// updates a parcel in the list
        /// </summary>
        /// <param name="parcel"></param>
        void UpdateParcel(Parcel parcel);
        /// <summary>
        /// updates the drone in the list
        /// </summary>
        /// <param name="drone"></param>
        public void UpdateDrone(Drone drone);
        /// <summary>
        /// creates a new array with the drone's electricity use  
        /// </summary>
        /// <returns>the new array</returns>
        double[] GetElectricityUse();
        /// <summary>
        /// return new list with customers who have parcel that has been delieverd.
        /// </summary>
        /// <returns>the new list</returns>
        //IEnumerable<Customer>  ListOfCustomerWithDelieverdParcel();
        /// <summary>
        /// creates list with all the available station and return the closest station in the list to the recieved drone.
        /// </summary>
        /// <param name="drone">for finding the closest </param>
        /// <param name="longtitude"></param>
        /// <param name="lattitude"></param>
        /// <returns></returns>
        Station GetClossestStation(double lattitude, double longtitude, List<Station> stations);
        /// <summary>
        /// calculate the distance between two points specified by latitude and longitude.
        /// https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns>the distance</returns>
        double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2);
        /// <summary>
        /// converts degree to radians
        /// https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
        /// </summary>
        /// <param name="deg"></param>
        /// <returns>the radians</returns>
        double deg2rad(double deg);
        /// <summary>
        /// returns the list of drone charges
        /// </summary>
        /// <returns>list of drone charges</returns>
        IEnumerable<DroneCharge> GetDroneChargeList();
        /// <summary>
        /// puts a updated station in the stations list
        /// </summary>
        /// <param name="station">updated station to add</param>
        public void UpdateStation(Station station);
        /// <summary>
        /// puts a updated customer in the customers list
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer);
    }
}