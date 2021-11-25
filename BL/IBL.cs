using System.Collections.Generic;
using IBL.BO;

namespace BL
{
    public interface IBL
    {
        void AddCustomer(Customer customer);
        void AddDrone(DroneForList d, int stationId);
        int AddParcel(Parcel parcel);
        void AddStation(Station s);
        void AttributingParcelToDrone(int droneId);
        void Delivered(int droneId);
        IEnumerable<StationForList> GetAvailableChargingSlotsStations();
        Customer GetCustomer(int customerId);
        IEnumerable<CustomerForList> GetCustomerList();
        Drone GetDrone(int droneID);
        IEnumerable<DroneForList> GetDroneList();
        Parcel GetParcel(int parcelId);
        IEnumerable<ParcelForList> GetParcelList();
        Station GetStation(int stationId);
        IEnumerable<StationForList> GetStationList();
        IEnumerable<ParcelForList> GetUnAtributtedParcels();
        void pickedUp(int droneId);
        void ReleasedroneFromeChargeSlot(Drone d, int timeInCharge);
        void SendDroneToChargeSlot(Drone d);
        public void UpdateCustomer(int customerId, string customerName, string customerPhone);
        public void UpdateDrone(int droneId, string newModel);
        public void UpdateStation(int stationId, string stationName, int numOfChargingSlots);
    }
}