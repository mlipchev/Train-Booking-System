//Created by Martin Lipchev 
//Edinburgh Napier University
//Last modified: 03.12.2018
//User Window - Window to Select a Train for which to create or delete bookings

using Data;
using Business;
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

namespace Presentation
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
            //Facade
            Facade facade = Facade.Instance();
            //Set Source for ListBox 
            lbx_Trains.ItemsSource = facade.Trains;
            //Populate Source for ListBox
            facade.populateTrainsListFromDatabase();
        }

        //Select Train and open Booking Window for it
        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Train tr = (Train)lbx_Trains.SelectedItem;
                BookingWindow bw = new BookingWindow(tr.Id);
                bw.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Please select a train!");
            }
        }

        //Go back to First Window
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
