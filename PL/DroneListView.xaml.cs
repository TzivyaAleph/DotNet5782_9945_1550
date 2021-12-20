using System;
using System.Collections.Generic;
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
        private List<DroneForList> drones;

        public Weight? SelectedWeight
        {
            get { return selectedWeight; }
            set
            {
                selectedWeight = value;
                FilterList();
            }
        }

        private DroneStatuses? selectedStatus = null;

        public DroneStatuses? SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                FilterList();
            }
        }

        public DroneListView(IBL Bl)
        {
            myBl = Bl;
            DataContext = this;
            InitializeComponent();
            GetDroneListFromBL();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.Weight));
        }

        private void FilterList()
        {
            if (SelectedWeight != null && SelectedStatus != null)
                DronesListView.ItemsSource = drones.Where(dr => dr.Weight == selectedWeight && dr.DroneStatuses == SelectedStatus);
            else if (selectedWeight != null)
                DronesListView.ItemsSource = drones.Where(dr => dr.Weight == selectedWeight);
            else
                DronesListView.ItemsSource = drones.Where(dr => dr.DroneStatuses == SelectedStatus);
        }

        private void GetDroneListFromBL()
        {
            drones = myBl.GetDroneList().ToList();
            DronesListView.ItemsSource = drones;
        }





        private void droneAdd_Click(object sender, RoutedEventArgs e)
        {
            DroneView droneWindow = new DroneView(myBl);
            droneWindow.OnUpdate += DroneWindow_onUpdate;
            droneWindow.Show();
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneForList d = DronesListView.SelectedItem as DroneForList;
            Drone dr = new Drone();
            dr = myBl.GetDrone(d.Id);//gets the selected drone as drone instead of drone for list 
            DroneView droneWindow = new DroneView(myBl, dr);
            droneWindow.OnUpdate += DroneWindow_onUpdate;
            droneWindow.Show();
        }

        private void DroneWindow_onUpdate()
        {
            GetDroneListFromBL();
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
