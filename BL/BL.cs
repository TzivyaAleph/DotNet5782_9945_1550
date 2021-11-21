using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;


namespace BL
{
    
    public partial class BL
    {
 
      IDAL.DO.IDal myDal;

        internal static Random rand = new Random();
        internal List<IBL.BO.DroneForList> drones;

        /// <summary>
        /// initializing list of drones.
        /// </summary>
        public BL()
        {
            myDal = new DalObject.DalObject();
            DroneForList droneTemp=new DroneForList();
            foreach(var item in myDal.CopyDroneArray())
            {
                droneTemp.ID = item.ID;
                droneTemp.Model = item.Model;
                droneTemp.MaxWeight =(Weight)item.MaxWeight;
                foreach (var par in myDal.CopyParcelArray())
                { 
                    // the drone has been attributted but the parcel has not delievred.
                    if(par.DroneID==item.ID&&par.Delivered>DateTime.Now)
                    {
                        droneTemp.DroneStatuses = DroneStatuses.Delivered;
                        //finds the customer who send the parcel.
                        IDAL.DO.Customer customerTemp = new IDAL.DO.Customer();
                        while (myDal.CopyCustomerArray().GetEnumerator().MoveNext())
                        {
                            if (myDal.CopyCustomerArray().GetEnumerator().Current.ID == par.SenderID)
                            {
                                customerTemp = myDal.CopyCustomerArray().GetEnumerator().Current;
                                break;
                            }
                        }
                        //the drone has been attributted but wasnt picked up
                        if (par.PickedUp > DateTime.Now)
                        {
                            //the current location is the clossest station to the sender
                            IDAL.DO.Station clossestStation = new IDAL.DO.Station();
                            clossestStation = myDal.GetClossestStation(customerTemp.Lattitude,customerTemp.Longtitude,(List<IDAL.DO.Station>)myDal.CopyStationArray());
                            droneTemp.CurrentLocation.Latitude = clossestStation.Lattitude;
                            droneTemp.CurrentLocation.Longitude = clossestStation.Longitude;
                            droneTemp.Battery = 
                        }
                        //the parcel has been picked up
                        else
                        {
                            droneTemp.CurrentLocation.Latitude = customerTemp.Lattitude;
                            droneTemp.CurrentLocation.Longitude = customerTemp.Longtitude;
                        }
                    }
                    //the drone has been attributted but does not executing a delievery.
                    else
                    {
                        int num = (rand.Next(0, 1)*2);
                        if (num == 0)
                            droneTemp.DroneStatuses = DroneStatuses.Available;
                        else if (num == 2)
                            droneTemp.DroneStatuses = DroneStatuses.Maintenance;
                    }
                    //the drone is maintanse
                    if(droneTemp.DroneStatuses==DroneStatuses.Maintenance)
                    {
                        //the location is the location of a random station.
                        int num = rand.Next(0, myDal.CopyStationArray().Count());
                        IEnumerator enumerator = myDal.CopyStationArray().GetEnumerator();
                        int i = 0;
                        IDAL.DO.Station temp = new IDAL.DO.Station();
                        while (i < num)
                        {
                            enumerator.MoveNext();
                        }
                        temp = (IDAL.DO.Station)enumerator.Current;
                        droneTemp.CurrentLocation.Latitude = temp.Lattitude;
                        droneTemp.CurrentLocation.Longitude = temp.Longitude;
                        droneTemp.Battery = rand.Next(0, 20);
                    }
                    //the drone is available.
                    if(droneTemp.DroneStatuses == DroneStatuses.Available)
                    {
                        //the cuurent location of the drone is the location of a random
                        //customer who has attributted parcel who hasnt been delieverd yet.
                        int num = rand.Next(0, myDal.ListOfCustomerWithUnDelieverdParcel().Count());
                        IEnumerator enumerator = myDal.ListOfCustomerWithUnDelieverdParcel().GetEnumerator();
                        int i = 0;
                        IDAL.DO.Customer temp = new IDAL.DO.Customer();
                        while (i < num)
                        {
                            enumerator.MoveNext();
                        }
                        temp = (IDAL.DO.Customer)enumerator.Current;
                        droneTemp.CurrentLocation.Latitude = temp.Lattitude;
                        droneTemp.CurrentLocation.Longitude = temp.Longtitude;
                    }
                }
                drones.Add(droneTemp);
            }
        }


        public Customer GetCustomer(int id)
        {
            Customer customer = default;
            try
            {
                customer.Id = myDal.GetCustomer(id).ID;
            }
            catch (IDAL.DO.UnvalidIDException custEx)
            { 
                throw new BLInvalidInputException($"Customer id {id} was not found",custEx);
            }
            return customer;
        }

        public Parcel GetParcel(int id)
        {
            Parcel parcel = default;
            try
            {
                IDAL.DO.Parcel dalParcel = myDal.GetParcel(id);
            }
            catch (IDAL.DO.UnvalidIDException custEx)
            {
                throw new BLInvalidInputException($"Customer id {id} was not found", custEx);
            }
            return parcel;
        }



    }
}
