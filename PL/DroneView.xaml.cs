using BO;
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

        private BlApi.IBL myBl;
        BackgroundWorker worker;
        private string originalDroneModel; //temp drone to hold the drone from the drone list view window (this drone will not be used for items source)
        private Drone selectedDrone;
        private DroneForList droneToAdd;
        public List<Weight> WeightOptions { get; set; } //list to hold weight options
        public List<DroneStatuses> Statuses { get; set; } //list to hold Drone Status options
        public List<string> Names { get; set; }//list of stations name
        public bool IsUpdateMode { get; set; } //to know wich window to open: update or add
        public event Action OnUpdate = delegate { }; //event that will refresh the drones list every time we update a drone
        public event PropertyChangedEventHandler PropertyChanged = delegate { };//event to tell us when a property was changed- so we know to refresh the binding
        private List<ParcelInDelivery> parcelsInDrone;
        private bool isAutomaticMode;

        /// <summary>
        /// prop for a bool parameter that checks if the simulator is running
        /// </summary>
        public bool IsAutomaticMode
        {
            get { return isAutomaticMode; }
            set
            {
                isAutomaticMode = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsAutomaticMode"));
            }
        }

        /// <summary>
        /// a list to put the parcels that the drone is holding
        /// </summary>
        public List<ParcelInDelivery> ParcelsInDrone
        {
            get { return parcelsInDrone; }
            set
            {
                parcelsInDrone = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ParcelsInDrone"));
            }
        }

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

        /// <summary>
        /// prop for a bool parameter that checkes if the drone was attributed but didnt pick-up the parcel (for the triggers binding) 
        /// </summary>
        public bool IsPickUpEnabled
        {
            get
            {
                return selectedDrone != null && selectedDrone.DroneStatuses == DroneStatuses.Delivered &&
                    selectedDrone.ParcelInDelivery.OnTheWay == false;
            }
        }

        /// <summary>
        /// prop for a bool parameter that checkes if the drone has picked up the parcel (for the triggers binding)
        /// </summary>
        public bool IsDeliverParcelEnabled
        {
            get
            {
                return selectedDrone != null && selectedDrone.DroneStatuses == DroneStatuses.Delivered &&
                    selectedDrone.ParcelInDelivery.OnTheWay == true;
            }
        }

        /// <summary>
        /// prop for a bool parameter that checkes if the drone is Maintenance (for the triggers binding)
        /// </summary>
        public bool IsReleaseDroneEnabled
        {
            get
            {
                return selectedDrone != null && selectedDrone.DroneStatuses == DroneStatuses.Maintenance;
            }
        }

        /// <summary>
        /// a ctor for adding new drone
        /// </summary>
        /// <param name="bl"></param>
        public DroneView(BlApi.IBL bl)
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
                MessageBox.Show("Failed to add!" + ex.ToString());
            }
        }

        /// <summary>
        /// c-tor for drone updates
        /// </summary>
        /// <param name="bl">bl object</param>
        /// <param name="dr">the selected drone</param>
        public DroneView(BlApi.IBL bl, Drone dr)
        {
            try
            {
                InitializeComponent();
                SelectedDrone = dr;
                Initialize();
                originalDroneModel = dr.Model;
                myBl = bl;
                IsUpdateMode = true;
                if (SelectedDrone.ParcelInDelivery != null)
                {
                    InitializeParcelInDrone();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to open the update window!");
            }
        }

        /// <summary>
        /// initializes the parcels the drone has
        /// </summary>
        private void InitializeParcelInDrone()
        {
            ParcelsInDrone = new List<ParcelInDelivery>();
            ParcelsInDrone.Add(SelectedDrone.ParcelInDelivery);
        }

        /// <summary>
        /// inisializer for both grids: add and update
        /// </summary>
        private void Initialize()
        {
            WeightOptions = Enum.GetValues(typeof(Weight)).Cast<Weight>().ToList();
            Statuses = Enum.GetValues(typeof(DroneStatuses)).Cast<DroneStatuses>().ToList();
            DataContext = this;
        }

        /// <summary>
        /// button for add, will send a new drone and station id to the bl add drone function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtID.Text != "0" && txtModel.Text.Length != 0 && cbxStations.SelectedIndex > -1)
            {
                try
                {
                    //finds the station with the input name and sends the station id to bl add
                    List<StationForList> stations = myBl.GetStationList().ToList();
                    StationForList stationForList = stations.FirstOrDefault(x => x.Name == cbxStations.Text);
                    int stationId = stationForList.Id;
                    //asks the user if hes sure 
                    var res = MessageBox.Show("Are you sure you want to add?", "myApp", MessageBoxButton.YesNoCancel);
                    if (res == MessageBoxResult.Yes)
                    {
                        myBl.AddDrone(DroneToAdd, stationId);
                        res = MessageBox.Show("Added succecfully!!");
                        if (res != MessageBoxResult.None)
                        {
                            OnUpdate();//updates the list of drones in the previous window.
                        }
                        Close();
                    }
                }
                catch (FailedToAddException ex)
                {
                    MessageBox.Show("Failed to add -" + ex.ToString());
                }
                catch (FailedToGetException ex)
                {
                    MessageBox.Show("Failed to add -" + ex.ToString());
                }
                catch (InvalidInputException ex)
                {
                    MessageBox.Show("Failed to add -" + ex.ToString());
                }

            }
            else
            {
                MessageBox.Show("It is mandatory to enter all the relevant details");
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
                if (result == MessageBoxResult.Yes)
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
                        MessageBox.Show("Failed to update - " + ex.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("Enter a model to update");
            }

        }

        /// <summary>
        /// sends drone to charge slot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendToCharge()
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
                    RefreshProperties();
                }
                catch (FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed sending drone to charge - " + ex.ToString());
                }
                catch (InputDoesNotExist ex)
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
        private void releaseDrone()
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
                    RefreshProperties();
                }
                catch (FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed releasing drone - " + ex.ToString());
                }
                catch (InputDoesNotExist ex)
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
        private void sendDroneToDeliver()
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
                    RefreshProperties();

                }
                catch (FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed to attribute - " + ex.ToString());
                }
                catch (InputDoesNotExist ex)
                {
                    MessageBox.Show("Failed to attribute - " + ex.ToString());
                }
                catch (Exception)
                {
                    MessageBox.Show("Error accourd");
                }
            }
        }

        /// <summary>
        /// func for the pickup button
        /// </summary>
        private void dronePickUp()
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
                    RefreshProperties();
                }
                catch (FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed to pick up - " + ex.ToString());
                }
                catch (InputDoesNotExist ex)
                {
                    MessageBox.Show("Failed to pick up - " + ex.ToString());
                }
                catch (InvalidInputException ex)
                {
                    MessageBox.Show("Failed to pick up - " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// func for the delivery button
        /// </summary>
        private void deliverParcel()
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
                    RefreshProperties();
                    if (SelectedDrone.ParcelInDelivery == null && SelectedDrone.IsDeleted)
                        Close();
                }
                catch (FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed to deliver - " + ex.ToString());
                }
                catch (InputDoesNotExist ex)
                {
                    MessageBox.Show("Failed to deliver - " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// button for the attributing, pickup, and delivery actions 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneActions_Click(object sender, RoutedEventArgs e)
        {
            //if the drone's status is avalable the button will show the attributing option
            if (SelectedDrone.DroneStatuses == DroneStatuses.Available)
                sendDroneToDeliver();
            //if the drone didnt pick up the parcel the button will show the pickup option
            else if (IsPickUpEnabled)
                dronePickUp();
            //if the drone picked up the parcel the button will show the delivery option
            else
                deliverParcel();
        }

        /// <summary>
        /// refreshes the binding
        /// </summary>
        private void RefreshProperties()
        {
            PropertyChanged(this, new PropertyChangedEventArgs("IsPickUpEnabled"));
            PropertyChanged(this, new PropertyChangedEventArgs("IsDeliverParcelEnabled"));
            PropertyChanged(this, new PropertyChangedEventArgs("IsReleaseDroneEnabled"));
        }

        /// <summary>
        /// button for charging and release drone 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneCharging_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDrone.DroneStatuses == DroneStatuses.Available)
                sendToCharge();
            else if (IsReleaseDroneEnabled)
                releaseDrone();
        }

        /// <summary>
        /// button to delete drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteDrone_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Delete drone?", "myApp", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                SelectedDrone.IsDeleted = true;
                try
                {
                    myBl.DeleteDrone(SelectedDrone);
                    OnUpdate();
                }
                catch (FailedToUpdateException ex)
                {
                    MessageBox.Show("Failed to delete -" + ex.ToString());
                }
                Close();
            }
        }

        /// <summary>
        /// opens the parcel that the drone is holding by a double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ParcelInDelivery p = parcelView.SelectedItem as ParcelInDelivery;
                if (p != null)
                {
                    Parcel par = new Parcel();
                    par = myBl.GetParcel(p.Id);//gets the selected station as station instead of station for list 
                    ParcelView parcelWindow = new ParcelView(myBl, par);
                    parcelWindow.Show();
                }
                else
                {
                    MessageBox.Show("Please select a parcel");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error occured");
            }
        }

        /// <summary>
        /// updates the drones window doring and according the simulator's work
        /// </summary>
        private void UpdateDroneWindow()
        {
            try
            {
                OnUpdate();//updates the list of drone
                SelectedDrone = myBl.GetDrone(SelectedDrone.Id);
                RefreshProperties();
                InitializeParcelInDrone();
            }
            catch(FailedToGetException ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show("Failed to update drone window - " + ex.ToString() + " " + ex.InnerException.ToString());
                else
                    MessageBox.Show("Failed to update drone window - " + ex.ToString());
            }
            catch(InputDoesNotExist ex)
            {
                MessageBox.Show("Failed to update drone window -" + ex.ToString());
            }
        }

        /// <summary>
        /// button to start the simulator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void automatic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsAutomaticMode = true;
                worker = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true, };
                worker.DoWork += (sender, args) => myBl.StartSimulatur((int)args.Argument, () => worker.ReportProgress(0), () => worker.CancellationPending);
                worker.ProgressChanged += (sender, args) => UpdateDroneWindow();
                worker.RunWorkerCompleted += (sender, args) => AutomaticCompleted();
                worker.RunWorkerAsync(SelectedDrone.Id);
            }
            catch(Exception)
            {
                MessageBox.Show("Failed running automatic mode");
            }
        }

        private void AutomaticCompleted()
        {
            IsAutomaticMode = false;
            MessageBox.Show("Process finished successfully!");
        }

        /// <summary>
        /// button to stop the simulator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopAutomatic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsAutomaticMode = false;
                worker.CancelAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed stoping automatic mode");
            }
        }
    }
}
