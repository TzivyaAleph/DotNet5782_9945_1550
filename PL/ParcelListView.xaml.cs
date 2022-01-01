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
        private IBL myBl = BlFactory.GetBl();
        private Weight? selectedWeight = null;
        private List<ParcelForList> parcels;
        public event PropertyChangedEventHandler PropertyChanged= delegate { };
        private CollectionView collectionView;//collectionView for the grouping
        private bool isGroupingMode;

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
            myBl = Bl;
            DataContext = this;
            InitializeComponent();
            GetParcelListFromBL();
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
            collectionView.GroupDescriptions.Add(groupDescription);
        }

        /// <summary>
        /// func that gets the parcels list from the bl 
        /// </summary>
        private void GetParcelListFromBL()
        {
            Parcels = myBl.GetParcelList().ToList();
            if (IsGroupingMode)
                GroupList();
        }
    }
}
