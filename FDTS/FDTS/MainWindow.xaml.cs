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
using System.Threading;

namespace FDMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        LiveData Page1 = new LiveData();
        Database Page2 = new Database();
        bool connectionOn = false;

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;

        }

        private void ButtonLiveData_Click(object sender, RoutedEventArgs e)
        {
            StarterText.Visibility = Visibility.Collapsed;
            Main.Content = Page1;
        }

        private void ButtonDatabase_Click(object sender, RoutedEventArgs e)
        {
            StarterText.Visibility = Visibility.Collapsed;
            Main.Content = Page2;
        }

        private void ConnectionPower_Click(object sender, RoutedEventArgs e)
        {

            if (connectionOn == false)
            {
                ConnectionButton.Background = Brushes.Green;
                connectionOn = true;
                Page1.server.notClosed = true;
                Page1.runServer();

            }
            else
            {
                ConnectionButton.Background = Brushes.Red;
                connectionOn = false;
                Page1.server.closeSocket();
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
