//Created by Martin Lipchev 
//Edinburgh Napier University
//Last modified: 03.12.2018
//Train Class - Create and Validate Trains

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Train
    {
        //Enum used to store the type of train
        public enum TypeOfTrain
        {
            Stopping,
            Express,
            Sleeper
        }

        //Properties
        private string id;
        private string departure;
        private string destination;
        private string intermediate;
        private DateTime departureDateTime;
        private bool firstClass;
        private bool sleeper;
        private TypeOfTrain typeOfTrain;

        public Train()
        {
        }

        //Accessors for Train ID - can't be blank
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("ID can not be empty!");
                }
                id = value;
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
                if (value == destination)
                {
                    throw new Exception("Departure can not be the same as Destination!");
                }
                departure = value;
            }
        }

        //Accessors for Destination Station - can't be blank
        public string Destination
        {
            get
            {
                return destination;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Destination can not be empty!");
                }
                if (value == departure)
                {
                    throw new Exception("Destination can not be the same as departure!");
                }
                destination = value;
            }
        }

        //Accessors for Intermediate Stops - can't be blank
        //Stopping trains need to have intermediate
        //Only stopping trains can have intermediates
        public string Intermediate
        {
            get
            {
                return intermediate;
            }
            set
            {
                if(TrainType == Train.TypeOfTrain.Stopping && value == "")
                {
                    throw new Exception("Stopping trains need to have an Intermediate");
                }
                intermediate = value;
            }
        }

        //Accessors for the Time and Date of Departure - can't be blank
        //Validation for dates in the past happens in Presentation
        public DateTime DepartureDateTime
        {
            get
            {
                return departureDateTime;
            }
            set
            {
                if(value == null && value.TimeOfDay.ToString() == "0:00:00")
                {
                    throw new Exception("Date can not be empty!");
                }
                departureDateTime = value;
            }
        }

        //Accessors for Type of Train
        public TypeOfTrain TrainType
        {
            get
            {
                return typeOfTrain;
            }
            set
            {
                typeOfTrain = value;
            }
        }

        //Accessors for is First Class selected - can't be blank
        public bool FirstClass { get => firstClass; set => firstClass = value; }

        //Accessors for is Sleeper Cabin available
        public bool Sleeper { get => sleeper; set => sleeper = value; }

        //To String Method to display Train Details
        public override string ToString()
        {
            return "ID: " + id + ". Departure: " + departure + ". Destination: " + destination + ". Intermediate: " + Intermediate +
            ". Departure: " + departureDateTime + ". First Class: " + firstClass + ". Type of Train: " + typeOfTrain;
        }
    }
}
 