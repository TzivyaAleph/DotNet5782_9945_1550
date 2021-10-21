﻿using System;
using IDAL.DO;

namespace ConsoleUI
{
    class main
    {
        static void Main(string[] args)
        {
            MenuOptions menuOption;
            EntitiesOptions entitiesOptions;
            ArrayPresentationOptions arrayOption;
            Console.WriteLine("Welcome!\n" );
            do
            {
                Console.WriteLine("Choose an option: \n 1: Add, 2:Update, 3: Object presentation, 4: Array presentation, 5:Exit\n");
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
                                        Console.WriteLine("Enter name and number of charging slots:\n");
                                        string name;
                                        int num;
                                        name = Console.ReadLine();
                                        int.TryParse(Console.ReadLine(), out num);
                                        Station s = new Station();
                                        s = CreateObjectStation(name, num);
                                        DalObject.DalObject.AddStation(s);
                                        break;
                                    }
                                case EntitiesOptions.Drone:
                                    {
                                        Console.WriteLine("Enter the drone's model:\n");
                                        string model;
                                        model = Console.ReadLine();
                                        Drone d = new Drone();
                                        d = CreateObjectDrone(model);
                                        DalObject.DalObject.AddDrone(d);
                                        break;
                                    }
                                case EntitiesOptions.Customer:
                                    {
                                        Console.WriteLine("Enter the customer's name:\n");
                                        string name;
                                        name = Console.ReadLine();
                                        Customer c = new Customer();
                                        c = CreateObjectCustomer(name);
                                        DalObject.DalObject.AddCusomer(c);
                                        break;

                                    }
                                case EntitiesOptions.Parcel:
                                    {
                                        Console.WriteLine("Enter parcel's weight:\n 0: light, 1: standard, 2: heavy\n  " +
                                            "Enter parcel's priority:\n 0: normal, 1: fast, 2: emergency\n");
                                        WeightCategories weight;
                                        Priorities priority;
                                        weight = (WeightCategories)int.Parse(Console.ReadLine());
                                        priority = (Priorities)int.Parse(Console.ReadLine());
                                        Parcel p = new Parcel();
                                        p = CreateObjectParcel(weight, priority);
                                        DalObject.DalObject.AddParcel(p);
                                        break;
                                    }
                            }
                            break;
                        }
                            case MenuOptions.Presentation:
                        {
                            Console.WriteLine("Choose an entity Presentation:\n 1: Station, 2: Drone, 3: Customer, 4: Parcel");
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
                            Console.WriteLine("Choose list Presentation:\n 1: Station, 2: Drone, 3: Customer, 4: Parcel," +
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

                            }
                            break;
                        }
                    case MenuOptions.Exit:
                        break;
                }
            }
            while (menuOption!=MenuOptions.Exit);
            
        }
        /// <summary>
        /// prints the list of stations
        /// </summary>
        private static void printListOfStations()
        {
            foreach (var item in DalObject.DalObject.CopyStationArray())
                Console.WriteLine(item);
        }
        /// <summary>
        /// prints the list of drones
        /// </summary>
        private static void printListOfDrones()
        {
            foreach (var item in DalObject.DalObject.CopyDroneArray())
                Console.WriteLine(item);
        }
        /// <summary>
        /// prints the list of customers
        /// </summary>
        private static void printListOfCustomers()
        {
            foreach (var item in DalObject.DalObject.CopyCustomerArray())
                Console.WriteLine(item);
        }
        /// <summary>
        /// prints the list of parcels
        /// </summary>
        private static void printListOfParcels()
        {
            foreach (var item in DalObject.DalObject.CopyParcelArray())
                Console.WriteLine(item);
        }


        /// <summary>
        /// creates a station object and updates it's data with random and user's input
        /// </summary>
        /// <param Name="name"></param>
        /// <param Name="num"></param>
        /// <returns></returns the new station>
        private static Station CreateObjectStation(string name, int num)
        {
            Random rand = new Random();
            Station s = new Station
            {
                ID = rand.Next(1000, 10000),
                stationName= name,
                chargeSlots= num,
                Lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                Longitude = (long)getRandomDoubleNumber(-5000, 5000)
            };
            return s;
            
        }

        /// <summary>
        /// creates a Drone object and updates it's data with random and user's input
        /// </summary>
        /// <param Name="myModel"></param>
        /// <returns></returns the new drone>
        private static Drone createObjectDrone(string myModel)
        {
            Random rand = new Random();
            Drone d = new Drone
            {
                ID = rand.Next(1000, 10000),
                model = myModel,
                maxWeight = (WeightCategories)rand.Next(3),
                status = (DroneStatuses)rand.Next(3),
                battery = getRandomDoubleNumber(0, 100)
            };
            return d;
        }

        /// <summary>
        /// creates a customer object and updates it's data with random and user's input
        /// </summary>
        /// <param Name="myName"></param>
        /// <returns></returns the new customer>
        private static Customer createObjectCustomer(string myName)
        {
            Random rand = new Random();
            Customer c = new Customer
            {
                ID = rand.Next(100000000, 1000000000),
                Name = myName,
                PhoneNumber = $"0{rand.Next(50, 60)}-{rand.Next(1000000, 10000000)}",//random numbers according to the israeli number
                Lattitude = (long)getRandomDoubleNumber(-5000, 5000),
                Longtitude = (long)getRandomDoubleNumber(-5000, 5000),
            };
            return c;
        }

        //private static Customer createObjectParcel(WeightCategories myWeight, Priorities myPrioriy)
        //{
        //    Parcel p = new Parcel
        //    {
        //        senderID = da,
        //        targetID = target,
        //        weight = ParcelWeight,
        //        priority = HisPriority,
        //        requested = DateTime.Today,//the parcel has been ready today
        //        droneID = 0//no drone has been costumed yet
        //    };
        //}

        /// <summary>
        /// gets a maximum and minimum numbers and returns a random double number 
        /// </summary>
        /// <param Name="min"></param>
        /// <param Name="max"></param>
        /// <returns></returns a random double number>
        static double getRandomDoubleNumber(double min, double max)
        {
            Random rand = new Random();
            return rand.NextDouble() * (max - min) + min;//return a random double number 
        }
    }
}


