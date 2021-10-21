namespace IDAL
{
    namespace DO
    {
        public enum WeightCategories
        {
            light = 0,
            standard=1,
            heavy=2
        };

        public enum Priorities
        {
            normal = 0,
            fast = 1,
            emergency = 2
        };

        public enum DroneStatuses
        {
            available = 0,
            maintenance = 1,
            delivery = 2
        };

        public enum customersName
        {
            Dan,Tony,Caleb,Gorge,Alis,Bob,Aria,Kally,Katy,Ariana
        };

        public enum MenuOptions
        {
            Add=1, Update, Presentation, ArrayPresentation, Exit
        };

        public enum EntitiesOptions 
        {
            Station, Drone, Customer, Parcel
        };

        public enum UpdateEntitiesOptions
        {
            attribute, PickUp, Delivery, ChargeDrone, ReleaseDrone
        };

        public enum ArrayPresentationOptions
        {
            Station, Drone, Customer, Parcel, NonAttributedParcels, AvalableChargeSlots
        };


    }
}