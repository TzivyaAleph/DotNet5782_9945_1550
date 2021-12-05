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
using IBL.BO;
using IBL;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneView.xaml
    /// </summary>
    public partial class DroneView : Window
    {
        private BL.IBL myBl;
        public DroneView(BL.IBL bl)
        {
            myBl = bl;
            InitializeComponent();
            gridUpdateDrone.Visibility = Visibility.Hidden;
        }

        public DroneView(BL.IBL bl, DroneForList dr)
        {
            myBl = bl;
            InitializeComponent();
            GridNewDrone.Visibility = Visibility.Hidden;
            
            //this.droneProperties.ItemsSource = (System.Collections.IEnumerable)dr;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddNewDronFromPage()
        {
            DroneForList newDrone = new DroneForList();
            newDrone.Id = int.Parse(txtID.Text);
            newDrone.Model = txtModel.Text;
            newDrone.Weight = (Weight)int.Parse(CBXWeight.Text);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Weight droneWeight = new();
            droneWeight = (Weight)CBXWeight.SelectedItem;
        }



        //private void InitDronFromPage()
        //{
        //    //var newDrone = new Drone();
        //    //txtID.Text = newDrone.Id ;
        //    //newDrone.Model = txtModel.Text;
        //    ////newDrone.Weight = txtID.Text;
        //    //newDrone.Battery = double.Parse(txtID.Text);
        //}


    }
}
