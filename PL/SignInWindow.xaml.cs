using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for SighnInWindow.xaml
    /// </summary>
    public partial class SighnInWindow : Window, INotifyPropertyChanged
    {
        private static readonly IBL myBl = BlFactory.GetBl();
        private bool isManagerChecked;
        private bool isCustomerChecked;
        public CustomersType CustomerType { get; set; }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private int? idForSignIn;

        public int? IdForSignIn
        {
            get { return idForSignIn; }
            set
            { 
                idForSignIn = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IdForSignIn"));
            }
        }

        public string Password { get; set; }
        public bool IsUserTypeChosen { get; set; }

        /// <summary>
        /// prop to bind the customer radio button
        /// </summary>
        public bool IsCustomerChecked
        {
            get { return isCustomerChecked; }
            set
            { 
                isCustomerChecked = value;
                IsUserTypeChosen = true;
                if (isCustomerChecked)
                    CustomerType = CustomersType.Customer;
                PropertyChanged(this, new PropertyChangedEventArgs("IsCustomerChecked"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsUserTypeChosen"));
            }
        }

        /// <summary>
        /// prop to bind to the manager radio button
        /// </summary>
        public bool IsManagerChecked
        {
            get { return isManagerChecked; }
            set
            {
                isManagerChecked = value;
                IsUserTypeChosen = true;
                if (isManagerChecked)
                    CustomerType = CustomersType.Manager;
                PropertyChanged(this, new PropertyChangedEventArgs("IsManagerChecked"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsUserTypeChosen"));
            }
        }

        /// <summary>
        /// c-tor
        /// </summary>
        public SighnInWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// button for adding a new customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerView newCustomer = new CustomerView(myBl);
            newCustomer.Show();
        }

        private void signIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var signedIn = myBl.GetCustomer((int)IdForSignIn);
                if (CustomerType != signedIn.CustomerType)
                {
                    MessageBox.Show("ERROR! USER TYPE IS NOT VALID");
                    return;
                }
                if (signedIn.Password != Password)
                {
                    MessageBox.Show("PASSWORD IS NOT VALID!!");
                    return;
                }
                if(isManagerChecked)
                {
                    MainWindow mainWindow = new MainWindow(myBl);
                    mainWindow.Show();
                }
                else
                {
                    CustomerView customerView = new CustomerView(myBl, signedIn);
                    customerView.Show();
                }
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch(FailedToGetException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// button to close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = password.Password;
            PropertyChanged(this,new PropertyChangedEventArgs("Password"));
        }
    }
}
