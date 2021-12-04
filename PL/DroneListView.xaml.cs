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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListView.xaml
    /// </summary>
    public partial class DroneListView : Window
    {
        private BL.IBL myBl;
        
        public DroneListView(BL.IBL Bl)
        {
            InitializeComponent();
            myBl = Bl;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.Weight));
            this.DronesListView.ItemsSource = myBl.GetDroneList();
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Weight droneWeight = new();
            droneWeight = (Weight)WeightSelector.SelectedItem;
            this.DronesListView.ItemsSource = myBl.GetDroneList(d => d.Weight == droneWeight);
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatuses droneStatuse =(DroneStatuses) StatusSelector.SelectedItem;
            this.DronesListView.ItemsSource = myBl.GetDroneList(d => d.DroneStatuses == droneStatuse);
        }

        private void droneAdd_Click(object sender, RoutedEventArgs e)
        {
            DroneView droneWindow = new DroneView(myBl);
            droneWindow.Show();
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            try
            {
                if (DronesListView.SelectedItem == null)
                    return;
                DroneForList dr = DronesListView.SelectedItem as DroneForList;
                DroneView droneWindow = new DroneView(myBl);
                droneWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
