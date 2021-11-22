

using System.Collections.Generic;

namespace IDAL.DO
{
    public interface IDal
    {
        void AddCustomer(Customer c);
        void AddDrone(Drone d);
        int AddParcel(Parcel p);
        void AddStation(Station s);
        void AttributingParcelToDrone(Parcel p, Drone d);
        IEnumerable<Customer> CopyCustomerArray();
        IEnumerable<Drone> CopyDroneArray();
        IEnumerable<Parcel> CopyParcelArray();
        IEnumerable<Station> CopyStationArray();
        IEnumerable<Station> FindAvailableStations();
        IEnumerable<Parcel> FindNotAttributedParcels();
        Customer GetCustomer(int customerID);
        Drone GetDrone(int droneID);
        DroneCharge GetDroneCharge(int stationID, int droneID);
        Parcel GetParcel(int id);
        Station GetStation(int stationID);
        void PickedUp(Parcel p, Drone d);
        void Delivered(Parcel p);
        void ReleaseDrone(Drone d, Station s, DroneCharge dc);
        void SendDroneToChargeSlot(Drone d, Station s);
        void UpdateParcel(Parcel parcel);
        public void UpdateDrone(Drone drone);
        double[] GetElectricityUse();
        IEnumerable<Customer>  ListOfCustomerWithUnDelieverdParcel();
         Station GetClossestStation(double lattitude, double longtitude, List<Station> stations);
         double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2);
         double deg2rad(double deg);
        IEnumerable<DroneCharge> GetDroneChargeList();
    }
}