namespace IDAL
{
    namespace DO
    {
        public enum WeightCategories
        {
            Light = 0,
            standard=1,
            heavy=2
        };

        public enum Priorities
        {
            normal = 0,
            fast = 1,
            emergency = 2
        };



        public enum CustomersName
        {
            Dan,Tony,Caleb,Gorge,Alis,Bob,Aria,Kally,Katy,Ariana
        };

        public enum MenuOptions
        {
            Add=1, Update, Presentation, ArrayPresentation, Exit
        };

        public enum EntitiesOptions 
        {
            Station=1, Drone, Customer, Parcel
        };

        public enum UpdateEntitiesOptions
        {
            attribute=1, PickUp, Delivery, ChargeDrone, ReleaseDrone
        };

        public enum ArrayPresentationOptions
        {
            Station=1, Drone, Customer, Parcel, NonAttributedParcels, AvalableChargeSlots
        };


    }
}