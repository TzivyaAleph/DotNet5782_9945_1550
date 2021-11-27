using System;
using System.Collections.Generic;
using IBL.BO;

namespace ConsoleUI_BL
{
    class Program
    {
       
        static void Main(string[] args)
        {
            BL.IBL bO = new BL.BL();
            try
            {
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
                                            Console.WriteLine("Enter a 4 digit station ID:");
                                            while (!(int.TryParse(Console.ReadLine(), out ID)))
                                            {
                                                Console.WriteLine("Enter a 4 digit station ID:");
                                            }
                                            while (ID < 1000 || ID > 10000) 
                                            {
                                               Console.WriteLine($"id {ID} is not valid !!");
                                               Console.WriteLine("Enter a 4 digit station ID:");
                                               while (!(int.TryParse(Console.ReadLine(), out ID)))
                                                {
                                                    Console.WriteLine("Enter a 4 digit station ID:");
                                                }
                                            }
                                            Console.WriteLine("Enter station's name:");
                                            name = Console.ReadLine();
                                            Console.WriteLine("Enter number of available charging slots between 0 and 50:");
                                            int.TryParse(Console.ReadLine(), out numOfSlots);
                                            while(numOfSlots < 0|| numOfSlots > 50)
                                            {
                                                Console.WriteLine($"number of slots {numOfSlots} is not valid !!");
                                                Console.WriteLine("Enter number of available charging slots between 0 and 50:");
                                                int.TryParse(Console.ReadLine(), out numOfSlots);
                                            }
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
                                            Console.WriteLine("Drone added successfully");
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
                                updateEntitiesOption = (UpdateEntitiesOptions)int.Parse(Console.ReadLine());
                                switch (updateEntitiesOption)
                                {
                                    case UpdateEntitiesOptions.DroneUpdate:
                                        {
                                            int droneID;
                                            string model;
                                            string input = Console.ReadLine();
                                            Console.WriteLine("Enter a 4 digit drone's ID:");
                                            while (!int.TryParse(input, out droneID))
                                            {
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                            }
                                            while (droneID < 1000 || droneID > 10000)
                                            {
                                                Console.WriteLine($"id {droneID} is not valid !!");
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                                while (!int.TryParse(input, out droneID))
                                                {
                                                    Console.WriteLine("Enter a 4 digit drone's ID:");
                                                }
                                            }
                                            Console.WriteLine("Enter drone's model:");
                                            model = Console.ReadLine();
                                            while(String.IsNullOrEmpty(model))
                                            {
                                                Console.WriteLine($"Drone model {model} is not valid\n");
                                                Console.WriteLine("Enter drone's model:");
                                                model = Console.ReadLine();
                                            }
                                            bO.UpdateDrone(droneID, model);
                                            break;
                                        }
                                    case UpdateEntitiesOptions.StationUpdate:
                                        {
                                            int stationID;
                                            string stationName;
                                            int numOfChargingSlots;
                                            string input = Console.ReadLine();
                                            Console.WriteLine("Enter a 4 digit drone's ID:");
                                            while (!int.TryParse(input, out stationID))
                                            {
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                            }
                                            while (stationID < 1000 || stationID > 10000)
                                            {
                                                Console.WriteLine($"Station ID {stationID} is not valid\n");
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                                while (!int.TryParse(input, out stationID))
                                                {
                                                    Console.WriteLine("Enter a 4 digit drone's ID:");
                                                }
                                            }
                                            Console.WriteLine("Enter the station's name:");
                                            stationName = Console.ReadLine();
                                            Console.WriteLine("Enter number of charging slots:");
                                            bool flag = int.TryParse(input, out numOfChargingSlots);
                                            if (flag == false)
                                                numOfChargingSlots = -1;
                                            else
                                            {
                                                while (numOfChargingSlots < 0 || numOfChargingSlots > 50)
                                                {
                                                    Console.WriteLine($"Number of charging slots is not valid\n");
                                                    Console.WriteLine("Enter number of charging slots:");
                                                    while (!int.TryParse(input, out numOfChargingSlots))
                                                    {
                                                        Console.WriteLine("Enter number of charging slots:");
                                                    }
                                                }
                                            }
                                            bO.UpdateStation(stationID, stationName, numOfChargingSlots);
                                            break;
                                        }
                                    case UpdateEntitiesOptions.CustomerUpdate:
                                        {
                                            int customerID;
                                            string customerName;
                                            string customerPhone;
                                            string input = Console.ReadLine();
                                            Console.WriteLine("Enter a 4 digit customer's ID:");
                                            while (!int.TryParse(input, out customerID))
                                            {
                                                Console.WriteLine("Enter a 4 digit customer's ID:");
                                            }
                                            while (customerID < 100000000 || customerID > 1000000000)
                                            {
                                                Console.WriteLine($"Customer ID {customerID} is not valid\n");
                                                Console.WriteLine("Enter a 4 digit customer's ID:");
                                                while (!int.TryParse(input, out customerID))
                                                {
                                                    Console.WriteLine("Enter a 4 digit customer's ID:");
                                                }
                                            }
                                            Console.WriteLine("Enter the customer's name:");
                                            customerName = Console.ReadLine();
                                            Console.WriteLine("Enter number of charging slots:");
                                            int numOfChargingSlots;
                                            bool flag = int.TryParse(input, out numOfChargingSlots);
                                            if (flag == false)
                                                numOfChargingSlots = -1;
                                            else
                                            {
                                                while (numOfChargingSlots < 0 || numOfChargingSlots > 50)
                                                {
                                                    Console.WriteLine($"Number of charging slots is not valid\n");
                                                    Console.WriteLine("Enter number of charging slots:");
                                                    while (!int.TryParse(input, out numOfChargingSlots))
                                                    {
                                                        Console.WriteLine("Enter number of charging slots:");
                                                    }
                                                }
                                            }
                                            Console.WriteLine("Enter the customer's phone number:");
                                            customerPhone = Console.ReadLine();
                                            while (customerName.Length != 10)
                                            {
                                                Console.WriteLine("Invalid phone number!!\n");
                                                Console.WriteLine("Enter the customer's phone number:");
                                                customerPhone = Console.ReadLine();
                                            }
                                            bO.UpdateCustomer(customerID, customerName, customerPhone);
                                            break;
                                        }
                                    case UpdateEntitiesOptions.ChargeDrone:
                                        {
                                            int droneID;
                                            string input = Console.ReadLine();
                                            Console.WriteLine("Enter a 4 digit drone's ID:");
                                            while (!int.TryParse(input, out droneID))
                                            {
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                            }
                                            while (droneID < 1000 || droneID > 10000)
                                            {
                                                Console.WriteLine($"id {droneID} is not valid !!");
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                                while (!int.TryParse(input, out droneID))
                                                {
                                                    Console.WriteLine("Enter a 4 digit drone's ID:");
                                                }
                                            }
                                            Drone d = bO.GetDrone(droneID);
                                            bO.SendDroneToChargeSlot(d);
                                            break;
                                        }
                                    case UpdateEntitiesOptions.ReleaseDrone:
                                        {
                                            int droneID;
                                            int amountOfHours;
                                            string input = Console.ReadLine(); ;
                                            Console.WriteLine("Enter a 4 digit drone's ID:");
                                            while (!int.TryParse(input, out droneID))
                                            {
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                            }
                                            while (droneID < 1000 || droneID > 10000)
                                            {
                                                Console.WriteLine($"id {droneID} is not valid !!");
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                                while (!int.TryParse(input, out droneID))
                                                {
                                                    Console.WriteLine("Enter a 4 digit drone's ID:");
                                                }
                                            }
                                            Console.WriteLine("Enter the amount of hour the drone was charging:");
                                            input = Console.ReadLine();
                                            while (!int.TryParse(input, out amountOfHours))
                                            {
                                                Console.WriteLine("Enter the amount of hour the drone was charging:");
                                            }
                                            while (amountOfHours<0)
                                            {
                                                Console.WriteLine($"Invalid hours");
                                                Console.WriteLine("Enter the amount of hour the drone was charging:");
                                                while (!int.TryParse(input, out droneID))
                                                {
                                                    Console.WriteLine("Enter the amount of hour the drone was charging:");
                                                }
                                            }
                                            Drone d = bO.GetDrone(droneID);
                                            bO.ReleasedroneFromeChargeSlot(d, amountOfHours);
                                            break;
                                        }
                                    case UpdateEntitiesOptions.attribute:
                                        {
                                            int droneID;
                                            string input = Console.ReadLine();
                                            Console.WriteLine("Enter a 4 digit drone's ID:");
                                            while (!int.TryParse(input, out droneID))
                                            {
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                            }
                                            while (droneID < 1000 || droneID > 10000)
                                            {
                                                Console.WriteLine($"id {droneID} is not valid !!");
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                                while (!int.TryParse(input, out droneID))
                                                {
                                                    Console.WriteLine("Enter a 4 digit drone's ID:");
                                                }
                                            }
                                            bO.AttributingParcelToDrone(droneID);
                                            break;
                                        }
                                    case UpdateEntitiesOptions.PickUp:
                                        {
                                            int droneID;
                                            string input = Console.ReadLine();
                                            Console.WriteLine("Enter a 4 digit drone's ID:");
                                            while (!int.TryParse(input, out droneID))
                                            {
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                            }
                                            while (droneID < 1000 || droneID > 10000)
                                            {
                                                Console.WriteLine($"id {droneID} is not valid !!");
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                                while (!int.TryParse(input, out droneID))
                                                {
                                                    Console.WriteLine("Enter a 4 digit drone's ID:");
                                                }
                                            }
                                            bO.pickedUp(droneID);
                                            break;
                                        }
                                    case UpdateEntitiesOptions.Delivery:
                                        {
                                            int droneID;
                                            string input = Console.ReadLine();
                                            Console.WriteLine("Enter a 4 digit drone's ID:");
                                            while (!int.TryParse(input, out droneID))
                                            {
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                            }
                                            while (droneID < 1000 || droneID > 10000)
                                            {
                                                Console.WriteLine($"id {droneID} is not valid !!");
                                                Console.WriteLine("Enter a 4 digit drone's ID:");
                                                while (!int.TryParse(input, out droneID))
                                                {
                                                    Console.WriteLine("Enter a 4 digit drone's ID:");
                                                }
                                            }
                                            bO.Delivered(droneID);
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
                                            Station s = bO.GetStation(stationID);
                                            Console.WriteLine(s);
                                            break;
                                        }
                                    case EntitiesOptions.Drone:
                                        {
                                            int droneID;
                                            Console.WriteLine("Enter the drone ID:");
                                            string input = Console.ReadLine();
                                            int.TryParse(input, out droneID);
                                            Drone d = bO.GetDrone(droneID);
                                            Console.WriteLine(d);
                                            break;
                                        }
                                    case EntitiesOptions.Customer:
                                        {
                                            int costumerID;
                                            Console.WriteLine("Enter the costumer's ID:");
                                            string input = Console.ReadLine();
                                            int.TryParse(input, out costumerID);
                                            Customer c = bO.GetCustomer(costumerID);
                                            Console.WriteLine(c);
                                            break;
                                        }
                                    case EntitiesOptions.Parcel:
                                        {
                                            int parcelID;
                                            Console.WriteLine("Enter the parcel ID:");
                                            string input = Console.ReadLine();
                                            int.TryParse(input, out parcelID);
                                            Parcel p = bO.GetParcel(parcelID);
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
                                switch (arrayOption)
                                {
                                    case ArrayPresentationOptions.Station:
                                        {
                                            printListOfStations(bO);
                                            break;
                                        }
                                    case ArrayPresentationOptions.Drone:
                                        {
                                            printListOfDrones(bO);
                                            break;
                                        }
                                    case ArrayPresentationOptions.Customer:
                                        {
                                            printListOfCustomers(bO);
                                            break;
                                        }
                                    case ArrayPresentationOptions.Parcel:
                                        {
                                            printListOfParcels(bO);
                                            break;
                                        }
                                    case ArrayPresentationOptions.NonAttributedParcels:
                                        {
                                            printNonAttributedParcels(bO);
                                            break;
                                        }
                                    case ArrayPresentationOptions.AvalableChargeSlots:
                                        printAvailableStations(bO);
                                        break;
                                }
                                break;
                            }
                        case MenuOptions.Exit:
                            break;
                    }
                }
                while (menuOption != MenuOptions.Exit);
            }
            catch(InvalidInputException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FailedToAddException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FailedToGetException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InputDoesNotExist ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FailedToUpdateException ex)
            {
                Console.WriteLine(ex.Message);
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
            DroneForList d = new DroneForList
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
                Sender = sender,
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

        /// <summary>
        /// prints the list of stations
        /// </summary>
        static void printListOfStations(BL.IBL bO)
        {
            IEnumerable<StationForList> newList = new List<StationForList>();
            newList = bO.GetStationList();
            foreach (var s in newList)
            {
                if (s.Id > 0)
                    Console.WriteLine(s);
            }
        }

        /// <summary>
        /// prints the list of drones
        /// </summary>
        static void printListOfDrones(BL.IBL bO)
        {
            IEnumerable<DroneForList> newList = new List<DroneForList>();
            newList = bO.GetDroneList();
            foreach (var d in newList)
            {
                if (d.Id > 0)
                    Console.WriteLine(d);
            }
        }

        /// <summary>
        /// prints the list of customers
        /// </summary>
        static void printListOfCustomers(BL.IBL bO)
        {
            IEnumerable<CustomerForList> newList = new List<CustomerForList>();
            newList = bO.GetCustomerList();
            foreach (var c in newList)
            {
                if (c.Id > 0)
                    Console.WriteLine(c);
            }
        }

        /// <summary>
        /// prints the list of parcels
        /// </summary>
        static void printListOfParcels(BL.IBL bO)
        {
            IEnumerable<ParcelForList> newList = new List<ParcelForList>();
            newList = bO.GetParcelList();
            foreach (var p in newList)
            {
                if (p.Id > 0)
                    Console.WriteLine(p);
            }
        }

        /// <summary>
        /// prints the list of Non Attributed Parcels
        /// </summary>
        static void printNonAttributedParcels(BL.IBL bO)
        {
            IEnumerable<ParcelForList> newList = new List<ParcelForList>();
            newList = bO.GetUnAtributtedParcels();
            foreach (var p in newList)
                Console.WriteLine(p);
        }

        /// <summary>
        /// prints a list with all the available stations in it
        /// </summary>
        static void printAvailableStations(BL.IBL bO)
        {
            IEnumerable<StationForList> temp = new List<StationForList>();
            temp = bO.GetAvailableChargingSlotsStations();
            foreach (var s in temp)
                Console.WriteLine(s);

        }
    }
}