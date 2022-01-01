using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
    /// Interaction logic for StationListView.xaml
    /// </summary>
    public partial class StationListView : Window, INotifyPropertyChanged
    {
        private IBL myBl = BlFactory.GetBl();
        private List<StationForList> stations;
        private CollectionView collectionView;//collectionView for the grouping
        private bool isGroupingMode;//to notify if the grouping button in checked
        public event PropertyChangedEventHandler PropertyChanged= delegate { };
        //prop for binding to the grouping button
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
        //prop for the stations list - for binding to the items source in the xml
        public List<StationForList> Stations
        {
            get { return stations; }
            set
            {
                stations = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Stations"));
            }
        }

        /// <summary>
        /// c-tor 
        /// </summary>
        /// <param name="Bl">Ibl object</param>
        public StationListView(IBL Bl)
        {
            myBl = Bl;
            DataContext = this;
            InitializeComponent();
            GetStationListFromBL();
        }

        /// <summary>
        /// double click event that opens an update station window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StationForList s = StationsListView.SelectedItem as StationForList;
            Station st = new Station();
            st = myBl.GetStation(s.Id);//gets the selected station as station instead of station for list 
            StationView stationWindow = new StationView(myBl, st);
            stationWindow.OnUpdate += StationWindow_onUpdate;//registers to event that is announced when a station was added or updated 
            stationWindow.Show();
        }

        /// <summary>
        /// button to open an addstation window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stationAdd_Click(object sender, RoutedEventArgs e)
        {
            StationView stationWindow = new StationView(myBl);
            stationWindow.OnUpdate += StationWindow_onUpdate;
            stationWindow.Show();
        }

        /// <summary>
        /// func that refreshes the stations list
        /// </summary>
        private void StationWindow_onUpdate()
        {
            GetStationListFromBL();
        }

        /// <summary>
        /// button for closing the list view window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelAdd_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StationsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
            collectionView = (CollectionView)CollectionViewSource.GetDefaultView(StationsListView.ItemsSource);
            //describe how we want to make the groups
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("AvailableChargingSlots");//לשאול את בת שבע איך לקשר אותו לID של השולח
            collectionView.GroupDescriptions.Add(groupDescription);
        }

        /// <summary>
        /// func that gets the station list from the bl 
        /// </summary>
        private void GetStationListFromBL()
        {
            Stations = myBl.GetStationList().ToList();
            if (IsGroupingMode)
                GroupList();
        }
    }
}
