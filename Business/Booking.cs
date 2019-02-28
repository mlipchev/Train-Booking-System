//Created by Martin Lipchev 
//Edinburgh Napier University
//Last modified: 03.12.2018
//Booking Class - Create and Validate Bookings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Business
{
    public class Booking
    {
        //Booking Properties
        private string name;
        private Train train;
        private string departure;
        private string arrival;
        private bool firstClass;
        private bool cabin;
        private string coach;
        private int seat;

        public Booking()
        {

        }

        //Accessors for Name - can't be blank
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("Name can not be empty!");
                }
                name = value;
            }
        }

        //Accessors for Train - can't be null!
        //Store an instance of a train for which the booking is made
        public Train Train
        {
            get
            {
                return train;
            }
            set
            {
                if (value == null)
                {
                    throw new Exception("Train can not be empty!");
                }
                train = value;
            }
        }

        //Accessors for Departure Station - can't be blank
        public string Departure
        {
            get
            {
                return departure;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Departure can not be empty!");
                }
                departure = value;
            }
        }

        //Accessors for Arrival Station - can't be blank
        public string Arrival
        {
            get
            {
                return arrival;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Arrival can not be empty!");
                }
                arrival = value;
            }
        }

        //Accessors for First Class available - can't be blank
        public bool FirstClass
        {
            get
            {
                return firstClass;
            }
            set
            {
                if (value != true & value != false)
                {
                    throw new Exception("First Class has to be selected!");
                }
                firstClass = value;
            }
        }

        //Accessors for Cabin available - can't be blank
        public bool Cabin
        {
            get
            {
                return cabin;
            }
            set
            {
                if (value != true & value != false)
                {
                    throw new Exception("Cabin has to be selected!");
                }
                cabin = value;
            }
        }

        //Accessors for Coach - Must be between A and H
        public string Coach
        {
            get
            {
                return coach;
            }
            set
            {
                if (!Regex.IsMatch(value, @"[a-hA-H]"))
                {
                    throw new Exception("Coach must be between A-H");
                }
                coach = value;
            }
        }

        //Accessors for Seat number - Must be between 1 and 60
        public int Seat
        {
            get
            {
                return seat;
            }
            set
            {
                if(value < 1 || value > 60)
                {
                    throw new Exception("Seat must be between 1-60");
                }
                seat = value;
            }
        }

        //To String Method to display Booking Details
        public override string ToString()
        {
            return "Train ID: " + train.Id + ". Name: " + name + ". Coach: " + coach + ". Seat: " + seat + ". Departure: " + departure +
            ". Destination: " + arrival + ". First Class: " + firstClass + ". Cabin: " + cabin;
        }
    }
}
