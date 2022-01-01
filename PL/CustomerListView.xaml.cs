using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using BlApi;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListView.xaml
    /// </summary>
    public partial class CustomerListView : Window, INotifyPropertyChanged
    {
        private IBL myBl = BlFactory.GetBl();
        private List<CustomerForList> customers;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        //property for the customers list - for binding to the items source in the xaml
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
            this.myBl = myBl;
            DataContext = this;
            InitializeComponent();
            GetCustomerListFromBL();
        }

        /// <summary>
        /// double click event that opens a window for updating the customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customersList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerForList c = customersList.SelectedItem as CustomerForList;
            Customer customer = new Customer();
            customer = myBl.GetCustomer(c.Id);//gets the selected customer as customer instead of customer for list 
            CustomerView customerWindow = new CustomerView(myBl, customer);
            customerWindow.OnUpdate += StationWindow_onUpdate;//registers to event that is announced when a station was added or updated 
            customerWindow.Show();
        }

        /// <summary>
        /// func that refreshes the stations list
        /// </summary>
        private void StationWindow_onUpdate()
        {
            GetCustomerListFromBL();
        }

        private void GetCustomerListFromBL()
        {
            Customers = myBl.GetCustomerList().ToList();
        }

        private void customersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// opens the customer view window for adding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customerAdd_Click(object sender, RoutedEventArgs e)
        {
            CustomerView customerView = new CustomerView(myBl);
            customerView.OnUpdate += StationWindow_onUpdate;
            customerView.Show();
        }

        private void cancelAdd_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
