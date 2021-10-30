using System;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {
        static DalObject.DalObject data;

        static void Main(string[] args)
        {
            DalObject.DalObject data = new DalObject.DalObject();
            MenuOptions menuOption;
            EntitiesOptions entitiesOptions;
            ArrayPresentationOptions arrayOption;
            UpdateEntitiesOptions updateEntitiesOption;
            Console.WriteLine("Welcome!" );
            do
            {
                Console.WriteLine("Choose an option: \n1: Add, 2:Update, 3: Object presentation, 4: Array presentation, 5:Exit");
                menuOption = (MenuOptions)int.Parse(Console.ReadLine());
                switch (menuOption)
                {
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
                                        bool flag=long.TryParse(Console.ReadLine(), out lattitude);
                                        Station s = new Station();
                                        s = createObjectStation(ID, name, numOfSlots,longitude,lattitude);
                                        DalObject.DalObject.AddStation(s);
                                        break;
                                    }
                                case EntitiesOptions.Drone:
                                    {
                                        string model;
                                        int ID;
                                        WeightCategories maxWeight;
                                        double battery;
                                        Console.WriteLine("Enter drone's ID:" );
                                        int.TryParse(Console.ReadLine(), out ID);
                                        Console.WriteLine("Enter drone's model:");
                                        model = Console.ReadLine();
                                        Console.WriteLine("Enter drone's  maximum weight:\n1: light, 2: standard, 3: heavy:");
                                        maxWeight = (WeightCategories)int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter drone's battery:");
                                        double.TryParse(Console.ReadLine(), out battery);
                                        Drone d = new Drone();
                                        d = createObjectDrone(ID, model, maxWeight, battery);
                                        DalObject.DalObject.AddDrone(d);
                                        break;
                                    }
                                case EntitiesOptions.Customer:
                                    {
                                        string name;
                                        int ID;
                                        string phoneNumber;
                                        long longitude, lattitude;
                                        Console.WriteLine("Enter customer's ID:");
                                        int.TryParse(Console.ReadLine(), out ID);
                                        Console.WriteLine("Enter customer's name:");
                                        name = Console.ReadLine();
                                        Console.WriteLine("Enter customer's phone number:");
                                        phoneNumber= Console.ReadLine();
                                        Console.WriteLine("Enter customer's Longitude:");
                                        long.TryParse(Console.ReadLine(), out longitude);
                                        Console.WriteLine("Enter customer's Lattitude:");
                                        long.TryParse(Console.ReadLine(), out lattitude);
                                        Customer c = new Customer();
                                        c = createObjectCustomer(ID, name, phoneNumber, longitude, lattitude);
                                        DalObject.DalObject.AddCusomer(c);
                                        break;

                                    }
                                case EntitiesOptions.Parcel:
                                    {
                                        int senderID;
                                        int targetID;
                                        WeightCategories weight;
                                        Priorities priority;
                                        Console.WriteLine("Enter sender ID:");
                                        int.TryParse(Console.ReadLine(), out senderID);
                                        Console.WriteLine("Enter target ID:");
                                        int.TryParse(Console.ReadLine(), out targetID);
                                        Console.WriteLine("Enter parcel's weight:\n 0: light, 1: standard, 2: heavy ");
                                        weight = (WeightCategories)int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter parcel's priority:\n 0: normal, 1: fast, 2: emergency");
                                        priority = (Priorities)int.Parse(Console.ReadLine());
                                        Parcel p = new Parcel();
                                        p = createObjectParcel(senderID, targetID, weight, priority);
                                        int newRunningID;
                                        newRunningID= DalObject.DalObject.AddParcel(p);
                                        break;
                                    }
                            }
                            break;
                        }
                            case MenuOptions.Presentation:
                        {
                            Console.WriteLine("Choose an entity Presentation:\n1: Station, 2: Drone, 3: Customer, 4: Parcel");
                            entitiesOptions = (EntitiesOptions)int.Parse(Console.ReadLine());//foe choosing the entity
                            switch (entitiesOptions)
                            {
                                case EntitiesOptions.Station:
                                    {
                                        int stationID;
                                        Console.WriteLine("Enter the station ID:");
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out stationID);
                                        Station s= DalObject.DalObject.GetStation(stationID);
                                        Console.WriteLine(s);
                                        break;
                                    }
                                case EntitiesOptions.Drone:
                                    {
                                        int droneID;
                                        Console.WriteLine("Enter the drone ID:");
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out droneID);
                                        Drone d = DalObject.DalObject.GetDrone(droneID);
                                        Console.WriteLine(d);
                                        break;
                                    }
                                case EntitiesOptions.Customer:
                                    {
                                        int costumerID;
                                        Console.WriteLine("Enter the drone ID:");
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out costumerID);
                                        Customer c = DalObject.DalObject.GetCustomer(costumerID);
                                        Console.WriteLine(c);
                                        break;
                                    }
                                case EntitiesOptions.Parcel:
                                    {
                                        int parcelID;
                                        Console.WriteLine("Enter the parcel ID:");
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out parcelID);
                                        Parcel p = DalObject.DalObject.GetParcel(parcelID);
                                        Console.WriteLine(p);
                                        break;
                                    }
                            }
                            break;
                        }
                    case MenuOptions.ArrayPresentation:
                        {
                            Console.WriteLine("Choose list Presentation:\n1: Station, 2: Drone, 3: Customer, 4: Parcel," +
                                " 5:Non-Attributed Parcels, 6:Avalable Charge Slots. ");
                            arrayOption = (ArrayPresentationOptions)int.Parse(Console.ReadLine());//foe choosing the entity
                            switch(arrayOption)
                            {
                                case ArrayPresentationOptions.Station:
                                {
                                    printListOfStations();
                                    break;
                                }
                                case ArrayPresentationOptions.Drone:
                                    {
                                        printListOfDrones();
                                        break;
                                    }
                                case ArrayPresentationOptions.Customer:
                                    {
                                        printListOfCustomers();
                                        break;
                                    }
                                case ArrayPresentationOptions.Parcel:
                                    {
                                        printListOfParcels();
                                        break;
                                    }
                                case ArrayPresentationOptions.NonAttributedParcels:
                                    {
                                        printNonAttributedParcels();
                                        break;
                                    }
                                case ArrayPresentationOptions.AvalableChargeSlots:
                                    printAvailableStations();
                                    break;
                            }
                            break;
                        }
                    case MenuOptions.Update:
                        {
                            Console.WriteLine("Choose an update option:\n1: Attribute parcel, 2: Pick-up, 3: Delivery, 4: Charge drone, 5: Release drone");
                            updateEntitiesOption=(UpdateEntitiesOptions)int.Parse(Console.ReadLine());
                            switch(updateEntitiesOption)
                            {
                                case UpdateEntitiesOptions.attribute:
                                    {
                                        Console.WriteLine("Enter the drone's ID:"); 
                                        int droneID;
                                        int parcelID;
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out droneID);
                                        Console.WriteLine("Enter the ID of the parcel to attribute: ");
                                        input= Console.ReadLine();
                                        int.TryParse(input, out parcelID);
                                        Drone d = DalObject.DalObject.GetDrone(droneID);
                                        Parcel p = DalObject.DalObject.GetParcel(parcelID);
                                        DalObject.DalObject.AttributingParcelToDrone(p, d);
                                        break;
                                    }
                                case UpdateEntitiesOptions.PickUp:
                                    {
                                        Console.WriteLine("Enter the drone's ID:");
                                        int droneID;
                                        int parcelID;
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out droneID);
                                        Console.WriteLine("Enter the ID of the parcel to pick up: ");
                                        input = Console.ReadLine();
                                        int.TryParse(input, out parcelID);
                                        Drone d = DalObject.DalObject.GetDrone(droneID);
                                        Parcel p = DalObject.DalObject.GetParcel(parcelID);
                                        DalObject.DalObject.PickedUp(p, d);
                                        break;
                                    }
                                case UpdateEntitiesOptions.Delivery:
                                    {
                                        Console.WriteLine("Enter parcel ID: ");
                                        int parcelID;
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out parcelID);
                                        Parcel p = DalObject.DalObject.GetParcel(parcelID);
                                        DalObject.DalObject.Delivered(p);
                                        break;
                                    }
                                case UpdateEntitiesOptions.ChargeDrone:
                                    {
                                        Console.WriteLine("Enter the drone's ID:");
                                        int droneID;
                                        int stationID;
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out droneID);
                                        printAvailableStations();
                                        Console.WriteLine("Choose a station:");
                                        input = Console.ReadLine();
                                        int.TryParse(input, out stationID);
                                        Drone d = DalObject.DalObject.GetDrone(droneID);
                                        Station s = DalObject.DalObject.GetStation(stationID);
                                        DalObject.DalObject.SendDroneToChargeSlot(d, s);
                                        break;
                                    }
                                case UpdateEntitiesOptions.ReleaseDrone:
                                    {
                                        Console.WriteLine("Enter the drone's ID:");
                                        int droneID;
                                        int stationID;
                                        string input = Console.ReadLine();
                                        int.TryParse(input, out droneID);
                                        Drone d = DalObject.DalObject.GetDrone(droneID);
                                        Console.WriteLine("Enter the station's ID:");
                                        input = Console.ReadLine();
                                        int.TryParse(input, out stationID);
                                        Station s = DalObject.DalObject.GetStation(stationID);
                                        DroneCharge dc = DalObject.DalObject.GetDroneCharge(stationID, droneID);
                                        DalObject.DalObject.ReleaseDrone(d, s, dc);
                                        break;
                                    }
                            }
                            break;
                        }

                    case MenuOptions.Exit:
                        break;
                }
            }
            while (menuOption!=MenuOptions.Exit);
            /// <summary>
            /// creates a station object and updates it's data with random and user's input
            /// </summary>
            /// <param Name="name"></param>
            /// <param Name="num"></param>
            /// <returns></returns the new station>
            static Station createObjectStation(int myID, string name, int numOfSlots, long myLongitude, long myLattitude)
            {
                Random rand = new Random();
                Station s = new Station
                {
                    ID = myID,
                    StationName = name,
                    ChargeSlots = numOfSlots,
                    Lattitude = myLattitude,
                    Longitude = myLongitude
                };
                return s;

            }

            /// <summary>
            /// creates a Drone object and updates it's data with random and user's input
            /// </summary>
            /// <param Name="myModel"></param>
            /// <returns></returns the new drone>
            static Drone createObjectDrone(int myID, string myModel, WeightCategories myMaxWeight, double myBattery)
            {
                Drone d = new Drone
                {
                    ID = myID,
                    Model = myModel,
                    MaxWeight = myMaxWeight,
                    Status = 0,
                    Battery = myBattery
                };
                return d;
            }

            /// <summary>
            /// creates a customer object and updates it's data with random and user's input
            /// </summary>
            /// <param Name="myName"></param>
            /// <returns></returns the new customer>
            static Customer createObjectCustomer(int myID, string myName, string myPhoneNumber, long myLongitude, long myLattitude)
            {
                Customer c = new Customer
                {
                    ID = myID,
                    Name = myName,
                    PhoneNumber = myPhoneNumber,
                    Lattitude = myLongitude,
                    Longtitude = myLattitude,
                };
                return c;
            }

            static Parcel createObjectParcel(int mySenderID, int myTargetID, WeightCategories myWeight, Priorities myPriority)
            {
                Parcel p = new Parcel
                {
                    SenderID = mySenderID,
                    TargetID = myTargetID,
                    Weight = myWeight,
                    Priority = myPriority,
                    Requested = DateTime.Today,//the parcel has been ready today
                    DroneID = 0//no drone has been costumed yet
                };
                return p;
            }

            /// <summary>
            /// prints the list of customers
            /// </summary>
            static void printListOfCustomers()
            {
                Customer[] temp = DalObject.DalObject.CopyCustomerArray();
                for(int i=0;i<temp.Length;i++)
                    if(temp[i].ID>0)
                        Console.WriteLine(temp[i]);
            }
            /// <summary>
            /// prints the list of parcels
            /// </summary>
            static void printListOfParcels()
            {
                Parcel[] temp = DalObject.DalObject.CopyParcelArray();
                for (int i = 0; i < temp.Length; i++)
                    if (temp[i].ID > 0)
                        Console.WriteLine(temp[i]);
            }

            /// <summary>
            /// prints the list of stations
            /// </summary>
            static void printListOfStations()
            {
                Station[] temp = DalObject.DalObject.CopyStationArray();
                for (int i = 0; i < temp.Length; i++)
                    if (temp[i].ID > 0)
                        Console.WriteLine(temp[i]);
            }
            /// <summary>
            /// prints the list of drones
            /// </summary>
            static void printListOfDrones()
            {
                Drone[] temp = DalObject.DalObject.CopyDroneArray();
                for (int i = 0; i < temp.Length; i++)
                    if (temp[i].ID > 0)
                        Console.WriteLine(temp[i]);
            }

            /// <summary>
            /// prints the list of Non Attributed Parcels
            /// </summary>
            static void printNonAttributedParcels()
            {
                foreach (Parcel p in DalObject.DalObject.FindNotAttributedParcels())
                    if (p.DroneID != 0)
                        Console.WriteLine(p);
            }

            static void printAvailableStations()
            {
                Station[] temp = DalObject.DalObject.FindAvailableStations();
                for (int i=0;i<temp.Length;i++)//prints the stations with available charging slots.
                {
                    if (temp[i].ChargeSlots > 0)
                    {
                        Console.WriteLine(temp[i]);
                    }
                }
            }
        }

    }
}



