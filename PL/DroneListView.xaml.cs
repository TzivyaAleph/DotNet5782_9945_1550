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
    public partial class DroneListView : Window, INotifyPropertyChanged
    {
        private  IBL myBl;
        private Weight? selectedWeight = null;
        private DroneStatuses? selectedStatus = null;
        private List<DroneForList> drones;
        private CollectionView collectionView;//collectionView for the grouping
        private bool isGroupingMode;//to notify if the grouping button in checked
        public event PropertyChangedEventHandler PropertyChanged= delegate { };

        /// <summary>
        /// prop for the drones list - for binding to the items source in the xml
        /// </summary>
        public List<DroneForList> Drones 
        {
            get { return drones; }
            set
            {
                drones = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Drones"));
            }
        }

        /// <summary>
        /// prop for binding to the grouping button
        /// </summary>
        public bool IsGroupingMode
        {
            get { return isGroupingMode; }
            set
            {
                isGroupingMode = value;
                if (isGroupingMode)
                    GroupList();
                else
                    UndoGrouping();
            }
        }

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
            try
            {
                myBl = Bl;
                DataContext = this;
                InitializeComponent();
                GetDroneListFromBL();
                StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
                WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            }
            catch (Exception)
            {
                MessageBox.Show("Failed opening drones' list");
            }
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
            if (IsGroupingMode)
                GroupList();
        }
        
        /// <summary>
        /// gets the drones list from bl
        /// </summary>
        private void GetDroneListFromBL()
        {
            Drones = myBl.GetDroneList(d => !d.IsDeleted || d.ParcelId != 0).ToList();
            DronesListView.ItemsSource = drones;
            if (SelectedWeight != null || SelectedStatus != null)
                FilterList();
            if (IsGroupingMode)
                GroupList();
        }

        /// <summary>
        /// button that opens a adding drone window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void droneAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DroneView droneWindow = new DroneView(myBl);
                droneWindow.OnUpdate += DroneWindow_onUpdate;
                droneWindow.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading the adding window");
            }
        }

        /// <summary>
        /// event that opens a drones window by a mouse double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //only click on a drone
            try
            {
                DroneForList d = DronesListView.SelectedItem as DroneForList;
                Drone dr = new Drone();
                if (d != null)
                {
                    dr = myBl.GetDrone(d.Id);//gets the selected drone as drone instead of drone for list 
                    DroneView droneWindow = new DroneView(myBl, dr);
                    droneWindow.OnUpdate += DroneWindow_onUpdate;
                    droneWindow.Show();
                }
                else
                {
                    MessageBox.Show("Please select a drone!");
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Error occured");
            }
        }

        /// <summary>
        /// function that updates the drone list window every time a drone was updated or added
        /// </summary>
        private void DroneWindow_onUpdate()
        {
            GetDroneListFromBL();
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

        /// <summary>
        /// button to undo the filter on the drones list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undoFilter_Click(object sender, RoutedEventArgs e)
        {
            DronesListView.ItemsSource = drones;
            if (IsGroupingMode)
                GroupList();
        }

        /// <summary>
        /// func that undoes the grouping of the list view
        /// </summary>
        private void UndoGrouping()
        {
            collectionView.GroupDescriptions.Clear();
        }

        /// <summary>
        /// func for grouping the stations list view
        /// </summary>
        private void GroupList()
        {
            //puts the defult value that we choose in the collection view
            collectionView = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            //describe how we want to make the groups
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("DroneStatuses");
            collectionView.GroupDescriptions.Add(groupDescription);
        }

        private void groupingButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
