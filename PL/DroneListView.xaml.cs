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

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListView.xaml
    /// </summary>
    public partial class DroneListView : Window
    {
        private  IBL myBl = BlFactory.GetBl();
        private Weight? selectedWeight = null;
        private DroneStatuses? selectedStatus = null;
        private List<DroneForList> drones;

        /// <summary>
        /// prop to bind to for filtering list by weight of drone
        /// </summary>
        public Weight? SelectedWeight
        {
            get { return selectedWeight; }
            set
            {
                selectedWeight = value;
                FilterList();
            }
        }

        /// <summary>
        /// prop to bind to for filtering list by status of drone
        /// </summary>
        public DroneStatuses? SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                FilterList();
            }
        }

        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="Bl"></param>
        public DroneListView(IBL Bl)
        {
            myBl = Bl;
            DataContext = this;
            InitializeComponent();
            GetDroneListFromBL();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.Weight));
        }

        /// <summary>
        /// filters the drones list by status and by weight
        /// </summary>
        private void FilterList()
        {
            if (SelectedWeight != null && SelectedStatus != null)
                DronesListView.ItemsSource = drones.Where(dr => dr.Weight == selectedWeight && dr.DroneStatuses == SelectedStatus);
            else if (selectedWeight != null)
                DronesListView.ItemsSource = drones.Where(dr => dr.Weight == selectedWeight);
            else
                DronesListView.ItemsSource = drones.Where(dr => dr.DroneStatuses == SelectedStatus);
        }
        
        /// <summary>
        /// gets the drones list from bl
        /// </summary>
        private void GetDroneListFromBL()
        {
            drones = myBl.GetDroneList().ToList();
            DronesListView.ItemsSource = drones;
            if (SelectedWeight != null || SelectedStatus != null)
                FilterList();
        }

        /// <summary>
        /// button that opens a adding drone window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void droneAdd_Click(object sender, RoutedEventArgs e)
        {
            DroneView droneWindow = new DroneView(myBl);
            droneWindow.OnUpdate += DroneWindow_onUpdate;
            droneWindow.Show();
        }

        /// <summary>
        /// event that opens a drones window by a mouse double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneForList d = DronesListView.SelectedItem as DroneForList;
            Drone dr = new Drone();
            dr = myBl.GetDrone(d.Id);//gets the selected drone as drone instead of drone for list 
            DroneView droneWindow = new DroneView(myBl, dr);
            droneWindow.OnUpdate += DroneWindow_onUpdate;
            droneWindow.Show();
        }

        /// <summary>
        /// function that updates the drone list window every time a drone was updated or added
        /// </summary>
        private void DroneWindow_onUpdate()
        {
            GetDroneListFromBL();
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// button to close the drone list view window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelAdd_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// button to undo the filter on the drones list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undoFilter_Click(object sender, RoutedEventArgs e)
        {
            drones = myBl.GetDroneList().ToList();
        }
    }
}
