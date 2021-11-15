using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{
    
    public class BLObject
    {
 
      IDAL.DO.IDal myDal;
        internal static Random rand = new Random();
        internal List<IBL.BO.DroneForList> drones;

        public BLObject()
        {
            myDal = new DalObject.DalObject();
          
        }
        public Customer GetCustomer(int id)
        {
            Customer customer = default;
            try
            {
                customer = myDal.GetCustomer(id);
            }
            catch (IDAL.DO.CustomerException custEx)
            { 
                throw new BLIdException($"Customer id {id} was not found",custEx);
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
            catch (IDAL.DO.CustomerException custEx)
            {
                throw new BLIdException($"Customer id {id} was not found", custEx);
            }
            return parcel;
        }



    }
}
