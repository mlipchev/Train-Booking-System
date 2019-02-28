//Created by Martin Lipchev 
//Edinburgh Napier University
//Last modified: 03.12.2018
//Main Window - Navigation Window

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            pswdField.Visibility = Visibility.Hidden;
        }

        //Instance of the Facade
        Facade facade = Facade.Instance();

        //Password
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (pswdField.Visibility == Visibility.Hidden)
            {
                pswdField.Visibility = Visibility.Visible;
            }
            if (pswdField.Text == "Password (1234):") { return; }
            else
            {
                if (pswdField.Text == "1234")
                {
                    AdminWindow win = new AdminWindow();
                    win.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect password!");
                    pswdField.Clear();
                }
            }
        }

        //Clear password field when clicked
        private void pswdField_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            pswdField.Clear();
        }

        //Open New Window
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UserWindow win = new UserWindow();
            win.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AdvancedSearch win = new AdvancedSearch();
            win.Show();
            this.Close();
        }
    }
}
