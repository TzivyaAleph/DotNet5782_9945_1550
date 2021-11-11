using System;
using IBL.BO;

namespace BL
{
    
    public class BLObject
    {
        IDAL.DO.IDal myDal;
        public BLObject()
        {
            myDal = new DalObject.DalObject();
        }

        public Customer GetCustomer(int id)
        {
            Customer customer = default;
            try
            {
                IDAL.DO.Customer dalCustomer = myDal.GetCustomer(id);
            }
            catch (IDAL.DO.CustomerException custEx)
            { 
                throw new BLCustomerException($"Customer id {id} was not found",custEx);
            }
            return customer;
        }

        
    }
}
