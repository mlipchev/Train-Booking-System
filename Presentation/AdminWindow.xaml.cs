//Created by Martin Lipchev 
//Edinburgh Napier University
//Last modified: 03.12.2018
//Admin Window - Window to create and delete Trains

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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            //Hide Dates in the Past
            date_Picker.BlackoutDates.AddDatesInPast();
            //Select Today as Date
            date_Picker.SelectedDate = DateTime.Today;
            //Sources for listboxes
            lbx_Trains.ItemsSource = facade.Trains;
            lbx_Intermediates.ItemsSource = facade.Ints;
            //Populate Trains ListBox from Database
            facade.populateTrainsListFromDatabase();
            //Generate Times
            populateTimeBox();
        }

        //Instance of the facade to use it's methods
        Facade facade = Facade.Instance();


        //Reading from Fields and Adding a new Train
        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Create object and set properties
                Train train = new Train();
                setTrainType(train);
                train.Id = txbx_ID.Text;
                train.Departure = (String)cmbx_Departure.SelectedValue;
                train.Destination = (String)cmbx_Destination.SelectedValue;
                if (radio_firstclassYes.IsChecked == true) { train.FirstClass = true; } else { train.FirstClass = false; }

                //Set Intermediate
                List<string> list = lbx_Intermediates.Items.OfType<string>().ToList();
                string intermediates = String.Join(",", list.ToArray());
                train.Intermediate = intermediates;

                //Set DateTime
                String str = date_Picker.SelectedDate.Value.ToString();
                if (cmbx_Time.SelectedIndex == -1) { throw new Exception("Time can not be empty!"); }
                train.DepartureDateTime = DateTime.Parse(str.Replace("00:00:00", cmbx_Time.SelectedValue.ToString()));
                
                //Add to Facade
                facade.addTrain(train);

                //Clear fields if successful
                if (facade.Trains.Contains(train))
                {
                    clearFields();
                }

                //and Refresh ListBox with Trains
                facade.populateTrainsListFromDatabase();
            }
            //Catch Exceptions
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Helper Method to set Train Type from Radio Buttons
        public void setTrainType(Train tr)
        {
            if (radio_Express.IsChecked == true)
            {
                tr.TrainType = Train.TypeOfTrain.Express;
            }else if(radio_Sleeper.IsChecked == true)
            {
                tr.TrainType = Train.TypeOfTrain.Sleeper;
            }else if(radio_Stopping.IsChecked == true)
            {
                tr.TrainType = Train.TypeOfTrain.Stopping;
            }
            else
            {
                throw new Exception("Train type must be selected! ");
            }
        }

        //Event handler when radio button is checked
        private void radio_Checked (object sender, RoutedEventArgs e)
        {
            HideFieldsForType();
        }

        //Depending on Train Type, hide and unhide fields
        //And generate times accordingly
        public void HideFieldsForType()
        {
            if(radio_Express.IsChecked == true)
            {
                //For Express Trains
                cmbx_Intermediate.SelectedIndex = -1;
                cmbx_Intermediate.IsEnabled = false;
                facade.Ints.Clear();
                populateTimeBox();
            }
            else if(radio_Sleeper.IsChecked == true)
            {
                //For Sleeper Trains
                cmbx_Intermediate.SelectedIndex = -1;
                cmbx_Intermediate.IsEnabled = false;
                facade.Ints.Clear();
                //Sleeper Times
                populateSleeperTimeBox();
            }
            else if(radio_Stopping.IsChecked == true)
            {
                //For Stopping Trains
                cmbx_Intermediate.IsEnabled = true;
                cmbx_Intermediate.SelectedIndex = 0;
                populateTimeBox();
            }
        }


        //Method to Clear fields if Adding Successful
        public void clearFields()
        {
            txbx_ID.Clear();
            cmbx_Departure.SelectedIndex = -1;
            cmbx_Destination.SelectedIndex = -1;
            cmbx_Intermediate.SelectedIndex = -1;
            radio_firstclassNo.IsChecked = true;
            date_Picker.SelectedDate = DateTime.Today;
            cmbx_Time.SelectedIndex = -1;
            cmbx_Intermediate.SelectedIndex = -1;
            facade.Ints.Clear();
        }

        //Method to populate Fields when Double-Clicking on a Train in List Box
        public void populateFields(Train tr)
        {
            switch (tr.TrainType)
            {
                case Train.TypeOfTrain.Express:
                    radio_Express.IsChecked = true;
                    break;
                case Train.TypeOfTrain.Sleeper:
                    radio_Sleeper.IsChecked = true;
                    break;
                case Train.TypeOfTrain.Stopping:
                    radio_Stopping.IsChecked = true;
                    break;
            }
            facade.Ints.Clear();
            txbx_ID.Text = tr.Id;
            cmbx_Departure.Text = tr.Departure;
            cmbx_Destination.Text = tr.Destination;
            cmbx_Intermediate.Text = tr.Intermediate;
            radio_firstclassYes.IsChecked = tr.FirstClass;
            date_Picker.SelectedDate = tr.DepartureDateTime.Date;
            populateTime(tr.DepartureDateTime);

            foreach (string str in tr.Intermediate.Split(','))
            {
                facade.Ints.Add(str);
            }

        }

        //Helper method for populating Fields for Time
        public void populateTime(DateTime dt)
        {
            if(dt.Minute == 30)
            {
                string str = dt.Hour.ToString() + ":" + dt.Minute.ToString();
                if (dt.Hour == 0)
                {
                    str = dt.Hour.ToString() + "0:" + dt.Minute.ToString();
                    cmbx_Time.Text = str;
                    return;
                }
                cmbx_Time.Text = str;
                return;
            }
            else if(dt.Hour == 0)
            {
                string str = dt.Hour.ToString() + "0:" + dt.Minute.ToString() + "0";
                cmbx_Time.Text = str;
                return;
            }else{
                string str = dt.Hour.ToString() + ":" + dt.Minute.ToString() + "0";
                cmbx_Time.Text = str;
            }
        }

        //Method to Populate ComboBox with the Times for Sleeper Trains
        public void populateSleeperTimeBox()
        {
            LinkedList<String> sleepertimes = new LinkedList<String>();
            sleepertimes.Clear();
            for (int i = 21; i <= 24; i++)
            {
                string hour = i.ToString();
                for (int j = 0; j <= 30; j += 30)
                {
                    string minute = j.ToString();
                    string minute1 = "30";
                    if (minute == "0")
                    {
                        minute1 = minute.Replace("0", "00");
                    }
                    if (hour == "24")
                    {
                        hour = "00";
                        minute1 = "00";
                    }
                    string toDisplay = String.Format("{0}:{1}", hour, minute1);
                    if (!sleepertimes.Contains(toDisplay))
                    {
                        sleepertimes.AddLast(toDisplay);
                    }
                }
            }
            cmbx_Time.ItemsSource = sleepertimes;
            cmbx_Time.SelectedIndex = -1;
        }


        //Method to Populate ComboBox with the Times for Normal Trains
        public void populateTimeBox()
        {
            LinkedList<String> times = new LinkedList<String>();
            times.Clear();
            for (int i = 6; i <= 24; i++)
            {
                string hour = i.ToString();
                for (int j = 0; j <= 30; j += 30)
                {
                    string minute = j.ToString();
                    string minute1 = "30";
                    if (minute == "0")
                    {
                        minute1 = minute.Replace("0", "00");
                    }
                    if (hour == "24")
                    {
                        hour = "00";
                        minute1 = "00";
                    }
                    string toDisplay = String.Format("{0}:{1}", hour, minute1);
                    if (!times.Contains(toDisplay))
                    {
                        times.AddLast(toDisplay);
                    }
                }
            }
            cmbx_Time.ItemsSource = times;
            cmbx_Time.SelectedIndex = -1;
        }

        //Close Current Window and go back to Main
        private void btn_Done_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        //Event for Double Click on a Train in ListBox
        private void lbx_Trains_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Train tr = (Train)lbx_Trains.SelectedItem;
            populateFields(tr);
        }


        //Test Button to add Trains
        private void btn_Test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Train tr = new Train();
                tr.Id = "1S45";
                tr.Departure = "London (Kings Cross)";
                tr.Destination = "Edinburgh (Waverley)";
                tr.TrainType = Train.TypeOfTrain.Express;
                tr.Intermediate = "";
                tr.DepartureDateTime = DateTime.Parse("01.12.2019 10:00");
                tr.Sleeper = false;
                tr.FirstClass = true;
                facade.addTrain(tr);
                Console.WriteLine(tr);

                Train tr1 = new Train();
                tr1.Id = "1E05";
                tr1.Departure = "Edinburgh (Waverley)";
                tr1.Destination = "London (Kings Cross)";
                //tr1.Intermediate = new List<string>();
                tr1.Intermediate = "Peterborough, Darlington, York";
                tr1.TrainType = Train.TypeOfTrain.Stopping;
                tr1.DepartureDateTime = DateTime.Parse("01.12.2019 12:00");
                tr1.Sleeper = false;
                tr1.FirstClass = true;
                facade.addTrain(tr1);

                Train tr2 = new Train();
                tr2.Id = "1E99";
                tr2.Departure = "Edinburgh (Waverley)";
                tr2.Destination = "London (Kings Cross)";
                tr2.TrainType = Train.TypeOfTrain.Sleeper;
                tr2.Intermediate = "";
                tr2.DepartureDateTime = DateTime.Parse("01.12.2019 21:30");
                tr2.Sleeper = true;
                tr2.FirstClass = false;
                facade.addTrain(tr2);
                facade.populateTrainsListFromDatabase();
            }
            catch
            {
                MessageBox.Show("Trains already added!");
            }
        }

        //Button to add Intermediates
        private void btn_AddIntermediate_Click(object sender, RoutedEventArgs e)
        {
            facade.Ints.Add((String)cmbx_Intermediate.SelectedValue);
            cmbx_Intermediate.SelectedIndex = -1;

        }

        //Button to remove Intermediates
        private void btn_RemoveIntermediate_Click(object sender, RoutedEventArgs e)
        {
            facade.Ints.Clear();
        }

        //Remove Train From ID
        private void btn_Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Train tr = facade.findTrain(txbx_RemoveID.Text);
                facade.removeTrain(tr);
            }catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        //Remove All Trains and Bookings from Database
        private void btn_RemoveAll_Click(object sender, RoutedEventArgs e)
        {
            facade.removeAllTrainsAndBookings();
        }
    }
}
