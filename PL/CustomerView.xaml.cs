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
    public partial class CustomerView : Window, INotifyPropertyChanged
    {
        public event Action OnUpdate = delegate { }; //event that will refresh the customer list every time we update a Customer
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public bool IsUpdateMode { get; set; } //to know which window to open: update or add
        Customer customerToAdd;
        private string originalCustomerName; //temp to hold the customer's name of the customer from the list view window (this will not be used for items source)
        private string originalPhoneNumber; //temp to hold the customers phone number from the list view window (this will not be used for items source)
        private List<int> sentIdList;
        private IBL myBl;
        private List<int> recievedIdList;
        private Customer custForUpdate;
        public List<CustomersType> CustomerTypeOptions { get; set; }

        /// <summary>
        /// property for the parcels id list - for binding to the items source in the xaml
        /// </summary>
        public List<int> SentIdList
        {
            get { return sentIdList; }
            set
            {
                sentIdList = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SentIdList"));
            }
        }

        /// <summary>
        /// property for the parcels id list - for binding to the items source in the xaml
        /// </summary>
        public List<int> RecievedIdList
        {
            get { return recievedIdList; }
            set
            {
                recievedIdList = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RecievedIdList"));
            }
        }

        /// <summary>
        /// the customer for binding in adding
        /// </summary>
        public Customer CustomerToAdd
        {
            get { return customerToAdd; }
            set
            {
                customerToAdd = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CustomerToAdd"));
            }
        }

        /// <summary>
        /// customer property for update a customer from the list
        /// </summary>
        public Customer CustForUpdate
        {
            get { return custForUpdate; }
            set
            {
                custForUpdate = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CustForUpdate"));
            }
        }

        /// <summary>
        /// ctor for the customers update 
        /// </summary>
        /// <param name="myBl"></param>
        /// <param name="customer"></param>
        public CustomerView(IBL myBl, Customer customer)
        {
            try
            {
                InitializeComponent();
                DataContext = this;//binding the data
                CustForUpdate = customer;
                originalCustomerName = customer.Name;
                originalPhoneNumber = customer.PhoneNumber;
                this.myBl = myBl;
                sentIdList = customer.SentParcels.Select(item => item.Id).ToList();
                recievedIdList = customer.ReceiveParcels.Select(item => item.Id).ToList();
                blockLocation.Text = customer.Location.ToString();
                IsUpdateMode = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to open the update window!");
            }
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
                CustomerTypeOptions = Enum.GetValues(typeof(CustomersType)).Cast<CustomersType>().ToList();
                DataContext = this;//binding the data
                customerToAdd = new Customer();
                customerToAdd.ReceiveParcels = new List<ParcelCustomer>();
                customerToAdd.SentParcels = new List<ParcelCustomer>();
                customerToAdd.Location = new Location();
            }
            catch (FailedToGetException ex)
            {
                MessageBox.Show("Failed to open the adding window!" + ex.ToString());
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

        /// <summary>
        /// close window in adding grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to close window!");
            }
        }

        /// <summary>
        /// closing window in update grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to close window!");
            }
        }

        /// <summary>
        /// updates the data of a custumer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (originalCustomerName == CustForUpdate.Name && originalPhoneNumber == CustForUpdate.PhoneNumber)
            {
                MessageBox.Show("Fill in the following details to update");
            }
            else
            {
                var result = MessageBox.Show("Save updates to current customer?", "myApp", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        myBl.UpdateCustomer(CustForUpdate.Id, CustForUpdate.Name, CustForUpdate.PhoneNumber);
                        var res = MessageBox.Show("success");
                        if (res != MessageBoxResult.None)
                        {
                            CustForUpdate = myBl.GetCustomer(CustForUpdate.Id);
                            OnUpdate();//refresh the customer list
                            originalCustomerName = CustForUpdate.Name;
                            originalPhoneNumber = CustForUpdate.PhoneNumber;
                        }
                    }
                    catch (FailedToUpdateException ex)
                    {
                        MessageBox.Show("Failed to update - " + ex.ToString());
                    }
                    catch (InvalidInputException ex)
                    {
                        MessageBox.Show("Failed to update - " + ex.ToString());
                    }
                    catch (FailedToGetException ex)
                    {
                        if (ex.InnerException != null)
                            MessageBox.Show("Failed to update - " + ex.ToString() + " " + ex.InnerException.ToString());
                        else
                            MessageBox.Show("Failed to update - " + ex.ToString());
                    }
                }
            }

        }

        /// <summary>
        /// a mouse double click on id will open the ids  parcel view 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxSent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //only click on a list item
            try
            {
                int id = (int)listBoxSent.SelectedItem;
                Parcel parcel = new Parcel();
                parcel = myBl.GetParcel(id);//gets the selected item as parcel.
                ParcelView parcelView = new ParcelView(myBl, parcel);
                parcelView.Show();
            }
            catch (FailedToGetException ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.ToString()+" "+ex.InnerException.ToString());
                }
                else
                    MessageBox.Show(ex.ToString());
            }
            catch(Exception)
            {
                MessageBox.Show("Error loading the sent parcel");
            }
        }

        /// <summary>
        /// a mouse double click on id will open the ids  parcel view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxRecieved_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int id = (int)listBoxRecieved.SelectedItem;
                Parcel parcel = new Parcel();
                parcel = myBl.GetParcel(id);//gets the selected item as parcel.
                ParcelView parcelView = new ParcelView(myBl, parcel);
                parcelView.Show();
            }
            catch (FailedToGetException ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.ToString() + " " + ex.InnerException.ToString());
                }
                else
                    MessageBox.Show(ex.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading the sent parcel");
            }
        }

        /// <summary>
        /// button to delete a customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Delete customer?", "myApp", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                CustForUpdate.IsDeleted = true;
                try
                {
                    myBl.DeleteCustomer(CustForUpdate);
                    OnUpdate();
                }
                catch (FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed to delete -" + ex.ToString());
                }
                Close();
            }
        }
    }
}
