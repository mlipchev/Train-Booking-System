//Created by Martin Lipchev 
//Edinburgh Napier University
//Last modified: 03.12.2018
//Advanced Search Window - Window used for advanced search functionality

using Business;
using Data;
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
    /// Interaction logic for AdvancedSearch.xaml
    /// </summary>
    public partial class AdvancedSearch : Window
    {

        public AdvancedSearch()
        {
            InitializeComponent();
        }

        //Instance of the facade to use it's methods
        Facade facade = Facade.Instance();

        //Take inputs from comboboxes and run Advanced Search method from the Facade
        private void btn_Done_Click(object sender, RoutedEventArgs e)
        {
            List<Train> trains = new List<Train>();
            trains = facade.AdvancedSearch(cmbx_Departure.Text, cmbx_Destination.Text);
            lbx_Trains.ItemsSource = trains;

        }

        private void btn_Done_Copy_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }
    }
}
