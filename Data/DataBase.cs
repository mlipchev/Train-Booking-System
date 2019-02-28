//Created by Martin Lipchev 
//Edinburgh Napier University
//Last modified: 03.12.2018
//Database Class - Class to handle database access and methods
//Design Patterns - Singleton

using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.ObjectModel;

namespace Data
{
    public class DataBase
    {

        //Creating a Connection to the database and pointing to the .mdf file location
        private SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DataBase.mdf;Integrated Security = True; Connect Timeout = 200");

        //Singleton
        private static DataBase instance;
        //Store an instance of the facade for ease
        Facade facade = Facade.Instance();

        //Singleton - Private Constructor
        private DataBase()
        {
        }

        //Can only be accessed using .instance
        public static DataBase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataBase();
                }
                return instance;
            }
        }

        //METHODS FOR INSERTING INTO THE DATABASE

        //Insert a Train into the database
        public void insertTrain(Train train)
        {
            con.Open();

            string id = train.Id;
            string departure = train.Departure;
            string destination = train.Destination;
            string type = train.TrainType.ToString();
            string intermediate = train.Intermediate; 
            string dateTime = train.DepartureDateTime.ToString();
            bool sleeper = train.Sleeper;
            bool firstclass = train.FirstClass;

            string sqla = String.Format(@"insert into Train(id, departure, destination, type, intermediate, depart_datetime, sleeper, firstclass) values(@id, @departure, @destination, @type, @intermediate, @dateTime, @sleeper, @firstclass)");

            SqlCommand com = new SqlCommand(sqla, con);

            using (com)
            {
                com.Parameters.AddWithValue("@id", id);
                com.Parameters.AddWithValue("@departure", departure);
                com.Parameters.AddWithValue("@destination", destination);
                com.Parameters.AddWithValue("@type", type);
                com.Parameters.AddWithValue("@intermediate", intermediate);
                com.Parameters.AddWithValue("@dateTime", dateTime);
                com.Parameters.AddWithValue("@sleeper", sleeper);
                com.Parameters.AddWithValue("@firstclass", firstclass);
            }
            com.ExecuteNonQuery();

            con.Close();
        }

        //Insert a Booking into the Database
        public void insertBooking(Booking booking)
        {
            con.Open();

            string name = booking.Name;
            string train_id = booking.Train.Id;
            string departure = booking.Departure;
            string arrival = booking.Arrival;
            bool firstclass = booking.FirstClass;
            bool cabin = booking.Cabin;
            string coach = booking.Coach;
            int seat = booking.Seat;

            string sqla = String.Format(@"insert into Booking(name, train_id, departure, arrival, firstclass, cabin, coach, seat) values(@name, @train_id, @departure, @arrival, @firstclass, @cabin, @coach, @seat)");

            SqlCommand com = new SqlCommand(sqla, con);

            using (com)
            {
                com.Parameters.AddWithValue("@name", name);
                com.Parameters.AddWithValue("@train_id", train_id);
                com.Parameters.AddWithValue("@departure", departure);
                com.Parameters.AddWithValue("@arrival", arrival);
                com.Parameters.AddWithValue("@firstclass", firstclass);
                com.Parameters.AddWithValue("@cabin", cabin);
                com.Parameters.AddWithValue("@coach", coach);
                com.Parameters.AddWithValue("@seat", seat);
            }
            com.ExecuteNonQuery();

            con.Close();
        }

        //METHODS FOR GETTING FROM THE DATABASE


        public List<Train> getAllTrains()
         {
            con.Open();

            string sql = String.Format(@"select * from Train");

            SqlCommand com = new SqlCommand(sql, con);


            List<Train> trains = new List<Train>();

            SqlDataReader sdr = com.ExecuteReader();

            while (sdr.Read())
            {
                try
                {
                    Train tr = new Train();

                    tr.Id = (string)sdr["id"];
                    tr.Departure = (string)sdr["departure"];
                    tr.Destination = (string)sdr["destination"];
                    tr.TrainType = getTrainType((string)sdr["type"]);
                    tr.Intermediate = (string)sdr["intermediate"];

                    DateTime result;
                    DateTime.TryParse((string)sdr["depart_datetime"], out result);
                    tr.DepartureDateTime = result;

                    tr.Sleeper = (Boolean)sdr["sleeper"];
                    tr.FirstClass = (Boolean)sdr["firstclass"];

                    trains.Add(tr);
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            con.Close();
            return trains;
        }

        //Helper method to get the type of train
        public Train.TypeOfTrain getTrainType(string str)
        {
            switch (str)
            {
                case "Express":
                    return Train.TypeOfTrain.Express;
                case "Sleeper":
                    return Train.TypeOfTrain.Sleeper;
                case "Stopping":
                    return Train.TypeOfTrain.Stopping;
            }
            return Train.TypeOfTrain.Express;
        }

        //Get all Bookings from the Database
        public List<Booking> getAllBookings()
        {
            con.Open();

            string sql = String.Format(@"select * from Booking");

            SqlCommand com = new SqlCommand(sql, con);

            List<Booking> bookings = new List<Booking>();

            SqlDataReader sdr = com.ExecuteReader();

            while (sdr.Read())
            {
                try
                {
                    Booking book = new Booking();

                    book.Name = (string)sdr["name"];
                    book.Train.Id = (string)sdr["train_id"];
                    book.Departure = (string)sdr["departure"];
                    book.Arrival = (string)sdr["arrival"];
                    book.FirstClass = (bool)sdr["firstclass"];
                    book.Cabin = (bool)sdr["cabin"];
                    book.Coach = (string)sdr["coach"];
                    book.Seat = (int)sdr["seat"];

                    bookings.Add(book);
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            con.Close();
            return bookings;
        }

        //Get a Specific Booking from the Database via Train_ID
        public List<Booking> getCertainBookings(string train_id)
        {
            con.Open();

            string sql = String.Format(@"select * from Booking where train_id = '{0}'", train_id);

            SqlCommand com = new SqlCommand(sql, con);

            List<Booking> bookings = new List<Booking>();

            SqlDataReader sdr = com.ExecuteReader();

            while (sdr.Read())
            {
                try
                {
                    Booking book = new Booking();

                    book.Name = (string)sdr["name"];
                    book.Train = facade.findTrain((string)sdr["train_id"]);
                    book.Departure = (string)sdr["departure"];
                    book.Arrival = (string)sdr["arrival"];
                    book.FirstClass = (bool)sdr["firstclass"];
                    book.Cabin = (bool)sdr["cabin"];
                    book.Coach = (string)sdr["coach"];
                    book.Seat = (int)sdr["seat"];

                    bookings.Add(book);
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }
            con.Close();
            return bookings;
        }


        //METHODS FOR REMOVING FROM THE DATABASE

        //Delete a Certain Train via Train ID
        public void removeTrain(string id)
        {
            con.Open();

            string sql = String.Format(@"delete from Train where Train.id = @id");

            SqlCommand com = new SqlCommand(sql, con);

            using (com)
            {
                com.Parameters.AddWithValue("@id", id);
            }

            com.ExecuteNonQuery();
            con.Close();
        }

        //Delete all Trains from the Database
        public void removeAllTrains()
        {
            con.Open();

            string sql = String.Format(@"delete from Train");

            SqlCommand com = new SqlCommand(sql, con);

            SqlDataReader sdr = com.ExecuteReader();

            while (sdr.Read())
            {

            }

            con.Close();
        }


        //Delete All Bookings from the database
        public void removeAllBookings()
        {
            con.Open();

            string sql = String.Format(@"delete from Booking");

            SqlCommand com = new SqlCommand(sql, con);

            SqlDataReader sdr = com.ExecuteReader();

            while (sdr.Read())
            {

            }

            con.Close();
        }

        //Delete All Bookings of a Specific Person via Name
        public void removeBooking(string name)
        {
            con.Open();

            string sql = String.Format(@"delete from Booking where Booking.name = @name");

            SqlCommand com = new SqlCommand(sql, con);

            using (com)
            {

                com.Parameters.AddWithValue("@name", name);
            }

            com.ExecuteNonQuery();
            con.Close();
        }

    }
}
