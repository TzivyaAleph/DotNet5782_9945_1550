using IBL.BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneView.xaml
    /// </summary>
    public partial class DroneView : Window, INotifyPropertyChanged
    {

        private BL.IBL myBl;
        private string originalDroneModel; //temp drone to hold the drone from the drone list view window (this drone will not be used for items source)
        private Drone selectedDrone;
        private DroneForList droneToAdd;

        /// <summary>
        /// a drone for putting the input data in it.
        /// </summary>
        public DroneForList DroneToAdd
        {
            get { return droneToAdd; }
            set
            {
                droneToAdd = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DroneToAdd"));
            }
        }

        /// <summary>
        /// a drone for updating the drone data
        /// </summary>
        public Drone SelectedDrone
        {
            get { return selectedDrone; }
            set
            {
                selectedDrone = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedDrone"));
            }
        }


        public List<Weight> WeightOptions { get; set; } //list to hold weight options
        public List<DroneStatuses> Statuses { get; set; } //list to hold Drone Status options
        public bool IsUpdateMode { get; set; } //to know wich window to open: update or add
        public event Action OnUpdate = delegate { }; //event that will refresh the drones list every time we update a drone
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// a ctor foe adding new drone
        /// </summary>
        /// <param name="bl"></param>
        public DroneView(BL.IBL bl)
        {
            Initialize();
            droneToAdd = new DroneForList();
            myBl = bl;
            InitializeComponent();
        }

        /// <summary>
        /// c-tor for drone updates
        /// </summary>
        /// <param name="bl">bl object</param>
        /// <param name="dr">the selected drone</param>
        public DroneView(BL.IBL bl, Drone dr)
        {
            Initialize();
            SelectedDrone = dr;
            originalDroneModel = dr.Model;
            myBl = bl;
            IsUpdateMode = true;
            if (SelectedDrone.ParcelInDelivery != null) 
                parcelView.Text = SelectedDrone.ParcelInDelivery.ToString();
            InitializeComponent();
        }

        private void Initialize()
        {
            WeightOptions = Enum.GetValues(typeof(Weight)).Cast<Weight>().ToList();
            Statuses = Enum.GetValues(typeof(DroneStatuses)).Cast<DroneStatuses>().ToList();
            DataContext = this;// לדעת איפה לחפש את הפרופרטיז ששמנו בביינדינג
        }

        /// <summary>
        /// bottom click for add will send a new drone and station id to bl add drone function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DroneForList newDr = AddNewDronFromPage();
                int stationId = 1234;
                myBl.AddDrone(newDr, stationId);
                MessageBox.Show("Added succecfully!!");
                OnUpdate();//updates the list of drones in the previous window.
                Close();
            }
            catch (FailedToAddException ex)
            {
                MessageBox.Show("Failed to add!!");
            }

        }

        /// <summary>
        /// creates new drone with data from user
        /// </summary>
        /// <returns>the created drone</returns>
        private DroneForList AddNewDronFromPage()
        {
            DroneForList newDrone = new DroneForList();
            newDrone.Id = int.Parse(txtID.Text);
            newDrone.Model = txtModel.Text;
            newDrone.Weight = (Weight)int.Parse(CBXWeight.Text);
            return newDrone;
        }

        /// <summary>
        /// for closing the window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// button to close the update drone window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// updates the model of the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateModelBtn_Click(object sender, RoutedEventArgs e)
        {
            //checkes if the model was updated by the user
            if (originalDroneModel != SelectedDrone.Model)
            {
                try
                {
                    myBl.UpdateDrone(SelectedDrone.Id, SelectedDrone.Model);
                    var res = MessageBox.Show("success");
                    if (res != MessageBoxResult.None)
                    {
                        SelectedDrone = myBl.GetDrone(SelectedDrone.Id);
                        OnUpdate();
                    }
                }
                catch (FailedToUpdateException)
                {
                    MessageBox.Show("failed");
                }
            }
        }

        /// <summary>
        /// sends drone to charge slot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendToChargeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myBl.SendDroneToChargeSlot(SelectedDrone);
                var res = MessageBox.Show("success");
                if (res != MessageBoxResult.None)
                {
                    SelectedDrone = myBl.GetDrone(SelectedDrone.Id);
                    OnUpdate();
                }
            }
            catch (IBL.BO.FailedToUpdateException)
            {
                MessageBox.Show("failed");
            }

            //Close();
        }

        private void releaseDroneBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                myBl.ReleasedroneFromeChargeSlot(SelectedDrone);
                var res = MessageBox.Show("success");
                if (res != MessageBoxResult.None)
                {
                    SelectedDrone = myBl.GetDrone(SelectedDrone.Id);
                    OnUpdate();
                }
            }
            catch (IBL.BO.FailedToUpdateException)
            {
                MessageBox.Show("failed");
            }
        }

        //private DroneForList converteDroneToDroneForList(Drone drone)
        //{
        //    DroneForList droneToC = new DroneForList();
        //    drone.CopyPropertiesTo(droneToC);
        //    droneToC.CurrentLocation.Longitude = drone.CurrentLocation.Longitude;
        //    droneToC.CurrentLocation.Latitude = drone.CurrentLocation.Latitude;
        //    droneToC.ParcelId = drone.ParcelInDelivery.Id;
        //    return droneToC;
        //}
    }
}
