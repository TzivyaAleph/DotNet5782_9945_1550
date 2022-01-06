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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlApi.IBL myBl = BlFactory.GetBl();
        public MainWindow(BlApi.IBL bl)
        {
            myBl = bl;
            InitializeComponent();
        }

        private void droneListViewBtn_Click(object sender, RoutedEventArgs e)
        {
            DroneListView droneListWindow = new DroneListView(myBl);
            droneListWindow.Show();
        }

        private void parcelListViewBtn_Click(object sender, RoutedEventArgs e)
        {
            ParcelListView parcelListView = new ParcelListView(myBl);
            parcelListView.Show();
        }

        private void customerListViewBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomerListView customerListView = new CustomerListView(myBl);
            customerListView.Show();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void stationListViewBtn_Click(object sender, RoutedEventArgs e)
        {
            StationListView stationListView = new StationListView(myBl);
            stationListView.Show();
        }
    }
}
