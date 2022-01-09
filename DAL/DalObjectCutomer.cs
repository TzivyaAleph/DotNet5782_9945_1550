using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    partial class DalObject
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
            if (!(DataSource.Customers.Exists(client => client.Id == customerID)))
            {
                throw new UnvalidIDException($"id {customerID} does not exist !!");
            };
            customerToReturn = DataSource.Customers.Find(c => c.Id == customerID);
            return customerToReturn;
        }

        /// <summary>
        /// gets a customer and adds it to the array.
        /// </summary>
        /// <param Name="c"></param>
        /// <returns></returns>
        public void AddCustomer(Customer c)
        {
            if (DataSource.Customers.Exists(client => client.Id == c.Id))
            {
                throw new ExistingObjectException($"customer {c.Name} allready exists !!");
            }
            DataSource.Customers.Add(c);
        }

        /// <summary>
        /// coppies the customer array
        /// </summary>
        /// <returns></returns the coppied array>
        public IEnumerable<Customer> CopyCustomerArray(Func<Customer, bool> predicate = null)
        {
            List<Customer> newList = new List<Customer>(DataSource.Customers);
            if(predicate==null)
               return newList;
            return newList.Where(predicate);
        }

        public void UpdateCustomer(Customer customer)
        {
            if (!DataSource.Customers.Exists(c => c.Id == customer.Id))
            {
                throw new UnvalidIDException($"id {customer.Id}  is not valid !!");
            }
            int index = DataSource.Customers.FindIndex(item => item.Id == customer.Id);
            DataSource.Customers[index] = customer;
        }
    }
}
