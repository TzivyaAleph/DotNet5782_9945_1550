using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDal
    {

        /// <summary>
        /// constructor
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }

        /// <summary>
        /// gets a station and adds it to the array
        /// </summary>
        /// <param Name="s">Station to add</param>

        public void AddStation(Station s)
        {
            if (DataSource.Stations.Exists(station => station.ID == s.ID))
            {
                throw new StationException($"id {s.ID} allready exists !!");
            }
            DataSource.Stations.Add(s);
        }

        /// <addDrone>
        /// add new drone and updates 
        /// </summary>
        /// <returns></returns>
        public void AddDrone(Drone d)
        {
            if (DataSource.Drones.Exists(drone => drone.ID == d.ID))
            {
                throw new DroneException($"id {d.ID} allready exists !!");
            }
            DataSource.Drones.Add(d);
        }

        /// <summary>
        /// gets a customer and adds it to the array.
        /// </summary>
        /// <param Name="c"></param>
        /// <returns></returns>
        public void AddCustomer(Customer c)
        {
            if (DataSource.Customers.Exists(client => client.ID == c.ID))
            {
                throw new CustomerException($"id {c.ID} allready exists !!");
            }
            DataSource.Customers.Add(c);
        }

        /// <summary>
        /// gets a customer and adds it to the array
        /// </summary>
        /// <param Name="p"></param>
        /// <returns></returns>
        public int AddParcel(Parcel p)
        {
            int id = ++DataSource.Config.RunningParcelID;
            p.ID = id;
            DataSource.Parcels.Add(p);
            return id;//return the id of the new  customer.
        }

        /// <summary>
        /// recieves a parcel and a drone and attributes the parcel to the drone
        /// </summary>
        /// <param Name="p"></param>
        /// <param Name="d"></param>
        public void AttributingParcelToDrone(Parcel p, Drone d)//targil1
        {
            p.DroneID = d.ID;
            p.Requested = DateTime.Now;
            UpdateParcel(p);
        }

        public void UpdateParcel(Parcel parcel)
        {
            if (!(DataSource.Parcels.Exists(p => p.ID == parcel.ID)))
            {
                throw new ParcelException("id { p.Id}  is not valid !!");
            }
            int index = DataSource.Parcels.FindIndex(item => item.ID == parcel.ID);
            DataSource.Parcels[index] = parcel;
        }

        /// <summary>
        /// recieves a parcel and updates the parcels picked up time
        /// </summary>
        /// <param Name="p"></param>
        public void PickedUp(Parcel p, Drone d)
        {
            int index = DataSource.Parcels.FindIndex(item => item.ID == p.ID);//find the index of the parcel were searching
            if (index == -1)
            {

                throw new ParcelException("id { p.Id}  does not exist !!!");
            }
            Parcel parcel = DataSource.Parcels[index];
            parcel.DroneID = d.ID;
            parcel.PickedUp = DateTime.Today;//updates the parcels pickedUp time
            DataSource.Parcels[index] = parcel;

            int indexDrone = DataSource.Drones.FindIndex(item => item.ID == d.ID);//find the index of the parcel were searching
            if (indexDrone == -1)
            {
                throw new ParcelException($"id { p.Id}   does not exist !!!");
            }
            DataSource.Drones[indexDrone] = d;
        }

        /// <summary>
        /// recieves a drone and a station and sends the drone to a chargeSlot in that station
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        public void SendDroneToChargeSlot(Drone d, Station s)
        {
            int indexStation = DataSource.Stations.FindIndex(item => item.ID == s.ID);//find the index of the parcel were searching
            if (indexStation == -1)
            {

                throw new StationException("id { s.Id}  does not exist !!!");
            }
            Station station = DataSource.Stations[indexStation];
            station.ChargeSlots++;
            //TODO
            ///
            DataSource.Stations[indexStation] = station;

            int indexDrone = DataSource.Drones.FindIndex(item => item.ID == d.ID);//find the index of the parcel were searching
            if (indexDrone == -1)
            {
                throw new ParcelException("id { p.Id}   does not exist !!!");
            }
            DataSource.Drones[indexDrone] = d;
        }

        /// <summary>
        /// recieves a drone and a station and releses the drone from the chargeSlot
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        /// <param Name="dc"></param>
        public void ReleaseDrone(Drone d, Station s, DroneCharge dc)
        {
            int indexOfDrones = DataSource.Drones.FindIndex(item => item.ID == d.ID);//find the index of the drone were searching
            int indexOfStations = DataSource.Stations.FindIndex(item => item.ID == d.ID);//find the index of the station were searching
            DataSource.Stations[indexOfStations].ChargeSlots++;//updates the available charge slots in the current staition
            int index = DataSource.DroneCharges.FindIndex(item => (item.DroneID == dc.DroneID)&&(item.StationID==item.StationID));
            DataSource.DroneCharges[index].DroneID = 0;
            DataSource.DroneCharges[index].StationID = 0;
        }

        /// <summary>
        /// searches for the droneCharge in the array by the station Id and drone id
        /// </summary>
        /// <param Name="stationID"></param>
        /// <returns></returs the drone charge object were looking for>
        public DroneCharge GetDroneCharge(int stationID, int droneID)
        {
            DroneCharge droneChargeToReturn = new DroneCharge();
            //searches the station with the recieved id.
            foreach (DroneCharge dc in DataSource.DroneCharges)
                if (dc.StationID == stationID && dc.DroneID == droneID)
                {
                    droneChargeToReturn = dc;
                }
            return droneChargeToReturn;
        }

        /// <summary>
        /// searches for the station in the array by the Id
        /// </summary>
        /// <param Name="stationID"></param>
        /// <returns></returs the station were looking for>
        public Station GetStation(int stationID)
        {
            Station stationToReturn = new Station();
            //searches the station with the recieved id.
            foreach (Station s in DataSource.Stations)
                if (s.ID == stationID)
                {
                    stationToReturn = s;
                }
            return stationToReturn;
        }

        /// <summary>
        /// searches for the drone in the array by the Id
        /// </summary>
        /// <param Name="droneID"></param>
        /// <returns></returnsthe drone were looking for>
        public Drone GetDrone(int droneID)
        {
            Drone droneToReturn = new Drone();
            foreach (Drone d in DataSource.Drones)//searches for the drone 
                if (d.ID == droneID)
                {
                    droneToReturn = d;
                }
            return droneToReturn;
        }

        /// <summary>
        /// searches for the customer in the array by the Id
        /// </summary>
        /// <param Name="customerID"></param>
        /// <returns></returnsthe customer were looking for>
        public Customer GetCustomer(int customerID)
        {
            Customer customerToReturn = default;
            //searches the customer by the id
            if (DataSource.Customers.Exists(client => client.ID == customerID))
            {
                throw new CustomerException($"id {customerID} doesn't exist !!");
            };
            customerToReturn = DataSource.Customers.Find(c => c.ID == customerID);
            return customerToReturn;
        }

        /// <summary>
        /// searches for the parcel in the array by the Id
        /// </summary>
        /// <param Name="parcelID"></param>
        /// <returns></returns parcel were looking for>
        public Parcel GetParcel(int id)
        {
            Parcel parcelToReturn = default;
            //searches the customer by the id
            if (DataSource.Parcels.Exists(p => p.ID == id))
            {
                throw new ParcelException($"id {id} doesn't exist !!");
            };
            parcelToReturn = DataSource.Parcels.Find(c => c.ID == id);
            return parcelToReturn;
        }

        /// <summary>
        /// coppies the station array
        /// </summary>
        /// <returns></returns the coppied array>
        public IEnumerable<Station> CopyStationArray()
        {
            List<Station> newList = new List<Station>(DataSource.Stations);
            return newList;
        }

        /// <summary>
        /// coppies the drone array
        /// </summary>
        /// <returns></returns the coppied array>
        public IEnumerable<Drone> CopyDroneArray()
        {
            List<Drone> newList = new List<Drone>(DataSource.Drones);
            return newList;
        }

        /// <summary>
        /// coppies the customer array
        /// </summary>
        /// <returns></returns the coppied array>
        public IEnumerable<Customer> CopyCustomerArray()
        {
            List<Customer> newList=new List<Customer>(DataSource.Customers);
            return newList;
        }

        /// <summary>
        /// coppies the parcel array
        /// </summary>
        /// <returns></returns the coppied array>
        public IEnumerable<Parcel> CopyParcelArray()
        {
            List<Parcel> newLIst = new List<Parcel>(DataSource.Parcels);
            return newLIst;
        }

        /// <summary>
        /// searches for the non atributted parcels and coppies them into a new array.
        /// </summary>
        /// <returns></returns the new array>
        public List<Parcel> FindNotAttributedParcels()
        {
            int i = 0;//for the array's index
            List<Parcel> notAttributed =new List<Parcel>();//new list to hold non attributed parcels
            foreach (Parcel p in DataSource.Parcels)//searches for the non attributed parcels
            {
                if (p.DroneID == 0)
                {
                    notAttributed[i] = p;
                    i++;
                }
            }
            return notAttributed;
        }

        /// <summary>
        /// creates an array by searching for available charge slots in the station array.
        /// </summary>
        /// <returns></returns the new array>
        public List<Station> FindAvailableStations()
        {
            int j = 0;//for the new array's index
            List<Station> availableStations = new List<Station>();//new array to hold Available Stations
            for (int i = 0; i < DataSource.Stations.Count; i++)
                if (DataSource.Stations[i].ChargeSlots > 0)
                {
                    availableStations[j] = DataSource.Stations[i];
                    j++;
                }
            return availableStations;
        }

        /// <summary>
        /// return new list with customers who have parcel that has been delieverd.
        /// </summary>
        /// <returns>the new list</returns>
        public IEnumerable<Customer> ListOfCustomerWithUnDelieverdParcel()
        {
            List<Customer> customerWithUnDelieverdParcel = new List<Customer>();
            //add to new list the customer who has parcel that have been delieverd
            foreach (var cust in DataSource.Customers)
            {
                foreach(var par in DataSource.Parcels)
                {
                    //the parcel has been attributed to the customer and has been delieverd
                    if (par.TargetID==cust.ID&&par.Delivered < DateTime.Now)
                    {
                        customerWithUnDelieverdParcel.Add(cust);
                        break;
                    }
                }
            }
            return customerWithUnDelieverdParcel;
        }

        /// <summary>
        /// find closest station to a recieved customer
        /// </summary>
        /// <param name="customerTemp"></param>
        /// <returns>return the closeset station</returns>
       public  Station GetClossestStation(Customer customerTemp)
        {
            IDAL.DO.Station minStation = new IDAL.DO.Station();
            double minDistance = Math.Sqrt((Math.Pow(customerTemp.Lattitude - DataSource.Stations.First().Lattitude, 2) + Math.Pow(customerTemp.Longtitude - DataSource.Stations.First().Longitude, 2))); ;
            foreach (var st in DataSource.Stations)
            {
                double distance = Math.Sqrt((Math.Pow(customerTemp.Lattitude - st.Lattitude, 2) + Math.Pow(customerTemp.Longtitude - st.Longitude, 2)));
                if (minDistance > distance)
                {
                    minDistance = distance;
                    minStation = st;
                }
            }
            return minStation;
        }



        //public void AddBaseStation(BaseStation b)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddClient(Client c)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddPackage(Package p)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AddSkimmer(Quadocopter q)
        //{
        //    throw new NotImplementedException();
        //}

        //public void AssignPackageSkimmer(int idp, int idq)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<BaseStation> BaseStationFreeCharging()
        //{
        //    throw new NotImplementedException();
        //}

        //public void CollectionPackage(int idp)
        //{
        //    throw new NotImplementedException();
        //}

        //public BaseStation GetBaseStation(int IDb)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<BaseStation> GetBaseStationList()
        //{
        //    throw new NotImplementedException();
        //}

        //public Client GetClient(int IDc)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Client> GetClientList()
        //{
        //    throw new NotImplementedException();
        //}

        //public Package GetPackage(int idp)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Package> GetPackageList()
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Quadocopter> GetQuadocopterList()
        //{
        //    throw new NotImplementedException();
        //}

        //public Quadocopter GetQuadrocopter(int IDq)
        //{
        //    throw new NotImplementedException();
        //}

        //public void PackageDelivery(int idp)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Package> PackagesWithoutSkimmer()
        //{
        //    throw new NotImplementedException();
        //}

        //public void SendingSkimmerForCharging(int idq, int idBS)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SkimmerRelease(int idq, int IdBS)
        //{
        //    throw new NotImplementedException();
        //}
    }
}



