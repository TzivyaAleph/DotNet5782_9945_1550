using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {

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

        /// <summary>
        /// coppies the customer array
        /// </summary>
        /// <returns></returns the coppied array>
        public IEnumerable<Customer> CopyCustomerArray()
        {
            List<Customer> newList = new List<Customer>(DataSource.Customers);
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
                foreach (var par in DataSource.Parcels)
                {
                    //the parcel has been attributed to the customer and has been delieverd
                    if (par.TargetID == cust.ID && par.Delivered < DateTime.Now)
                    {
                        customerWithUnDelieverdParcel.Add(cust);
                        break;
                    }
                }
            }
            return customerWithUnDelieverdParcel;
        }

        /// <summary>
        /// puts a updated customer in the customers list
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            if (!(DataSource.Customers.Exists(c => c.Id == customer.Id)))
            {
                throw new UnvalidIDException($"id {customer.Id}  is not valid !!");
            }
            int index = DataSource.Customers.FindIndex(item => item.Id == customer.Id);
            DataSource.Customers[index] = customer;
        }
    }
}
