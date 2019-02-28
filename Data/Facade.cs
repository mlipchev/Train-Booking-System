//Created by Martin Lipchev 
//Edinburgh Napier University
//Last modified: 03.12.2018
//Facade Class - Used to connect Business, Data and Presentation layers and provide methods 
//Design Patterns - Singleton, Facade

using Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Facade
    {
        //Binding List with trains for List Boxes
        private BindingList<Train> trains = new BindingList<Train>();
        //Binding List with bookings for List Boxes
        private BindingList<Booking> bookings = new BindingList<Booking>();
        //Binding List for intermediate stations for List Boxes
        BindingList<string> intermediates = new BindingList<string>();
        //Singleton
        private static Facade instance;
        //Keep an instance of Database for ease
        private static DataBase database;

        //Getters and Setters for Lists
        public BindingList<Train> Trains { get => trains; set => trains = value; }
        public BindingList<string> Ints { get => intermediates; set => intermediates = value; }
        public BindingList<Booking> Bookings { get => bookings; set => bookings = value; }


        //Singleton
        private Facade()
        { 
        }

        //Singleton
        public static Facade Instance()
        {
            if ( instance == null )
            {
                instance = new Facade();
                database = DataBase.Instance;
            }
            return instance;
        }

        //METHODS RELATED TO TRAINS

        //Method to add Train to Database
        public void addTrain(Train train)
        {
            foreach (Train tr in trains)
            {
                if(tr.Id == train.Id)
                {
                    throw new Exception("Train already exists!");
                }
            }
            database.insertTrain(train);
            trains.Add(train);
        }

        //Method to Find Specific Train via ID
        public Train findTrain(string id)
        {
            foreach( Train tr in trains)
            {
                if (id == tr.Id)
                {
                    return tr;
                }
            }
            throw new Exception("No such train!");
        }

        //Method which returns a List of all the Intermediates of a certain Train
        public List<String> getIntermediatesList(Train tr)
        {
            List<String> intermediates = new List<string>();
            foreach (string str in tr.Intermediate.Split(','))
            {
                intermediates.Add(str.Replace(" ",""));
            }
            return intermediates;
        }

        //Method to get all Trains from the Database
        public List<Train> getAllTrains
        {
            get
            {
                return database.getAllTrains();
            }
        }


        //METHODS RELATED TO BOOKINGS


        //Method To Add Booking to Database 
        public void addBooking(Booking booking)
        {
            foreach (Booking bkng in bookings)
            {
                if (bkng.Coach == booking.Coach && bkng.Seat == booking.Seat)
                {
                    throw new Exception("You can only book a seat once!");
                }
            }
            bookings.Add(booking);
            database.insertBooking(booking);
        }

        //Method To Get Bookings From Database for a Specific Train
        public List<Booking> getCertainBookings(Train tr)
        {
            return database.getCertainBookings(tr.Id);
        }

        //Method to Remove Train from Database
        public void removeTrain(Train train)
        {
            if (Trains.Contains(train))
            {
                Trains.Remove(train);
                database.removeTrain(train.Id);
            }
            else
            {
                throw new Exception("No such train in the system!");
            }
        }
        
        //Method To Remove Everything From Database
        public void removeAllTrainsAndBookings()
        {
            database.removeAllTrains();
            database.removeAllBookings();
            populateTrainsListFromDatabase();
        }

        //Method to Check if Booking Already Exists
        public void checkBookingExists(Booking booking)
        {
            foreach (Booking bkng in bookings)
            {
                if (bkng.Coach == booking.Coach && bkng.Seat == booking.Seat)
                {
                    throw new Exception("You can only book a seat once!");
                }
            }
        }

        //Method to find Booking for a specific person
        public Booking findBooking(string name)
        {
            foreach (Booking bkng in bookings)
            {
                if (name == bkng.Name)
                {
                    return bkng;
                }
            }
            throw new Exception("No such booking!");
        }

        //Method to Remove Booking from Database
        public void removeBooking(Booking bkng)
        {
            Booking b = this.findBooking(bkng.Name);
            if (b == bkng)
            {
                Bookings.Remove(bkng);
                database.removeBooking(bkng.Name);
            }
            else
            {
                throw new Exception("No such booking in the system");
            }
        }


        //HELPER METHODS


        //Used for calculating the price of the journey
        public int FareCalculator (Train tr, Booking book)
        {
            int price = 0;
            if ((book.Departure == "Edinburgh (Waverley)" || book.Departure == "London (Kings Cross)") &&
                (book.Arrival == "Edinburgh (Waverley)" || book.Arrival == "London (Kings Cross)"))
            {
                price += 50;
            }
            else
            {
                price += 25;
            }
            if (book.FirstClass)
            {
                price += 10;
            }
            if (tr.TrainType == Train.TypeOfTrain.Sleeper)
            {
                price += 10;
            }
            if (book.Cabin == true)
            {
                price += 20;
            }
            return price;
        }

        //Advanced Search method - finds all trains which run between any two points.
        public List<Train> AdvancedSearch(string dep, string dest)
        {
            //Get all Trains from the database
            List<Train> trains = getAllTrains;
            List<Train> sorted = new List<Train>();

            //Loop through all trains
            foreach(Train tr in trains)
            {
                //If looking for trains starting and ending from main stations
                if ((tr.Departure == dep && tr.Destination == dest) || (tr.Departure == dest && tr.Destination == dep))
                {
                    sorted.Add(tr);
                }
                //Trains from intermediate point to intermediate point
                else if (Facade.instance.getIntermediatesList(tr).Contains(dep) && Facade.instance.getIntermediatesList(tr).Contains(dest))
                {
                    sorted.Add(tr);
                }
                //Trains from intermediate point to main station
                else if (Facade.instance.getIntermediatesList(tr).Contains(dep) && tr.Destination == dest)
                {
                    sorted.Add(tr);
                }
                //Trains from main station to intermediate station
                else if (tr.Departure == dep && Facade.instance.getIntermediatesList(tr).Contains(dest))
                {
                    sorted.Add(tr);
                }
            }
            //return the list
            return sorted;
        }

        //Method to populate the Binding List of Trains from the Database
        public void populateTrainsListFromDatabase()
        {
            Trains.Clear();
            List<Train> trains = getAllTrains;
            foreach (Train tr in trains)
            {
                Trains.Add(tr);
            }
        }

        //Method to populate the Binding List of Bookings from the Database for a specific Train
        public void populateBookingsListFromDatabase(Train tr)
        {
            Bookings.Clear();
            List<Booking> bookings = getCertainBookings(tr);
            foreach (Booking book in bookings)
            {
                Bookings.Add(book);
            }
        }
    }
}
