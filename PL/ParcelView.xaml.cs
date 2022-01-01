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
        public bool IsUpdateMode { get; set; } //to know which window to open: update or add
        private IBL myBl;
        Parcel parcelToDelete;

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

        Parcel parcelToAdd;

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
            InitializeComponent();
            DataContext = this;//binding the data
            ParcelToDelete = parcel;
            this.myBl = myBl;
            IsUpdateMode = true;
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
            Close();
        }
    }
}
