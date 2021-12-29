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

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelListView.xaml
    /// </summary>
    public partial class ParcelListView : Window
    {
        private IBL myBl = BlFactory.GetBl();
        private Weight? selectedWeight = null;
        private List<ParcelForList> parcels;

        public ParcelListView()
        {
            InitializeComponent();
        }
    }
}
