﻿

namespace IDAL.DO
{
    public interface IDal
    {
        void AddCustomer(Customer c);
        void AddDrone(Drone d);
        int AddParcel(Parcel p);
        void AddStation(Station s);
        void AttributingParcelToDrone(Parcel p, Drone d);
        Customer[] CopyCustomerArray();
        Drone[] CopyDroneArray();
        Parcel[] CopyParcelArray();
        Station[] CopyStationArray();
        Station[] FindAvailableStations();
        Parcel[] FindNotAttributedParcels();
        Customer GetCustomer(int customerID);
        Drone GetDrone(int droneID);
        DroneCharge GetDroneCharge(int stationID, int droneID);
        Parcel GetParcel(int id);
        Station GetStation(int stationID);
        void PickedUp(Parcel p, Drone d);
        void ReleaseDrone(Drone d, Station s, DroneCharge dc);
        void SendDroneToChargeSlot(Drone d, Station s);
        void UpdateParcel(Parcel parcel);
    }
}