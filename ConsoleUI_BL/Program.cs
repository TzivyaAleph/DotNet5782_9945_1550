using System;
using IBL.BO;

namespace ConsoleUI_BL
{
    class Program
    {
        static void Main(string[] args)
        {
            BL.BL bO = new BL.BL();
            MenuOptions menuOption;
            EntitiesOptions entitiesOptions;
            ArrayPresentationOptions arrayOption;
            UpdateEntitiesOptions updateEntitiesOption;
            Console.WriteLine("Welcome!");
            do
            {
                Console.WriteLine("Choose an option: \n1: Add, 2:Update, 3: Object presentation, 4: Array presentation, 5:Exit");
                menuOption = (MenuOptions)int.Parse(Console.ReadLine());
                switch (menuOption)
                    case MenuOptions.Add:
                    {
                        Console.WriteLine("Choose an entity to add:\n 1: Station, 2: Drone, 3: Customer, 4: Parcel");
                        entitiesOptions = (EntitiesOptions)int.Parse(Console.ReadLine());
                        switch (entitiesOptions)
                        {
                            case EntitiesOptions.Station:
                                {
                                    string name;
                                    int numOfSlots;
                                    long longitude, lattitude;
                                    int ID;
                                    Console.WriteLine("Enter station's ID:");
                                    int.TryParse(Console.ReadLine(), out ID);
                                    Console.WriteLine("Enter station's name:");
                                    name = Console.ReadLine();
                                    Console.WriteLine("Enter number of available charging slots:");
                                    int.TryParse(Console.ReadLine(), out numOfSlots);
                                    Console.WriteLine("Enter station's Longitude:");
                                    long.TryParse(Console.ReadLine(), out longitude);
                                    Console.WriteLine("Enter station's Lattitude:");
                                    bool flag = long.TryParse(Console.ReadLine(), out lattitude);
                                    Station s = new Station();
                                    s = createObjectStation(ID, name, numOfSlots, longitude, lattitude);
                                    bO.AddStation(s);
                                    Console.WriteLine("Station added successfully");
                                    break;
                                }
                            case EntitiesOptions.Drone:
                                {
                                    string model;
                                    int ID;
                                    int stationId;
                                    Weight maxWeight;
                                    Console.WriteLine("Enter drone's ID:");
                                    int.TryParse(Console.ReadLine(), out ID);
                                    Console.WriteLine("Enter drone's model:");
                                    model = Console.ReadLine();
                                    Console.WriteLine("Enter drone's  maximum weight:\n1: light, 2: standard, 3: heavy:");
                                    maxWeight = (Weight)int.Parse(Console.ReadLine());
                                    Console.WriteLine("Enter the Id of the station to charge the drone in:");
                                    int.TryParse(Console.ReadLine(), out stationId);
                                    DroneForList d = new DroneForList();
                                    d = createObjectDrone(ID, model, maxWeight);
                                    bO.AddDrone(d, stationId);
                                    break;
                                }
                            case EntitiesOptions.Customer:
                                {
                                    string name;
                                    int ID;
                                    string phoneNumber;
                                    double longitude, lattitude;
                                    Console.WriteLine("Enter customer's ID:");
                                    int.TryParse(Console.ReadLine(), out ID);
                                    Console.WriteLine("Enter customer's name:");
                                    name = Console.ReadLine();
                                    Console.WriteLine("Enter customer's phone number:");
                                    phoneNumber = Console.ReadLine();
                                    Console.WriteLine("Enter customer's Longitude:");
                                    double.TryParse(Console.ReadLine(), out longitude);
                                    Console.WriteLine("Enter customer's Lattitude:");
                                    double.TryParse(Console.ReadLine(), out lattitude);
                                    Customer c = new Customer();
                                    c = createObjectCustomer(ID, name, phoneNumber, longitude, lattitude);
                                    bO.AddCustomer(c);
                                    break;

                                }
                            case EntitiesOptions.Parcel:
                                {
                                    int senderID;
                                    int targetID;
                                    Weight weight;
                                    Priority priority;
                                    Console.WriteLine("Enter sender ID:");
                                    int.TryParse(Console.ReadLine(), out senderID);
                                    Console.WriteLine("Enter target ID:");
                                    int.TryParse(Console.ReadLine(), out targetID);
                                    Console.WriteLine("Enter parcel's weight:\n 0: light, 1: standard, 2: heavy ");
                                    weight = (Weight)int.Parse(Console.ReadLine());
                                    Console.WriteLine("Enter parcel's priority:\n 0: normal, 1: fast, 2: emergency");
                                    priority = (Priority)int.Parse(Console.ReadLine());
                                    Parcel p = new Parcel();
                                    p = createObjectParcel(senderID, targetID, weight, priority);
                                    int newRunningID;
                                    newRunningID = bO.AddParcel(p);
                                    break;
                                }
                        }
                        break;
                    }
                    case MenuOptions.Update:
                        {
                            Console.WriteLine("Choose an update option:\n1: Update Drone, 2: Update Station, 3: Update customer, 4: Charge drone, 5: Release drone, 6: Attribute parcel, , 7: Pick-up, 8: Delivery");
                            updateEntitiesOption=(UpdateEntitiesOptions)int.Parse(Console.ReadLine());
                            switch(updateEntitiesOption)
                            {
                            case UpdateEntitiesOptions.DroneUpdate:
                                {
                                    Console.WriteLine("Enter the drone's ID:");
                                    int droneID;
                                    string model;
                                    string input = Console.ReadLine();
                                    int.TryParse(input, out droneID);
                                    Console.WriteLine("Enter drone's model:");
                                    model = Console.ReadLine();
                                }
                               

                }
            }
            
        }
        
        /// <summary>
        /// creates a station object and updates it's data with user's input
        /// </summary>
        /// <param Name="name"></param>
        /// <param Name="num"></param>
        /// <returns></returns the new station>
        static Station createObjectStation(int myID, string name, int numOfSlots, double myLongitude, double myLattitude)
        {
            Station s = new Station
            {
                Id = myID,
                Name = name,
                ChargeSlots = numOfSlots,
                StationLocation = createLocationObject(myLongitude, myLattitude),
            };
            return s;

        }

        /// <summary>
        /// creates a Drone object and updates it's data with random and user's input
        /// </summary>
        /// <param Name="myModel"></param>
        /// <returns></returns the new drone>
        static DroneForList createObjectDrone(int myID, string myModel, Weight myMaxWeight)
        {
            Drone d = new Drone
            {
                Id = myID,
                Model = myModel,
                Weight = myMaxWeight,
            };
            return d;
        }

        /// <summary>
        /// creates a customer object and updates it's data with useres' input
        /// </summary>
        /// <param name="myID"></param>
        /// <param name="myName"></param>
        /// <param name="myPhoneNumber"></param>
        /// <param name="myLongitude"></param>
        /// <param name="myLattitude"></param>
        /// <returns>the new customer</returns>
        static Customer createObjectCustomer(int myID, string myName, string myPhoneNumber, double myLongitude, double myLattitude)
        {
            Customer c = new Customer
            {
                Id = myID,
                Name = myName,
                PhoneNumber = myPhoneNumber,
                Location = createLocationObject(myLongitude, myLattitude)
            };
            return c;
        }

        /// <summary>
        /// creates a new parcel with the data it recieves.
        /// </summary>
        /// <param name="mySenderID"></param>
        /// <param name="myTargetID"></param>
        /// <param name="myWeight"></param>
        /// <param name="myPriority"></param>
        /// <returns></returns the new parcel>
        static Parcel createObjectParcel(int mySenderID, int myTargetID, Weight myWeight, Priority myPriority)
        {
            CustomerParcel recipient = new CustomerParcel
            {
                Id = myTargetID
            };
            CustomerParcel sender = new CustomerParcel
            {
                Id = mySenderID
            };
            DroneParcel drone = default;
            Parcel p = new Parcel
            {
                Sender=sender,
                Recipient = recipient,
                Weight = myWeight,
                Priority = myPriority,
                DroneInParcel = drone//no drone has been costumed yet
            };
            return p;
        }

        /// <summary>
        /// creates a location object and returns the location with long and lat from the users input
        /// </summary>
        /// <param name="myLongitude"></param>
        /// <param name="myLattitude"></param>
        /// <returns>the location</returns>
        static Location createLocationObject(double myLongitude, double myLattitude)
        {
            Location l = new();
            l.Latitude = myLattitude;
            l.Longitude = myLongitude;
            return l;
        }
    }
}



