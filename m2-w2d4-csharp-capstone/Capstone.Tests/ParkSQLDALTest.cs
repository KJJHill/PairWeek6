using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.Models;
namespace Capstone.Tests
{
    [TestClass]
    public class ParkSQLDALTEST
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=DESKTOP-4EOMNFH\sqlexpress;Initial Catalog=ParkReservationSystem;Integrated Security=True";
        private int numberOfParks = 0;
        int numberAllParkByDate = 0;
        int numberParksByDateOutPut = 0;


        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand("SELECT COUNT(*) FROM park; ", connection);
                numberOfParks = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand(@"SELECT COUNT(*) FROM park 
                                     JOIN campground ON park.park_id = campground.park_id
                                     JOIN site ON campground.campground_id = site.campground_id
                                     WHERE park.park_id = 1
                                     AND 02 BETWEEN(open_from_mm) AND(open_to_mm)
                                     AND 03 BETWEEN(open_from_mm) and(open_to_mm)
									 AND site.site_id NOT IN (SELECT site_id FROM reservation 
									 WHERE (from_date BETWEEN('2017-2-20') AND ('2017-03-01'))
                                     OR (to_date BETWEEN('2017-2-20') AND ('2017-03-01'))
                                     OR (('2017-2-20') BETWEEN from_date AND to_date) 
                                     OR(('2017-03-01') BETWEEN from_date AND to_date));", connection);

                numberParksByDateOutPut = (int)cmd.ExecuteScalar();
            }

        }


        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }


        [TestMethod()]
        public void GetAllParks()
        {
            //Arrange
            ParkSQLDAL parkDAL = new ParkSQLDAL(connectionString);

            //Act
            int parkCount = parkDAL.GetAllParksShortened().Count;

            //Assert
            Assert.IsNotNull(parkCount);
            Assert.AreEqual(numberOfParks, parkCount);
        }

        [TestMethod()]
        public void TestGetParksDate()
        {
            //Arrange
            ParkSQLDAL parkDAL = new ParkSQLDAL(connectionString);

            //Act
            int numberAllParkByDate = parkDAL.GetAllParksByDate(DateTime.Now, DateTime.Now, 1).Count;

            //Assert
            Assert.AreEqual(numberParksByDateOutPut, numberAllParkByDate);
        }
    }
}
