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

        /// <addDrone>
        /// add new drone and updates 
        /// </summary>
        /// <returns></returns>
        public void AddDrone(Drone d)
        {
            if (DataSource.Drones.Exists(drone => drone.ID == d.ID))
            {
                throw new ExistingObjectException($"drone {d.ID} allready exists !!");
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
                throw new ExistingObjectException($"customer {c.Name} allready exists !!");
            }
            DataSource.Customers.Add(c);
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

        /// <summary>
        /// searches for the drone in the array by the Id
        /// </summary>
        /// <param Name="droneID"></param>
        /// <returns></returnsthe drone were looking for>
        public Drone GetDrone(int droneID)
        {
            if (!(DataSource.Drones.Exists(d => d.ID == droneID)))
            {
                throw new UnvalidIDException("id { d.Id}  is not valid !!");
            }
            int index = DataSource.Drones.FindIndex(item => item.ID == droneID);
            return DataSource.Drones[index];
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
            if (!(DataSource.Customers.Exists(client => client.ID == customerID)))
            {
                throw new UnvalidIDException($"id {customerID} is not valid !!");
            };
            customerToReturn = DataSource.Customers.Find(c => c.ID == customerID);
            return customerToReturn;
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



