using System;
using Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessTests
{
    [TestClass]
    public class TrainTests
    {
        private Train train = new Train();



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

            string expected = "ID: 1S45. Departure: London (Kings Cross). Destination: Edinburgh (Waverley). Intermediate: . Departure: 01/12/2019 10:00:00. First Class: True. Type of Train: Express";

            //Act
            string test = Train.ToString();

            //Assert
            Assert.AreEqual(expected, test, "Train object setting does not function properly");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "ID can not be empty!")]
        public void TestMethod2()
        {
            Train Train1 = new Train();
            Train1.Id = "";
            Train1.Departure = "London (Kings Cross)";
            Train1.Destination = "Edinburgh (Waverley)";
            Train1.TrainType = Train.TypeOfTrain.Express;
            Train1.Intermediate = "";
            Train1.DepartureDateTime = DateTime.Parse("01.12.2019 10:00");
            Train1.Sleeper = false;
            Train1.FirstClass = true;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Departure can not be empty!")]
        public void TestMethod3()
        {
            Train Train1 = new Train();
            Train1.Id = "1S45";
            Train1.Departure = "";
            Train1.Destination = "Edinburgh (Waverley)";
            Train1.TrainType = Train.TypeOfTrain.Express;
            Train1.Intermediate = "";
            Train1.DepartureDateTime = DateTime.Parse("01.12.2019 10:00");
            Train1.Sleeper = false;
            Train1.FirstClass = true;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Destination can not be empty!")]
        public void TestMethod4()
        {
            Train Train1 = new Train();
            Train1.Id = "1S45";
            Train1.Departure = "London(Kings Cross)";
            Train1.Destination = "";
            Train1.TrainType = Train.TypeOfTrain.Express;
            Train1.Intermediate = "";
            Train1.DepartureDateTime = DateTime.Parse("01.12.2019 10:00");
            Train1.Sleeper = false;
            Train1.FirstClass = true;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Stopping trains need to have an Intermediate")]
        public void TestMethod5()
        {
            Train Train1 = new Train();
            Train1.Id = "1S45";
            Train1.Departure = "London (Kings Cross)";
            Train1.Destination = "Edinburgh (Waverley)";
            Train1.TrainType = Train.TypeOfTrain.Stopping;
            Train1.Intermediate = "";
            Train1.DepartureDateTime = DateTime.Parse("01.12.2019 10:00");
            Train1.Sleeper = false;
            Train1.FirstClass = true;
        }


        [TestMethod]
        [ExpectedException(typeof(FormatException), "String was not recognized as a valid DateTime.")]
        public void TestMethod6()
        {
            Train Train1 = new Train();
            Train1.Id = "1S45";
            Train1.Departure = "London (Kings Cross)";
            Train1.Destination = "Edinburgh (Waverley)";
            Train1.TrainType = Train.TypeOfTrain.Express;
            Train1.Intermediate = "";
            Train1.DepartureDateTime = DateTime.Parse("");
            Train1.Sleeper = false;
            Train1.FirstClass = true;
        }

    }
}
