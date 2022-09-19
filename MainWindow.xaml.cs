using Hardcodet.Wpf.TaskbarNotification;
using Hardcodet.Wpf.TaskbarNotification.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ITried
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        ServerHandler server; // сервер
        ObservableCollection<ClientHandler> clients = new ObservableCollection<ClientHandler>();
        List<string> Applications = new List<string>();
        Thread listenThread; // поток для прослушивания
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindingOperations.EnableCollectionSynchronization(clients,new Object());

            Applications = File.ReadAllLines("ExeConfig.txt").ToList();
            Listapps.ItemsSource = Applications;
            ObservableCollection<ClientHandler> clients1 = new ObservableCollection<ClientHandler>();
            
            DB.ItemsSource = clients;
            try
            {
                server = new ServerHandler(this.clients) ;
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {

                server.Disconnect();
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {



                   List<ClientHandler> clientschecked = clients.Where(c => c.Checked == true).ToList();
                   for (int i = 0; i < clientschecked.Count(); i++)
                   {
                            server.ShutdownClient(clientschecked[i].Id);

                   }
                
            
        }

       

       

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            server.Disconnect();
            TaskBar.Dispose();
            this.Close();
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            TaskBar.Visibility = Visibility.Visible;
            this.Hide();
        }

      

      

        private void High_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton==MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void TaskbarIcon_MouseDown(object sender, RoutedEventArgs e)
        {
            Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Listapps.SelectedItem!=null)
            {
                List<ClientHandler> clientschecked = clients.Where(c => c.Checked == true).ToList();
                for (int i = 0; i < clientschecked.Count(); i++)
                {
                    server.BroadcastMessage(Listapps.SelectedItem.ToString(), clientschecked[i].Id);

                }
            }
            if (Listapps.SelectedItem == null)
            {
                MessageBox.Show("Choice Application to start");
            }
        }
        
    }
}
