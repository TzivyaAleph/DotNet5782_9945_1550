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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListView.xaml
    /// </summary>
    public partial class DroneListView : Window
    {
        public DroneListView(BL.IBL Bl)
        {
            InitializeComponent();
            BL.IBL myBl = Bl;
        }

        //public DroneListView(BL.IBL Bl)
        //{
        //    BL.IBL myBl = Bl;
        //}
    }
}
