using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        /// gets a parcel and adds it to the list
        /// </summary>
        /// <param Name="p"></param>
        /// <returns></returns>
        public int AddParcel(Parcel p)
        {
            int id = ++DataSource.Config.RunningParcelID;
            p.ID = id;
            DataSource.Parcels.Add(p);
            return id;//return the id of the new  parcel.
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
                throw new UnvalidIDException("id { p.Id}  is not valid !!");
            }
            int index = DataSource.Parcels.FindIndex(item => item.ID == parcel.ID);
            DataSource.Parcels[index] = parcel;
        }

        public void UpdateDrone(Drone drone)
        {
            if (!(DataSource.Drones.Exists(d => d.ID == drone.ID)))
            {
                throw new UnvalidIDException("id { d.Id}  is not valid !!");
            }
            int index = DataSource.Drones.FindIndex(item => item.ID == drone.ID);
            DataSource.Drones[index] = drone;
        }

        public void UpdateStation(Station station)
        {
            if (!(DataSource.Stations.Exists(s => s.ID == station.ID)))
            {
                throw new UnvalidIDException("id { s.Id}  is not valid !!");
            }
            int index = DataSource.Stations.FindIndex(item => item.ID == station.ID);
            DataSource.Stations[index] = station;
        }

        /// <summary>
        /// recieves a parcel and updates the parcels picked up time
        /// </summary>
        /// <param Name="p"></param>
        public void PickedUp(Parcel p, Drone d)
        {
            p.DroneID = d.ID;
            p.PickedUp = DateTime.Now;//updates the parcels pickedUp time
            UpdateParcel(p);
        }

        /// <summary>
        /// recieves a parcel and updates the parcels delivery time
        /// </summary>
        /// <param name="p"></param>
        public void Delivered(Parcel p)
        {
            p.Delivered = DateTime.Now;
            UpdateParcel(p);
        }

        /// <summary>
        /// recieves a drone and a station and sends the drone to a chargeSlot in that station
        /// </summary>
        /// <param Name="d"></param>
        /// <param Name="s"></param>
        public void SendDroneToChargeSlot(Drone d, Station s)
        {
            s.ChargeSlots--;
            UpdateStation(s);
            DroneCharge dc=new DroneCharge();
            dc.DroneID = d.ID;
            dc.StationID = s.ID;
            DataSource.DroneCharges.Add(dc);
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
        /// searches for the parcel in the array by the Id
        /// </summary>
        /// <param Name="parcelID"></param>
        /// <returns></returns parcel were looking for>
        public Parcel GetParcel(int id)
        {
            Parcel parcelToReturn = default;
            //searches the customer by the id
            if (!(DataSource.Parcels.Exists(p => p.ID == id)))
            {
                throw new UnvalidIDException($"id {id} is not valid !!");
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
        /// searches for the non atributted parcels and coppies them into a new list.
        /// </summary>
        /// <returns></returns the new array>
        public IEnumerable<Parcel> FindNotAttributedParcels()
        {
            List<Parcel> notAttributed =new List<Parcel>();//new list to hold non attributed parcels
            foreach (Parcel p in DataSource.Parcels)//searches for the non attributed parcels
            {
                if (p.DroneID == 0)
                {
                    notAttributed.Add(p);
                }
            }
            return notAttributed;
        }

        /// <summary>
        /// creates an array by searching for available charge slots in the station list.
        /// </summary>
        /// <returns></returns the new list>
        public IEnumerable<Station> FindAvailableStations()
        {
            List<Station> availableStations = new List<Station>();//new list to hold Available Stations
            for (int i = 0; i < DataSource.Stations.Count; i++)
                if (DataSource.Stations[i].ChargeSlots > 0)
                {
                    availableStations.Add(DataSource.Stations[i]);
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
        /// <summary>
        /// creates a new array with the drone's electricity use  
        /// </summary>
        /// <returns>the new array</returns>
        public double[] GetElectricityUse()
        {
            double[] electricityUse = new double[5];
            electricityUse[0] = DataSource.Config.Avalaible;
            electricityUse[1] = DataSource.Config.Light;
            electricityUse[2] = DataSource.Config.Medium;
            electricityUse[3] = DataSource.Config.Heavy;
            electricityUse[4] = DataSource.Config.ChargingRate;
            return electricityUse;
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



