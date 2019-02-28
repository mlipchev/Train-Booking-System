using System;
using Business;
using Data;
using BusinessTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessTests
{
    [TestClass]
    public class BookingTest
    {
        TrainTests trts = new TrainTests();

        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            Train Train = new Train();
            Train.Id = "1S45";
            Train.Departure = "London (Kings Cross)";
            Train.Destination = "Edinburgh (Waverley)";
            Train.TrainType = Train.TypeOfTrain.Express;
            Train.Intermediate = "";
            Train.DepartureDateTime = DateTime.Parse("01.12.2019 10:00");
            Train.Sleeper = false;
            Train.FirstClass = true;

            Booking booking = new Booking();
            booking.Name = "Martin";
            booking.Train = Train;
            booking.Departure = "Edinburgh (Waverley)";
            booking.Arrival = "London (Kings Cross)";
            booking.FirstClass = true;
            booking.Cabin = false;
            booking.Coach = "A";
            booking.Seat = 1;

            string expected = "Train ID: 1S45. Name: Martin. Coach: A. Seat: 1. Departure: Edinburgh (Waverley). Destination: London (Kings Cross). First Class: True. Cabin: False";

            //Act
            string test = booking.ToString();

            //Assert
            Assert.AreEqual(expected, test, "Booking object setting does not function properly");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Name can not be empty!")]
        public void TestMethod2()
        {
            Booking booking = new Booking();
            booking.Name = "";
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Train can not be empty!")]
        public void TestMethod3()
        {
            Booking booking = new Booking();
            booking.Name = "Martin";
            booking.Train = null;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Departure can not be empty!")]
        public void TestMethod4()
        {
            Train Train = new Train();
            Train.Id = "1S45";
            Train.Departure = "London (Kings Cross)";
            Train.Destination = "Edinburgh (Waverley)";
            Train.TrainType = Train.TypeOfTrain.Express;
            Train.Intermediate = "";
            Train.DepartureDateTime = DateTime.Parse("01.12.2019 10:00");
            Train.Sleeper = false;
            Train.FirstClass = true;

            Booking booking = new Booking();
            booking.Name = "Martin";
            booking.Train = Train;
            booking.Departure = "";
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Arrival can not be empty!")]
        public void TestMethod5()
        {
            Train Train = new Train();
            Train.Id = "1S45";
            Train.Departure = "London (Kings Cross)";
            Train.Destination = "Edinburgh (Waverley)";
            Train.TrainType = Train.TypeOfTrain.Express;
            Train.Intermediate = "";
            Train.DepartureDateTime = DateTime.Parse("01.12.2019 10:00");
            Train.Sleeper = false;
            Train.FirstClass = true;

            Booking booking = new Booking();
            booking.Name = "Martin";
            booking.Train = Train;
            booking.Departure = "Edinburgh (Waverley)";
            booking.Arrival = "";
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Coach must be between A-H")]
        public void TestMethod6()
        {
            Train Train = new Train();
            Train.Id = "1S45";
            Train.Departure = "London (Kings Cross)";
            Train.Destination = "Edinburgh (Waverley)";
            Train.TrainType = Train.TypeOfTrain.Express;
            Train.Intermediate = "";
            Train.DepartureDateTime = DateTime.Parse("01.12.2019 10:00");
            Train.Sleeper = false;
            Train.FirstClass = true;

            Booking booking = new Booking();
            booking.Name = "Martin";
            booking.Train = Train;
            booking.Departure = "Edinburgh (Waverley)";
            booking.Arrival = "London (Kings Cross)";
            booking.Coach = "";
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Seat must be between 1-60")]
        public void TestMethod7()
        {
            Train Train = new Train();
            Train.Id = "1S45";
            Train.Departure = "London (Kings Cross)";
            Train.Destination = "Edinburgh (Waverley)";
            Train.TrainType = Train.TypeOfTrain.Express;
            Train.Intermediate = "";
            Train.DepartureDateTime = DateTime.Parse("01.12.2019 10:00");
            Train.Sleeper = false;
            Train.FirstClass = true;

            Booking booking = new Booking();
            booking.Name = "Martin";
            booking.Train = Train;
            booking.Departure = "Edinburgh (Waverley)";
            booking.Arrival = "London (Kings Cross)";
            booking.Coach = "A";
            booking.Seat = 0;
        }
    }
}
