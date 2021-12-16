namespace BO
{
    public enum Weight
    {
        Light, Medium, Heavy
    }

    public enum Status
    {
        Created, Assigned, Picked, Delivered
    }

    public enum Priority
    {
        Regular, Fast, Emergency
    }

    public enum DroneStatuses
    {
        Available, Maintenance, Delivered
    };

    public enum MenuOptions
    {
        Add = 1, Update, Presentation, ArrayPresentation, Exit
    };

    public enum EntitiesOptions
    {
        Station = 1, Drone, Customer, Parcel
    };

    public enum UpdateEntitiesOptions
    {
        DroneUpdate = 1, StationUpdate, CustomerUpdate, ChargeDrone, ReleaseDrone, attribute, PickUp, Delivery
    };

    public enum ArrayPresentationOptions
    {
        Station = 1, Drone, Customer, Parcel, NonAttributedParcels, AvalableChargeSlots
    };
}
