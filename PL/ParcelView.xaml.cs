using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelView.xaml
    /// </summary>
    public partial class ParcelView : Window, INotifyPropertyChanged
    {
        public event Action OnUpdate = delegate { }; //event that will refresh the parcels list every time we update a parcel
        public List<Weight> WeightOptions { get; set; } //list to hold weight options
        public List<Priority> Priorities { get; set; } //list to hold priority options
        public List<int> SenderIDs { get; set; }//list of senders ids.
        public List<int> RecieverIDs { get; set; }//list of reciever ids.
        public event PropertyChangedEventHandler PropertyChanged = delegate { };//an event for binding property who changes
        public bool IsDeleteeMode { get; set; } //to know which window to open: update or add
        private IBL myBl;
        Parcel parcelToDelete;
        bool closeChecked;

        /// <summary>
        /// func that overides the closing window event to prevent closing the window by the x button
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (closeChecked == false)
            {
                e.Cancel = true;
            }
            else
                e.Cancel = false;
        }

        /// <summary>
        /// parcel property for deleting
        /// </summary>
        public Parcel ParcelToDelete
        {
            get { return parcelToDelete; }
            set { parcelToDelete = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ParcelToDelete"));//when changing somthing on window
            }
        }

        private Parcel parcelToAdd;

        /// <summary>
        /// parcel property for adding
        /// </summary>
        public Parcel ParcelToAdd
        {
            get { return parcelToAdd; }
            set { parcelToAdd = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ParcelToAdd"));//when changing somthing on window
            }
        }

        /// <summary>
        /// for deleting parcel
        /// </summary>
        /// <param name="myBl"></param>
        /// <param name="parcel">the parcel to delete</param>
        public ParcelView(IBL myBl, Parcel parcel)
        {
            try
            {
                InitializeComponent();
                DataContext = this;//binding the data
                ParcelToDelete = parcel;
                this.myBl = myBl;
                IsDeleteeMode = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to open the update window!");
            }
        }

        /// <summary>
        /// constructor for adding parcel
        /// </summary>
        /// <param name="myBl"></param>
        public ParcelView(IBL myBl)
        {
            try
            {
                InitializeComponent();
                this.myBl = myBl;
                parcelToAdd = new Parcel();
                parcelToAdd.DroneInParcel = new DroneParcel();
                parcelToAdd.Recipient = new CustomerParcel();
                parcelToAdd.Sender = new CustomerParcel();
                WeightOptions = Enum.GetValues(typeof(Weight)).Cast<Weight>().ToList();
                Priorities = Enum.GetValues(typeof(Priority)).Cast<Priority>().ToList();
                List<CustomerForList> Custumers = myBl.GetCustomerList().ToList();
                SenderIDs = Custumers.Select(item => item.Id).ToList();
                RecieverIDs = Custumers.Select(item => item.Id).ToList();
                DataContext = this;//binding the data
            }
            catch (FailedToGetException ex)
            {
                MessageBox.Show("Failed to add!!" + ex.ToString());
            }
        }

        /// <summary>
        /// adds the parcel into the parcel list
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
                    myBl.AddParcel(ParcelToAdd);
                    res = MessageBox.Show("Added succecfully!!");
                    if (res != MessageBoxResult.None)
                        OnUpdate();//updates the list of stations in the previous window.
                    closeChecked = true;
                    Close();
                }
            }
            catch (FailedToAddException ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show("Failed to add - " + ex.ToString() + " " + ex.InnerException.ToString());
                else
                    MessageBox.Show("Failed to add - " + ex.ToString());
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show("Failed to add -" + ex.ToString());
            }
            catch(InputDoesNotExist ex)
            {
                MessageBox.Show("Failed to add -" + ex.ToString());
            }
        }

        /// <summary>
        /// closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            closeChecked = true;
            Close();
        }

        /// <summary>
        /// opens the window of reciever-custumerView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Customer customer = new Customer();
                customer = myBl.GetCustomer(ParcelToDelete.Recipient.Id);//finds the custumer that we will open the window for
                CustomerView customerView = new CustomerView(myBl, customer);
                customerView.Show();
            }
            catch(InvalidInputException ex)
            {
                MessageBox.Show("Cant show recipient data - " + ex.ToString());
            }
            catch(FailedToGetException ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show("Cant show recipient data- " + ex.ToString() + " " + ex.InnerException.ToString());
                else
                    MessageBox.Show("Cant show recipient data - " + ex.ToString());
            }

        }

        /// <summary>
        /// opens custumerView window for the sender 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Customer customer = new Customer();
                customer = myBl.GetCustomer(ParcelToDelete.Sender.Id);//finds the custumer that we will open the window for
                CustomerView customerView = new CustomerView(myBl, customer);
                customerView.Show();
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show("Cant show sender data - " + ex.ToString());
            }
            catch (FailedToGetException ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show("Cant show sender data- " + ex.ToString() + " " + ex.InnerException.ToString());
                else
                    MessageBox.Show("Cant show sender data - " + ex.ToString());
            }

        }

        /// <summary>
        /// opens the DroneView window for the attributted drone in parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_MouseDoubleClick_2(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Drone drone = new Drone();
                drone = myBl.GetDrone(ParcelToDelete.DroneInParcel.Id);//finds the drone that attributted to the parcel
                DroneView droneView = new DroneView(myBl, drone);
                droneView.Show();
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show("Cant show drone data - " + ex.ToString());
            }
            catch (FailedToGetException ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show("Cant show drone data- " + ex.ToString() + " " + ex.InnerException.ToString());
                else
                    MessageBox.Show("Cant show drone data - " + ex.ToString());
            }
        }

        /// <summary>
        /// deleting the parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Delete parcel?", "myApp", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ParcelToDelete.IsDeleted = true;
                try
                {
                    myBl.DeleteParcel(ParcelToDelete);
                    OnUpdate();
                }
                catch (FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed to delete -" + ex.ToString());
                }
                closeChecked = true;
                Close();
            }
        }

    }
}
