using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SSL_Fix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            progressBar.Visibility = Visibility.Hidden;
        }
        private async void Run_Button_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            //check if tcp/ip connection is true
            bool tcpResponse;
            tcpResponse = await Task.Run(() => tcpConnect());

            //bool bothFail = (!httpsResponse && !tcpResponse); //check if both are false
            SSL.checkSslExists();//run SSL Cert check
            bool sslKeyExist = checkSslKey();

            //if (bothFail) //if both endpoints fail
            if (!tcpResponse)//if port 80 fails show message
            {
                /*FixButton.Visibility = 0;
                StatusButton.Visibility = 0;
                StatusButton.Background = Brushes.Red;
                StatusButton.Foreground = Brushes.White;
                StatusButton.Content = "Failed! \nPlease see below";*/

                //HTTPSFailResults.Visibility = 0;
                //HTTPSFailResults.Text = "Https connection Failed - Check Firewall/network configurations";
                TCPFailResults.Visibility = 0;
                TCPFailResults.Text = " TCP connection on PORT 80 Failed - Check Firewall/network configurations\n Contact appropriate vendor for support";
                //Grid.SetRow(HTTPSFailResults, 1);
                Grid.SetRow(TCPFailResults, 2);

                progressBar.Visibility = Visibility.Hidden;
            }
            if (!sslKeyExist)
            {
                FixButton.Visibility = 0;
                StatusButton.Visibility = 0;
                StatusButton.Background = Brushes.Red;
                StatusButton.Foreground = Brushes.White;
                StatusButton.Content = "Failed! \nPlease see below";

                SSLKeyFailResults.Visibility = 0;
                SSLKeyFailResults.Text = "SSL.com Root Certificate not found!";
                Grid.SetRow(SSLKeyFailResults, 1);

                progressBar.Visibility = Visibility.Hidden;
            }
            if (sslKeyExist)
            {
                ClearButton.Visibility = 0;
                StatusButton.Visibility = 0;
                StatusButton.Background = Brushes.Green;
                StatusButton.Foreground = Brushes.White;
                StatusButton.Content = "SSL Cert Exists!";

                progressBar.Visibility = Visibility.Hidden;
            }
            /*if (httpsResponse && tcpResponse) //if both endpoints succeed
            {
                StatusButton.Visibility = 0;
                StatusButton.Background = Brushes.Green;
                StatusButton.Foreground = Brushes.FloralWhite;
                StatusButton.Content = "!--PASSED--!";
                ClearButton.Visibility = 0;
                //FixButton.Visibility = 0;
                progressBar.Visibility = Visibility.Hidden;
            }
            if (!httpsResponse && !bothFail) //if https fails
            {
                StatusButton.Visibility = 0;
                StatusButton.Background = Brushes.Red;
                StatusButton.Foreground = Brushes.White;
                StatusButton.Content = "Failed! \nPlease see info below";

                Grid.SetRow(HTTPSFailResults, 1);
                HTTPSFailResults.Visibility = 0;
                HTTPSFailResults.Text = "Https connection Failed - Check Firewall/network configurations";
                ClearButton.Visibility = 0;
                progressBar.Visibility = Visibility.Hidden;
            }*/
            /*if (!tcpResponse && !bothFail) //if tcp/ip fails
            {
                StatusButton.Visibility = 0;
                StatusButton.Background = Brushes.Red;
                StatusButton.Foreground = Brushes.White;
                StatusButton.Content = "Failed! \nPlease see info below";

                Grid.SetRow(TCPFailResults, 1);
                TCPFailResults.Visibility = 0;
                TCPFailResults.Text = "TCP connection Failed - Check Firewall/network configurations";
                ClearButton.Visibility = 0;
                progressBar.Visibility = Visibility.Hidden;
            }*/
            //progressBar.Visibility = Visibility.Hidden;
        }
        private void Fix_Button_Click(object sender, RoutedEventArgs e)
        {
            FixButton.Visibility = (Visibility)1;
            HTTPSFailResults.Visibility = (Visibility)1;
            TCPFailResults.Visibility = (Visibility)1;
            SSL.InstallCertificate();
            bool sslInstall = checkSslInstall();
            StatusButton.Visibility = 0;
            SSLKeyFailResults.Visibility = (Visibility)1;

            if (sslInstall)
            {
                StatusButton.Background = Brushes.Green;
                StatusButton.Foreground = Brushes.White;
                StatusButton.Content = "SSL Cert Installed!";
            }
            else
            {
                StatusButton.Background = Brushes.IndianRed;
                StatusButton.Foreground = Brushes.White;
                StatusButton.Content = "Unable to install Cert\n Please run as Admin";
            } 
            CloseButton.Visibility = 0;
        }
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            StatusButton.Visibility = (Visibility)1;
            ClearButton.Visibility = (Visibility)1;
            HTTPSFailResults.Visibility = (Visibility)1;
            TCPFailResults.Visibility = (Visibility)1; ;
        }
        public static bool tcpConnect()
        {
            bool tcpResponse;
            var appAddress = "download.windowsupdate.com";
            //bool tcpResponse;

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                s.Connect(appAddress, 80);
                tcpResponse = true;
                Console.WriteLine("Connection to {0} succeeded", appAddress);
                s.Dispose();
            }
            catch (SocketException)
            {
                tcpResponse = false;
                Console.WriteLine("Connection to {0} failed", appAddress);
                s.Dispose();
            }
            return tcpResponse;
        }
        public static bool checkSslInstall()
        {
            bool sslResponse;
            sslResponse = SSL.sslInstallStatus;
            return sslResponse;
        }
        public static bool checkSslKey()
        {
            bool sslKey;
            sslKey = SSL.sslKeyExists;
            return sslKey;
        }
    } 
}
