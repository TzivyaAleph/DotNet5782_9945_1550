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
using BO;
using BlApi;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationView.xaml
    /// </summary>
    public partial class StationView : Window, INotifyPropertyChanged
    {
        private IBL myBl;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };//event to tell us when a property was changed- so we know to refresh the binding
        public event Action OnUpdate = delegate { }; //event that will refresh the station list every time we update a station
        private Station stationToAdd; //the station we're adding to the stations list
        private Station selectedStation;
        private string originalStationName; //temp to hold the station's name of the station from the list view window (this will not be used for items source)
        private int originalNumOfSlots; //temp to hold the station's Num Of Slots of the station from the list view window (this will not be used for items source)
        /// <summary>
        /// to know wich window to open: update or add
        /// </summary>
        public bool IsUpdateMode { get; set; } 
        /// <summary>
        /// for binding
        /// </summary>
        public int TotalNumOfSlots { get; set; }
        private List<DroneCharge> droneCharges;

        /// <summary>
        /// drone charges list for the listbox of drone charging in the station
        /// </summary>
        public List<DroneCharge> DroneCharges
        {
            get { return droneCharges; }
            set
            {
                droneCharges = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DroneCharges"));
            }
        }

        /// <summary>
        /// a station for putting the input data we recieve in it.
        /// </summary>
        public Station StationToAdd
        {
            get { return stationToAdd; }
            set
            {
                stationToAdd = value;
                PropertyChanged(this, new PropertyChangedEventArgs("StationToAdd"));
            }
        }

        /// <summary>
        /// a station for updating the station's data
        /// </summary>
        public Station SelectedStation
        {
            get { return selectedStation; }
            set
            {
                selectedStation = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedStation"));
            }
        }

        /// <summary>
        /// c-tor for the add station grid
        /// </summary>
        /// <param name="bl"></param>
        public StationView(IBL bl)
        {
            try
            {
                InitializeComponent();
                myBl = bl;
                DataContext = this;//binding the data
                stationToAdd = new Station();
                stationToAdd.StationLocation = new();
            }
            catch (FailedToGetException ex)
            {
                MessageBox.Show("Failed to add!!" + ex.ToString());
            }
        }

        /// <summary>
        /// c-tor for the update station grid
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="st"></param>
        public StationView(IBL bl, Station st)
        {
            try
            {
                InitializeComponent();
                DataContext = this;//binding the data
                SelectedStation = st;
                DroneCharges = SelectedStation.DroneCharges;
                originalStationName = st.Name;
                originalNumOfSlots = st.ChargeSlots;
                myBl = bl;
                IsUpdateMode = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to open the update window!");
            }

        }

        /// <summary>
        /// button for adding a station, sends a station to the bl add station function
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
                    myBl.AddStation(StationToAdd);
                    res = MessageBox.Show("Added succecfully!!");
                    if (res != MessageBoxResult.None)
                        OnUpdate();//updates the list of stations in the previous window.
                    Close();
                }
            }
            catch (FailedToAddException ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show("Failed To Add - "+ex.ToString() + " " + ex.InnerException.ToString());
                }
                else
                    MessageBox.Show("Failed To Add - "+ex.ToString());
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show("Failed to add -" + ex.ToString());
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
        /// button for updating the stations name and number of available charging slots
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (originalStationName != SelectedStation.Name)
            {
                var result = MessageBox.Show("Save updates to current station?", "myApp", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        myBl.UpdateStation(SelectedStation.Id, SelectedStation.Name, TotalNumOfSlots);
                        var res = MessageBox.Show("success");
                        if (res != MessageBoxResult.None)
                        {
                            SelectedStation = myBl.GetStation(SelectedStation.Id);
                            OnUpdate();
                        }
                    }
                    catch (FailedToUpdateException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            MessageBox.Show("Failed To update - " + ex.ToString() + " " + ex.InnerException.ToString());
                        }
                        else
                            MessageBox.Show("Failed To update - " + ex.ToString());
                    }
                    catch(InputDoesNotExist ex)
                    {
                        MessageBox.Show("Failed to update - " + ex.ToString());
                    }
                    catch (InvalidInputException ex)
                    {
                        MessageBox.Show("Failed to update - " + ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// double click event for opening a drone window of a drone that is charging in station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dronesInStation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DroneCharge d = dronesInStation.SelectedItem as DroneCharge;
                Drone dr = new Drone();
                if(d!=null)
                {
                    dr = myBl.GetDrone(d.Id);//gets the selected drone as drone instead of drone for list 
                    DroneView droneWindow = new DroneView(myBl, dr);
                    droneWindow.Show();
                }
                else
                {
                    MessageBox.Show("Please select a station!");
                }
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
        /// button to delete station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Delete station?", "myApp", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                SelectedStation.IsDeleted = true;
                try
                {
                    myBl.DeleteStation(SelectedStation);
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
