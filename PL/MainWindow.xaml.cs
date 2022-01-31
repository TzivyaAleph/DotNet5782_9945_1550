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
        private IBL myBl;

        /// <summary>
        /// c-tor
        /// </summary>
        /// <param name="bl"></param>
        public MainWindow(IBL bl)
        {
            try
            {
                myBl = bl;
                InitializeComponent();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed opening the main window");
            }
        }

        /// <summary>
        /// button to open the drones' list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void droneListViewBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DroneListView droneListWindow = new DroneListView(myBl);
                droneListWindow.Show();
            }
            catch(Exception)
            {
                MessageBox.Show("Failed loading the drones' list window");
            }
        }

        /// <summary>
        /// button to open the parcels' list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelListViewBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParcelListView parcelListView = new ParcelListView(myBl);
                parcelListView.Show();
            }           
            catch (Exception)
            {
                MessageBox.Show("Failed loading the parcels' list window");
            }
        }

        /// <summary>
        /// button to open the customers' list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customerListViewBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerListView customerListView = new CustomerListView(myBl);
                customerListView.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed loading the customers' list window");
            }
        }

        /// <summary>
        ///  button to open the stations' list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stationListViewBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StationListView stationListView = new StationListView(myBl);
                stationListView.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed loading the stations' list window");
            }
        }

        /// <summary>
        /// button to close the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
