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
    /// Interaction logic for ParcelListView.xaml
    /// </summary>
    public partial class ParcelListView : Window, INotifyPropertyChanged
    {
        private IBL myBl;
        private Weight? selectedWeight = null;
        private Status? selectedStatus = null;
        private Priority? selectedPriority = null;
        private List<ParcelForList> parcels;
        public event PropertyChangedEventHandler PropertyChanged= delegate { };
        private CollectionView collectionView;//collectionView for the grouping
        private bool isGroupingMode;

        /// <summary>
        /// prop for filtering list by weight of parcel
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
        /// prop to bind to for filtering list by status of parcel
        /// </summary>
        public Status? SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value;
                FilterList();
            }
        }

        /// <summary>
        /// prop to bind to for filtering list by priority of parcel
        /// </summary>
        public Priority? SelectedPriority
        {
            get { return selectedPriority; }
            set
            {
                selectedPriority = value;
                FilterList();
            }
        }

        /// <summary>
        /// prop for the grouping button binding
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
        /// prop for the parcels list were binding to
        /// </summary>
        public List<ParcelForList> Parcels
        {
            get { return parcels; }
            set 
            { 
                parcels = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Parcels"));
            }
        }

        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="Bl"></param>
        public ParcelListView(IBL Bl)
        {
            try
            {
                myBl = Bl;
                DataContext = this;
                InitializeComponent();
                GetParcelListFromBL();
                StatusSelector.ItemsSource = Enum.GetValues(typeof(Status));
                WeightSelector.ItemsSource = Enum.GetValues(typeof(Weight));
                PrioritySelector.ItemsSource = Enum.GetValues(typeof(Priority));
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to open parcel's list");
            }

        }

        /// <summary>
        /// func that undoes the grouping of the list view
        /// </summary>
        private void UndoGrouping()
        {
            collectionView.GroupDescriptions.Clear();
        }

        /// <summary>
        /// func for grouping the parcels list view
        /// </summary>
        private void GroupList()
        {
            //puts the defult value that we choose in the collection view
            collectionView = (CollectionView)CollectionViewSource.GetDefaultView(parcelsList.ItemsSource);
            //describe how we want to make the groups
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Sender");
            if(!collectionView.GroupDescriptions.Contains(groupDescription))
            {
                collectionView.GroupDescriptions.Add(groupDescription);
            }
        }

        /// <summary>
        /// func that gets the parcels list from the bl 
        /// </summary>
        private void GetParcelListFromBL()
        {
            try
            {
                Parcels = myBl.GetParcelList().ToList();
            }
            catch(InputDoesNotExist ex)
            {
                MessageBox.Show("Failed to get list - "+ ex.ToString());
            }
            if (SelectedWeight != null || SelectedStatus != null || SelectedPriority != null)
                FilterList();
            if (IsGroupingMode)
                GroupList();
        }

        /// <summary>
        /// button that closses the parcel list view window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// filters the list of parcels by the priority, weight and status of the parcels
        /// </summary>
        private void FilterList()
        {
            try
            {
                //if all three filters are selected
                if (SelectedWeight != null && SelectedStatus != null && SelectedPriority != null)
                    parcelsList.ItemsSource = parcels.Where(p => p.Weight == SelectedWeight && p.Status == SelectedStatus && p.Priority == SelectedPriority);
                else if (SelectedWeight != null && SelectedStatus != null)
                    parcelsList.ItemsSource = parcels.Where(p => p.Weight == SelectedWeight && p.Status == SelectedStatus);
                else if (SelectedWeight != null && SelectedPriority != null)
                    parcelsList.ItemsSource = parcels.Where(p => p.Weight == SelectedWeight && p.Priority == SelectedPriority);
                else if (SelectedStatus != null && SelectedPriority != null)
                    parcelsList.ItemsSource = parcels.Where(p => p.Status == SelectedStatus && p.Priority == SelectedPriority);
                else if (selectedWeight != null)
                    parcelsList.ItemsSource = parcels.Where(p => p.Weight == SelectedWeight);
                else if (SelectedPriority != null)
                    parcelsList.ItemsSource = parcels.Where(p => p.Priority == SelectedPriority);
                else
                    parcelsList.ItemsSource = parcels.Where(p => p.Status == SelectedStatus);
                if (IsGroupingMode)
                    GroupList();
            }
            catch(Exception)
            {
                MessageBox.Show("Filter Failed!!");
            }
        }

        /// <summary>
        /// button for undoing the list filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undoFilterButton_Click(object sender, RoutedEventArgs e)
        {
            parcelsList.ItemsSource = parcels;
            if (IsGroupingMode)
                GroupList();
        }

        /// <summary>
        /// event that opens a parcels window by a mouse double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parcelsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ParcelForList p = parcelsList.SelectedItem as ParcelForList;
                //checks if click on parcel
                if (p != null)
                {
                    Parcel par = new Parcel();
                    par = myBl.GetParcel(p.Id);//gets the selected station as station instead of station for list 
                    ParcelView parcelWindow = new ParcelView(myBl, par);
                    parcelWindow.OnUpdate += ParcelView_onUpdate;//registers to event that is announced when a station was added or updated 
                    parcelWindow.Show();
                }
                else
                {
                    MessageBox.Show("Please select a parcel!");
                }
            }
            catch (FailedToGetException ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show("Failed to show parcel data - " + ex.ToString() + " " + ex.InnerException.ToString());
                else
                    MessageBox.Show("Failed to show parcel data - " + ex.ToString());
            }

        }

        /// <summary>
        /// func that refreshes the stations list
        /// </summary>
        private void ParcelView_onUpdate()
        {
            GetParcelListFromBL();
        }

        /// <summary>
        /// button for adding a parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParcelView parcelWindow = new ParcelView(myBl);
                parcelWindow.OnUpdate += ParcelView_onUpdate;//registers to event that is announced when a station was added or updated 
                parcelWindow.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading the adding window");
            }
        }

 
    }
}
