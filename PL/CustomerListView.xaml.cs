using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BlApi;
using BO;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListView.xaml
    /// </summary>
    public partial class CustomerListView : Window, INotifyPropertyChanged
    {
        private IBL myBl;
        private List<CustomerForList> customers;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// property for the customers list - for binding to the items source in the xaml
        /// </summary>
        public List<CustomerForList> Customers
        {
            get { return customers; }
            set
            {
                customers = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Customers"));
            }
        }

        /// <summary>
        /// constructor that initialize and call for the list of customers
        /// </summary>
        /// <param name="myBl"></param>
        public CustomerListView(IBL myBl)
        {
            try
            {
                this.myBl = myBl;
                DataContext = this;
                InitializeComponent();
                GetCustomerListFromBL();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed opening customer list");
            }
        }

        /// <summary>
        /// double click event that opens a window for updating the customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //double click only on a customer
            try
            {
                CustomerForList c = customersList.SelectedItem as CustomerForList;
                if (c != null)
                {
                    Customer customer = new Customer();
                    customer = myBl.GetCustomer(c.Id);//gets the selected customer as customer instead of customer for list 
                    CustomerView customerWindow = new CustomerView(myBl, customer);
                    customerWindow.OnUpdate += StationWindow_onUpdate;//registers to event that is announced when a station was added or updated 
                    customerWindow.Show();
                }
                else
                {
                    MessageBox.Show("Please select a customer!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured");
            }
        }

        /// <summary>
        /// func that refreshes the stations list
        /// </summary>
        private void StationWindow_onUpdate()
        {
            GetCustomerListFromBL();
        }

        /// <summary>
        /// gets the customers list 
        /// </summary>
        private void GetCustomerListFromBL()
        {
            Customers = myBl.GetCustomerList().ToList();
        }

        /// <summary>
        /// opens the customer view window for adding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customerAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerView customerView = new CustomerView(myBl);
                customerView.OnUpdate += StationWindow_onUpdate;
                customerView.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading the adding window");
            }
        }

        /// <summary>
        /// button to close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR");
            }
        }

        private void customersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
