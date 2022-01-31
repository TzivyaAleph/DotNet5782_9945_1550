using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    /// <summary>
    /// 
    /// </summary>
    sealed partial class DalXml : IDal
    {
        private const string DroneXml = @"drones.xml";
        private const string StationXml = @"stations.xml";
        private const string ParcelXml = @"parcels.xml";
        private const string CustomerXml = @"customers.xml";
        private const string DroneChargeXml = @"DroneCharge.xml";
        private const string ConfigXml = @"ConfigXml.xml";
        private const string DataDirectory = "PL\\Data\\";

        #region singleton
        //lazt<T> is doing a lazy initialzation and hi sdefualt is thread safe
        internal static readonly Lazy<DalXml> singleInstance = new Lazy<DalXml>(() => new DalXml());
        //internal static readonly DalXml singleInstance = new DalXml();
        public static DalXml Instance
        {
            get
            {
                return singleInstance.Value;
                //return singleInstance;
            }
        }

        /// <summary>
        /// private constructor
        /// </summary>
        private DalXml()
        {
            var dirInfo = new System.IO.DirectoryInfo(DataDirectory);
            if(dirInfo.GetFiles().Count() <=1)
            {
                DataSource.Initialize();
            }
        }

        /// <summary>
        /// static ctor to ensure instance init is done just before first usage
        /// </summary>
        static DalXml() { }

        #endregion singleton

        #region AskForBattery
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetElectricityUse()
        {
            XElement configXml = XmlHelper.LoadListFromXMLElement(DataDirectory + ConfigXml);
            double[] arr = { double.Parse(configXml.Element("Available").Value),
                double.Parse(configXml.Element("Light").Value),
                double.Parse(configXml.Element("MediumWeight").Value),
                double.Parse(configXml.Element("Heavy").Value),
                double.Parse(configXml.Element("ChargingRate").Value) };
            return arr;
        }
        #endregion

        #region addingFunctions

        /// <summary>
        /// gets a customer and adds it to the list.
        /// </summary>
        /// <param name="c"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer c)
        {
            XElement customerXml = XmlHelper.LoadListFromXMLElement(DataDirectory + CustomerXml);

            XElement customer = (from cus in customerXml.Elements()
                                 where cus.Element("Id").Value == c.Id.ToString()
                                 select cus).FirstOrDefault();
            if (customer != null)
            {
                throw new ExistingObjectException($"Customer id: {c.Id} already exists.");
            }

            XElement CustomerElem = new XElement("Customer",
                                 new XElement("Id", c.Id),
                                 new XElement("Name", c.Name),
                                 new XElement("PhoneNumber", c.PhoneNumber),
                                 new XElement("Lattitude", c.Lattitude),
                                 new XElement("Longtitude", c.Longtitude),
                                 new XElement("CustomerType", c.CustomerType),
                                 new XElement("Password", c.Password),
                                 new XElement("IsDeleted", c.IsDeleted));

            customerXml.Add(CustomerElem);
            XmlHelper.SaveListToXMLElement(customerXml, DataDirectory + CustomerXml);
        }

        /// <summary>
        /// add new drone and updates 
        /// </summary>
        /// <param name="d"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(DO.Drone d)
        {
            List<Drone> drones = XmlHelper.DeserializeData<Drone>(DataDirectory + DroneXml);
            if (drones.Exists(drone => drone.Id == d.Id))
            {
                throw new ExistingObjectException($"drone {d.Id} allready exists !!");
            }
            drones.Add(d);
            XmlHelper.SerializeData(drones, DataDirectory + DroneXml);
        }


        /// <summary>
        /// gets a parcel and adds it to the list
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddParcel(Parcel p)
        {
            List<Parcel> parcels = XmlHelper.DeserializeData<Parcel>(DataDirectory + ParcelXml);
            XElement configXml = XmlHelper.LoadListFromXMLElement(DataDirectory + ConfigXml);
            int parcId = int.Parse(configXml.Element("RunningParcelId").Value);
            parcId++;
            p.Id = parcId;
            p.Requested = DateTime.Now;
            p.Delivered = null;
            p.PickedUp = null;
            p.Scheduled = null;
            p.IsDeleted = false;
            parcels.Add(p);
            XmlHelper.SerializeData(parcels, DataDirectory + ParcelXml);
            configXml.Element("RunningParcelId").Value = parcId.ToString();
            XmlHelper.SaveListToXMLElement(configXml, DataDirectory + ConfigXml);
            return p.Id;
        }

        /// <summary>
        ///  adds a station and adds it to the array
        /// </summary>
        /// <param name="s"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station s)
        {
            List<Station> stations = XmlHelper.DeserializeData<Station>(DataDirectory + StationXml);
            if (stations.Exists(station => station.Id == s.Id))
            {
                throw new ExistingObjectException($"station {s.Id} allready exists !!");
            }
            stations.Add(s);
            XmlHelper.SerializeData(stations, DataDirectory + StationXml);
        }
        #endregion

        #region ListFunctions

        /// <summary>
        /// coppies the customer list
        /// </summary>
        /// <returns></returns the coppied list>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> CopyCustomerArray(Func<Customer, bool> predicate = null)
        {
            XElement customerXml = XmlHelper.LoadListFromXMLElement(DataDirectory + CustomerXml);

            IEnumerable<Customer> customers = from cus in customerXml.Elements()
                                              select new Customer()
                                              {
                                                  Id = int.Parse(cus.Element("Id").Value),
                                                  Name = cus.Element("Name").Value,
                                                  PhoneNumber = cus.Element("PhoneNumber").Value,
                                                  Longtitude = double.Parse(cus.Element("Longtitude").Value),
                                                  Lattitude = double.Parse(cus.Element("Lattitude").Value),
                                                  IsDeleted = bool.Parse(cus.Element("IsDeleted").Value),
                                                  Password = cus.Element("Password").Value,
                                                  CustomerType = (CustomersType)Enum.Parse(typeof(CustomersType), cus.Element("CustomerType").Value)
                                              };

            if (predicate == null)
                return customers;//customers.Select(item => item);

            return customers.Where(predicate);

        }

        /// <summary>
        /// coppies the drone list
        /// </summary>
        /// <returns></returns the coppied list>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DO.Drone> CopyDroneArray(Predicate<Drone> predicate = null)
        {
            List<Drone> drones = XmlHelper.DeserializeData<Drone>(DataDirectory + DroneXml);
            return from item in drones
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        /// <summary>
        /// coppies the parcel list
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> CopyParcelArray(Func<Parcel, bool> predicate = null)
        {
            List<Parcel> parcels = XmlHelper.DeserializeData<Parcel>(DataDirectory + ParcelXml);
            return from item in parcels
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        /// <summary>
        /// coppies the station list
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> CopyStationArray(Func<Station, bool> predicate = null)
        {
            List<Station> stations = XmlHelper.DeserializeData<Station>(DataDirectory + StationXml);
            return from item in stations
                   where predicate == null ? true : predicate(item)
                   select item;
        }

        /// <summary>
        /// returns the list of drone charges
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDroneChargeList()
        {
            List<DroneCharge> newList = XmlHelper.DeserializeData<DroneCharge>(DataDirectory + DroneChargeXml);
            return newList;
        }
        #endregion

        #region getFunctions

        /// <summary>
        /// searches for the customer in the list by the Id
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerID)
        {
            XElement customersRootElem = XmlHelper.LoadListFromXMLElement(DataDirectory + CustomerXml);

            Customer c = (from per in customersRootElem.Elements()
                          where int.Parse(per.Element("Id").Value) == customerID
                          select new Customer()
                        {
                            Id = int.Parse(per.Element("Id").Value),
                            Name = per.Element("Name").Value,
                            PhoneNumber = per.Element("PhoneNumber").Value,
                            Longtitude= double.Parse(per.Element("Longtitude").Value),
                            Lattitude= double.Parse(per.Element("Lattitude").Value),
                            IsDeleted=bool.Parse(per.Element("IsDeleted").Value),
                            Password=per.Element("Password").Value,
                            CustomerType= (CustomersType)Enum.Parse(typeof(CustomersType), per.Element("CustomerType").Value)
                          }
                        ).FirstOrDefault();
            if (c.Id != 0)
            {
                return c;
            }
            else
            {
                throw new ExistingObjectException($"Customer id: { customerID } does not exists.");
            }
        }

        /// <summary>
        ///  searches for the drone in the list by the Id
        /// </summary>
        /// <param name="droneID"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DO.Drone GetDrone(int droneID)
        {
            List<Drone> drones = XmlHelper.DeserializeData<Drone>(DataDirectory + DroneXml);
            if (!(drones.Exists(d => d.Id == droneID)))
            {
                throw new UnvalidIDException("id { d.Id}  is not valid !!");
            }
            int index = drones.FindIndex(item => item.Id == droneID);
            return drones[index];
        }

        /// <summary>
        /// searches for the droneCharge in the list by the station Id and drone id
        /// </summary>
        /// <param name="stationID"></param>
        /// <param name="droneID"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int stationID, int droneID)
        {
            List<DroneCharge> droneCharges = XmlHelper.DeserializeData<DroneCharge>(DataDirectory + DroneChargeXml);
            DroneCharge droneChargeToReturn = new DroneCharge();
            foreach (var dc in
            //searches the station with the recieved id.
            from DroneCharge dc in droneCharges
            where dc.StationID == stationID && dc.DroneID == droneID
            select dc)
            {
                droneChargeToReturn = dc;
            }

            return droneChargeToReturn;
        }

        /// <summary>
        /// searches for the parcel in the list by the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int id)
        {
            Parcel parcelToReturn = default;
            List<Parcel> parcels = XmlHelper.DeserializeData<Parcel>(DataDirectory + ParcelXml);
            //searches the parcel by the id
            if (!(parcels.Exists(p => p.Id == id)))
            {
                throw new UnvalidIDException($"id {id} is not valid !!");
            };
            parcelToReturn = parcels.Find(c => c.Id == id);
            return parcelToReturn;
        }

        /// <summary>
        /// searches for the station in the list by the Id
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationID)
        {
            List<Station> stations = XmlHelper.DeserializeData<Station>(DataDirectory + StationXml);
            if (!(stations.Exists(s => s.Id == stationID)))
            {
                throw new UnvalidIDException("id { s.Id}  is not valid !!");
            }
            int index = stations.FindIndex(item => item.Id == stationID);
            return stations[index];
        }
        #endregion

        #region updateFunctions

        /// <summary>
        /// updates a parcel in the list
        /// </summary>
        /// <param name="parcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(Parcel parcel)
        {
            List<Parcel> parcels = XmlHelper.DeserializeData<Parcel>(DataDirectory + ParcelXml);
            if (!(parcels.Exists(p => p.Id == parcel.Id)))
            {
                throw new ExistingObjectException($"id { parcel.Id}  does not exist!!");
            }
            int index = parcels.FindIndex(item => item.Id == parcel.Id);
            parcels[index] = parcel;
            XmlHelper.SerializeData(parcels, DataDirectory + ParcelXml);
        }

        /// <summary>
        /// updates the drone in the list
        /// </summary>
        /// <param name="drone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(DO.Drone drone)
        {
            List<Drone> drones = XmlHelper.DeserializeData<Drone>(DataDirectory + DroneXml);
            if (!(drones.Exists(d => d.Id == drone.Id)))
            {
                throw new UnvalidIDException($"id { drone.Id}  is not valid !!");
            }
            int index = drones.FindIndex(item => item.Id == drone.Id);
            drones[index] = drone;
            XmlHelper.SerializeData(drones, DataDirectory + DroneXml);
        }

        /// <summary>
        /// puts a updated station in the stations list
        /// </summary>
        /// <param name="station"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station station)
        {
            List<Station> stations = XmlHelper.DeserializeData<Station>(DataDirectory + StationXml);
            if (!(stations.Exists(s => s.Id == station.Id)))
            {
                throw new UnvalidIDException($"id {station.Id} is not valid !!");
            }
            int index = stations.FindIndex(item => item.Id == station.Id);
            stations[index] = station;
            XmlHelper.SerializeData<Station>(stations, DataDirectory + StationXml);
        }

        /// <summary>
        /// updates a customer in the list
        /// </summary>
        /// <param name="customer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer customer)
        {
            XElement customerXml = XmlHelper.LoadListFromXMLElement(DataDirectory + CustomerXml);

            bool custFound=false;
            foreach (var cust in from cust in customerXml.Elements()
                                 let id = int.Parse(cust.Element("Id").Value)
                                 where id == customer.Id
                                 select cust)
            {
                custFound = true;
                cust.Element("Name").Value = customer.Name;
                cust.Element("PhoneNumber").Value = customer.PhoneNumber;
                cust.Element("IsDeleted").Value =customer.IsDeleted.ToString();
            }

            if (custFound==false)
                throw new UnvalidIDException($"id {customer.Id}  is not valid !!");

            XmlHelper.SaveListToXMLElement(customerXml, DataDirectory + CustomerXml);
        }
        #endregion

        /// <summary>
        /// recieves a parcel and a drone and attributes the parcel to the drone
        /// </summary>
        /// <param Name="p"></param>
        /// <param Name="d"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AttributingParcelToDrone(Parcel p, DO.Drone d)
        {
            p.DroneID = d.Id;
            p.Requested = DateTime.Now;
            UpdateParcel(p);
        }

        /// <summary>
        /// recieves a parcel and updates the parcels picked up time
        /// </summary>
        /// <param Name="p"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickedUp(Parcel p, DO.Drone d)
        {
            p.PickedUp = DateTime.Now;//updates the parcels pickedUp time
            UpdateParcel(p);
        }

        /// <summary>
        /// recieves a parcel and updates the parcels delivery time
        /// </summary>
        /// <param name="p"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Delivered(Parcel p)
        {
            p.Delivered = DateTime.Now;
            p.DroneID = 0;
            UpdateParcel(p);
        }

        /// <summary>
        /// recieves a drone and a station and releses the drone from the chargeSlot
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        /// <param Name="dc"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDrone(DO.Drone d, Station s)
        {
            s.ChargeSlots++;
            UpdateStation(s);//updates the available charge slots in the current staition
            List<DroneCharge> droneCharges = XmlHelper.DeserializeData<DroneCharge>(DataDirectory + DroneChargeXml);
            if (!(droneCharges.Exists(dc => dc.DroneID == d.Id && dc.StationID == s.Id)))
            {
                throw new UnvalidIDException("dc is not valid !!");
            }
            int index = droneCharges.FindIndex(item => item.StationID == s.Id && item.DroneID == d.Id);
            DroneCharge help = droneCharges[index];
            help.DroneID = 0;
            help.StationID = 0;
            help.SentToCharge = null;
            droneCharges[index] = help;
            XmlHelper.SerializeData<DroneCharge>(droneCharges, DataDirectory + DroneChargeXml);
        }

        /// <summary>
        /// recieves a drone and a station and sends the drone to a chargeSlot in that station
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendDroneToChargeSlot(DO.Drone d, Station s)
        {
            s.ChargeSlots--;
            UpdateStation(s);
            DroneCharge dc = new DroneCharge();
            dc.DroneID = d.Id;
            dc.StationID = s.Id;
            dc.SentToCharge = DateTime.Now;
            List<DroneCharge> droneCharges = XmlHelper.DeserializeData<DroneCharge>(DataDirectory + DroneChargeXml);
            droneCharges.Add(dc);
            XmlHelper.SerializeData<DroneCharge>(droneCharges, DataDirectory + DroneChargeXml);
        }

        /// <summary>
        /// creates list with all the available station and return the closest station in the list to the recieved drone.
        /// </summary>
        /// <param name="drone">for finding the closest </param>
        /// <param name="longtitude"></param>
        /// <param name="lattitude"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetClossestStation(double lattitude, double longtitude, List<Station> stations)
        {
            DO.Station minStation = new Station();
            double minDistance = Math.Sqrt(Math.Pow(lattitude - stations.First().Lattitude, 2) + Math.Pow(longtitude - stations.First().Longitude, 2));
            minStation = stations.First();
            List<Station> stations1 = XmlHelper.DeserializeData<Station>(DataDirectory + StationXml);
            foreach (var (st, distance) in from st in stations1
                                           let distance = Math.Sqrt(Math.Pow(lattitude - st.Lattitude, 2) + Math.Pow(longtitude - st.Longitude, 2))
                                           where minDistance > distance
                                           select (st, distance))
            {
                minDistance = distance;
                minStation = st;
            }

            return minStation;
        }

        /// <summary>
        /// calculate the distance between two points specified by latitude and longitude.
        /// https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns>the distance</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        /// <summary>
        /// converts degree to radians
        /// https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
        /// </summary>
        /// <param name="deg"></param>
        /// <returns>the radians</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }

    }
}
