using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : Window,INotifyPropertyChanged
    {
        public event Action OnUpdate = delegate { }; //event that will refresh the station list every time we update a Customer
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsUpdateMode { get; set; } //to know which window to open: update or add
        Customer customerToAdd = new Customer();
        /// <summary>
        /// the customer for binding in adding
        /// </summary>
        public Customer CustomerToAdd
        {
            get { return customerToAdd; }
            set {
                customerToAdd = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CustomerToAdd"));
            }
        }

        private IBL myBl;

        /// <summary>
        /// ctor for the customers update 
        /// </summary>
        /// <param name="myBl"></param>
        /// <param name="customer"></param>
        public CustomerView(IBL myBl, Customer customer)
        {
            this.myBl = myBl;

        }

        /// <summary>
        /// ctor for adding a customer
        /// </summary>
        /// <param name="Bl"></param>
        public CustomerView(IBL Bl)
        {
            try
            {
                InitializeComponent();
                myBl = Bl;
                DataContext = this;//binding the data
                customerToAdd = new Customer();
                customerToAdd.ReceiveParcels = new List<ParcelCustomer>();
                customerToAdd.SentParcels = new List<ParcelCustomer>();
                customerToAdd.Location = new Location();
            }
            catch (FailedToGetException ex)
            {
                MessageBox.Show("Failed to add!!" + ex.ToString());
            }
        }

        /// <summary>
        /// adds the new customer with rhe user data to list of customers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var res = MessageBox.Show("Are you sure you want to add?", "myApp", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Yes)
                {
                    myBl.AddCustomer(CustomerToAdd);
                    res = MessageBox.Show("Added succecfully!!");
                    if (res != MessageBoxResult.None)
                        OnUpdate();//updates the list of stations in the previous window.
                    Close();
                }
            }
            catch (FailedToAddException ex)
            {
                MessageBox.Show("Failed to add -" + ex.ToString());
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show("Failed to add -" + ex.ToString());
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
