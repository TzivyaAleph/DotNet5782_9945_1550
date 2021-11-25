using System.Collections.Generic;
using IBL.BO;

namespace BL
{
    public interface IBL
    {
        /// <summary>
        /// adds customer to customers list
        /// </summary>
        /// <param name="customer"></param>
        void AddCustomer(Customer customer);
        /// <summary>
        /// adds a new drone to the drones' list
        /// </summary>
        /// <param name="d">drone to add</param>
        /// <param name="stationId">station number to put the drone in</param>
        void AddDrone(DroneForList d, int stationId);
        /// <summary>
        /// adds parcel to parcel list
        /// </summary>
        /// <param name="parcel"></param>
        int AddParcel(Parcel parcel);
        /// <summary>
        /// adds a new station to the station list
        /// </summary>
        /// <param name="myID">station id</param>
        /// <param name="name">station name</param>
        /// <param name="numOfSlots">number of available charge slots in station</param>
        /// <param name="location">station's location</param>
        void AddStation(Station s);
        /// <summary>
        /// recieves a drone ID and attributes a parcel to the drone
        /// </summary>
        /// <param name="droneId">id of the drone we are attributing to the parcel</param>
        void AttributingParcelToDrone(int droneId);
        /// <summary>
        /// updates a parcel to be delivered and the drone that delivered the parcel to be available 
        /// </summary>
        /// <param name="droneId">id of the drone who picked up the parcel</param>
        void Delivered(int droneId);
        /// <summary>
        /// creates list with all the station that has available charging slots.
        /// </summary>
        /// <returns></returns>
        IEnumerable<StationForList> GetAvailableChargingSlotsStations();
        /// <summary>
        /// gets a customer by its id and from dal data.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>the customer</returns>
        Customer GetCustomer(int customerId);
        /// <summary>
        /// creates new list with data frome the customer list and parcels from dal
        /// </summary>
        /// <returns>the created list</returns>
        IEnumerable<CustomerForList> GetCustomerList();
        /// <summary>
        /// return drone in the list by its recieved id.
        /// </summary>
        /// <param name="droneID">for finding the drone</param>
        /// <returns>the drone from the list</returns>
        Drone GetDrone(int droneID);
        /// <summary>
        /// creates new list and copies all the fields from the drone list in bl
        /// </summary>
        /// <returns>the created list</returns>
        IEnumerable<DroneForList> GetDroneList();
        /// <summary>
        /// return parcel in the list by its recieved id.
        /// </summary>
        /// <param name="parcelId">for finding the parcel</param>
        /// <returns>the parcel from the list</returns>
        Parcel GetParcel(int parcelId);
        /// <summary>
        /// creates new list of parcels with data in fields.
        /// </summary>
        /// <returns>the created parcel</returns>
        IEnumerable<ParcelForList> GetParcelList();
        /// <summary>
        /// gets a station by the id and update the other fields by the fields in dal
        /// </summary>
        /// <param name="stationId">for getting the object</param>
        /// <returns>the object</returns>
        Station GetStation(int stationId);
        /// <summary>
        /// createse list of stations and update each fields by dal data.
        /// </summary>
        /// <returns>the creates list</returns>
        IEnumerable<StationForList> GetStationList();
        /// <summary>
        /// creates list of parcels who  does not attribute to drone
        /// </summary>
        /// <returns>the created list</returns>
        IEnumerable<ParcelForList> GetUnAtributtedParcels();
        /// <summary>
        /// finds the drone thats attributed to a parcel and updates him and the parcel to be picked up
        /// </summary>
        /// <param name="droneId">id of the pickup drone</param>
        void pickedUp(int droneId);
        /// <summary>
        /// release drone from charge slot by update the fields
        /// </summary>
        /// <param name="d">the dron to release</param>
        /// <param name="timeInCharge">for the hour its been charging</param>
        void ReleasedroneFromeChargeSlot(Drone d, int timeInCharge);
        /// <summary>
        /// send drone to chargh slots by updating fields.
        /// </summary>
        /// <param name="d"></param>
        void SendDroneToChargeSlot(Drone d);
        /// <summary>
        /// updates a customer's id, phone number and name
        /// </summary>
        /// <param name="customer">customer to update</param>
        public void UpdateCustomer(int customerId, string customerName, string customerPhone);
        /// <summary>
        /// updates a drones name
        /// </summary>
        /// <param name="drone">the drone to update</param>
        public void UpdateDrone(int droneId, string newModel);
        /// <summary>
        /// updates a stations name and number of available charging slots
        /// </summary>
        /// <param name="stationToUpdate">station with the values to update</param>
        public void UpdateStation(int stationId, string stationName, int numOfChargingSlots);
    }
}