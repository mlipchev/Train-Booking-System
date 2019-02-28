//Created by Martin Lipchev 
//Edinburgh Napier University
//Last modified: 03.12.2018
//Booking Window - Window to create and delete Bookings

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
    /// Interaction logic for BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        //Keep an instance of train, facade, and booking
        Train train;
        Facade facade;
        Booking booking;

        public BookingWindow(string id)
        {
            InitializeComponent();
            this.facade = Facade.Instance();
            //Parse Train from previous window
            this.train = facade.findTrain(id);
            //Source for passengers ListBox
            lbx_Passengers.ItemsSource = facade.Bookings;
            //Populate ListBox from Database
            facade.populateBookingsListFromDatabase(train);
            //Show current selected train
            lbx_Train.Items.Add(train);
            txbx_TrainID.Text = train.Id;
            //Disable buttons
            txbx_TrainID.IsEnabled = false;
            btn_Add.IsEnabled = false;
            //Populate cabin and seat comboboxes
            populateCabin();
            populateSeat();
        }

        //Event when Add Button is clicked - Add to Database
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                facade.addBooking(this.booking);
                facade.populateBookingsListFromDatabase(train);
                clearFields();
                btn_Add.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Clear Fields if successful
        private void clearFields()
        {
            txbx_Name.Clear();
            cmbx_Departure.SelectedIndex = -1;
            cmbx_Arrival.SelectedIndex = -1;
            cmbx_Coach.SelectedIndex = -1;
            cmbx_Seat.SelectedIndex = -1;
            radio_cabinNo.IsChecked = true;
            radio_firstclassNo.IsChecked = true;
            txt_Price.Text = "0";
            cmbx_Coach.SelectedIndex = 0;
            cmbx_Seat.SelectedIndex = 0;
        }

        //Create Booking object and call Calculator to show price
        private void btn_Calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Creating Booking
                Booking booking = new Booking();
                booking.Name = txbx_Name.Text;
                booking.Train = train;
                booking.Departure = (String)cmbx_Departure.SelectedValue;
                booking.Arrival = (String)cmbx_Arrival.SelectedValue;
                booking.Coach = (String)cmbx_Coach.SelectedValue;
                booking.Seat = (Int32)cmbx_Seat.SelectedValue;
                setFirstClass(booking, train);
                setCabin(booking, train);

                //Checking everything is correct and calculating price
                facade.checkBookingExists(booking);
                checkDepartArrive(booking);
                txt_Price.Text = (facade.FareCalculator(train, booking)).ToString();
                this.booking = booking;
                btn_Add.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //Method to Validate Departure and Arrival Stations:
        //Only stopping trains can have intermediates
        //Tickets can only be booked from stations which are intermediates or First/Last
        //Tickets can only be booked in the direction of the train
        private void checkDepartArrive(Booking book)
        {
            btn_Add.IsEnabled = false;
            txt_Price.Text = "";

            //Check for Train Type 
            if(train.TrainType != Train.TypeOfTrain.Stopping)
            {
                if(book.Departure != train.Departure || book.Arrival != train.Destination)
                {
                    throw new Exception("Only Stopping trains can be booked from intermediate stations");
                }
            }
            else
            //Check if Selected Station is in Intermediates or Start/End
            {
                List<string> intermediates = facade.getIntermediatesList(train);
                if (intermediates.Contains(book.Departure) || book.Departure == train.Departure)
                {
                    if(intermediates.Contains(book.Arrival) || book.Arrival == train.Destination)
                    {
                    }
                    else
                    {
                        throw new Exception("Stopping trains can only be booked from end or intermediate stations");
                    }
                }
                else
                {
                    throw new Exception("Stopping trains can only be booked from end or intermediate stations");
                }
            }
            //Checking for Direction
            int depart = cmbx_Departure.SelectedIndex;
            int arrive = cmbx_Arrival.SelectedIndex;

            if (this.train.Departure == "Edinburgh (Waverley)")
            {
                if (depart > arrive || depart == arrive)
                {
                    throw new Exception("Arrival station can not be before Departure or the same");

                }
            }
            if(this.train.Departure == "London (Kings Cross)")
            {
                if (arrive > depart || depart==arrive)
                {
                    throw new Exception("Departure station can not be before Arrival or the same");
                }
            }
        }

        //Check if Radio button first class is checked and Validate
        private void setFirstClass(Booking bkng, Train tr)
        {

            if (radio_firstclassYes.IsChecked == true)
            {
                if (tr.FirstClass == true)
                {
                    bkng.FirstClass = true;
                }
                else
                {
                    throw new Exception("First Class can only be selected on First Class trains!");
                }
            }
            else
            {
                bkng.FirstClass = false;
            }
        }

        //Check if Radio button cabin is checked and Validate
        private void setCabin(Booking bkng, Train tr)
        {

            if (radio_cabinYes.IsChecked == true)
            {
                if (tr.TrainType == Train.TypeOfTrain.Sleeper)
                {
                    bkng.Cabin = true;
                }
                else
                {
                    radio_cabinNo.IsChecked = true;
                    throw new Exception("Cabin can only be selected on Sleeper trains!");
                }
            }
            else
            {
                bkng.Cabin = false;
            }
        }

        //Populate seat ComboBox
        private void populateSeat()
        {
            LinkedList<int> seat = new LinkedList<int>();
            for (int i = 1; i <= 60; i++)
            {
                seat.AddLast(i);
            }
            cmbx_Seat.ItemsSource = seat;
        }

        //Populate Cabin ComboBox
        private void populateCabin()
        {
            List<string> list = new List<string>();
            list.Add("A");
            list.Add("B");
            list.Add("C");
            list.Add("D");
            list.Add("E");
            list.Add("F");
            list.Add("G");
            list.Add("H");
            cmbx_Coach.ItemsSource = list;
        }

        //Go back to previous window
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            UserWindow userwin = new UserWindow();
            userwin.Show();
            this.Close();
        }

        //Remove Booking by name
        private void btn_Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Booking book = facade.findBooking(txbx_RemoveName.Text);
                facade.removeBooking(book);
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        //Set textbox for removing to Selected Booking's ID
        private void lbx_Passengers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Booking book = (Booking)lbx_Passengers.SelectedItem;
            txbx_RemoveName.Text = book.Name;
        }
    }
}
