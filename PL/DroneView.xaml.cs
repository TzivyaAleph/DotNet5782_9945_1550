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
        public List<string> Names { get; set; }//list of stations name
        public bool IsUpdateMode { get; set; } //to know wich window to open: update or add
        public event Action OnUpdate = delegate { }; //event that will refresh the drones list every time we update a drone
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// a ctor for adding new drone
        /// </summary>
        /// <param name="bl"></param>
        public DroneView(BL.IBL bl)
        {
            try
            {
                InitializeComponent();
                myBl = bl;
                Initialize();
                Names = new List<string>();
                List<StationForList> stations = bl.GetStationList().ToList();
                //create list of stations name
                foreach (var st in stations)
                {
                    Names.Add(st.Name);
                }
                droneToAdd = new DroneForList();
            }
            catch (FailedToGetException ex)
            {
                MessageBox.Show("Failed to add!!" + ex.ToString());
            }
        }

        /// <summary>
        /// c-tor for drone updates
        /// </summary>
        /// <param name="bl">bl object</param>
        /// <param name="dr">the selected drone</param>
        public DroneView(BL.IBL bl, Drone dr)
        {
            InitializeComponent();
            Initialize();
            SelectedDrone = dr;
            originalDroneModel = dr.Model;
            myBl = bl;
            IsUpdateMode = true;             
            InitializeComponent();
        }

        /// <summary>
    /// inisialize for both add and update
    /// </summary>
        private void Initialize()
        {
            WeightOptions = Enum.GetValues(typeof(Weight)).Cast<Weight>().ToList();
            Statuses = Enum.GetValues(typeof(DroneStatuses)).Cast<DroneStatuses>().ToList();
            DataContext = this;// לדעת איפה לחפש את הפרופרטיז ששמנו בביינדינג
            try
            {
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri("..\\..\\..\\images\\main.jpg", UriKind.Relative));
                grMain.Background = b;
            }
            catch { };
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
                //finds the station with the input name and sends the station id to bl add
                List<StationForList> stations = myBl.GetStationList().ToList();
                StationForList stationForList = stations.FirstOrDefault(x => x.Name ==cbxStations.Text);
                int stationId = stationForList.Id;
                //asks the user if hes sure 
                var res = MessageBox.Show("Are you sure you want to add?", "myApp", MessageBoxButton.YesNoCancel);
                if(res==MessageBoxResult.Yes)
                {
                    myBl.AddDrone(DroneToAdd, stationId);
                    res = MessageBox.Show("Added succecfully!!");
                    if(res!=MessageBoxResult.None)
                    OnUpdate();//updates the list of drones in the previous window.
                    Close();
                }
            }
            catch (FailedToAddException ex)
            {
                MessageBox.Show("Failed to add -" + ex.ToString());
            }
            catch (FailedToGetException ex)
            {
                MessageBox.Show("Failed to add!!" + ex.ToString());
            }

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
                var result = MessageBox.Show("Save updates to current drone?", "myApp", MessageBoxButton.YesNo);
                if (result==MessageBoxResult.Yes)
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
                    catch (FailedToUpdateException ex)
                    {
                        MessageBox.Show("Failed to update - "+ ex.ToString());
                    }
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
            var result = MessageBox.Show("Send drone to charge?", "myApp", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    myBl.SendDroneToChargeSlot(SelectedDrone);
                    var res = MessageBox.Show("Success");
                    if (res != MessageBoxResult.None)
                    {
                        SelectedDrone = myBl.GetDrone(SelectedDrone.Id);
                        OnUpdate();
                    }
                }
                catch (IBL.BO.FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed sending drone to charge - " + ex.ToString());
                }
            }             
        }

        /// <summary>
        /// button to release drone from charging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void releaseDroneBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Release drone from charge?", "myApp", MessageBoxButton.YesNo);//gets the users choice (yes or no)
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    myBl.ReleasedroneFromeChargeSlot(SelectedDrone);
                    var res = MessageBox.Show("Success");
                    if (res != MessageBoxResult.None)
                    {
                        SelectedDrone = myBl.GetDrone(SelectedDrone.Id);
                        OnUpdate();
                    }
                }
                catch (IBL.BO.FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed releasing drone - " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// button to send drone to delivery
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendDroneToDeliverBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Send drone to delivery?", "myApp", MessageBoxButton.YesNo);//gets the users choice (yes or no)
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    myBl.AttributingParcelToDrone(SelectedDrone.Id);
                    var res = MessageBox.Show("Success");
                    if (res != MessageBoxResult.None)
                    {
                        SelectedDrone = myBl.GetDrone(SelectedDrone.Id);
                        OnUpdate();
                    }
                }
                catch (IBL.BO.FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed to attribute - " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// button to pickup a package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dronePickUpBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Send drone to pick-up?", "myApp", MessageBoxButton.YesNo);//gets the users choice (yes or no)
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    myBl.pickedUp(SelectedDrone.Id);
                    var res = MessageBox.Show("Success");
                    if (res != MessageBoxResult.None)
                    {
                        SelectedDrone = myBl.GetDrone(SelectedDrone.Id);
                        OnUpdate();
                    }
                }
                catch (IBL.BO.FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed to pick up - " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// button to deliver a parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deliverParcelBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Deliver parcel?", "myApp", MessageBoxButton.YesNo);//gets the users choice (yes or no)
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    myBl.Delivered(SelectedDrone.Id);
                    var res = MessageBox.Show("Success");
                    if (res != MessageBoxResult.None)
                    {
                        SelectedDrone = myBl.GetDrone(SelectedDrone.Id);
                        OnUpdate();
                    }
                }
                catch (IBL.BO.FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed to pick up - " + ex.ToString());
                }
            }
        }

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
       {
            var bc = new BrushConverter();
             if(txtID.Text.First()=='-' || txtID.Text.Length!=4||!txtID.Text.All(char.IsDigit))
                txtID.BorderBrush=(Brush)bc.ConvertFrom("#FFABADB3");
        }

    }
}
