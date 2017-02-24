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
    public class CampgroundSQLDALTEST
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=DESKTOP-4EOMNFH\sqlexpress;Initial Catalog=ParkReservationSystem;Integrated Security=True";
        private int numberOfAvailableCampgrounds = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand("SELECT COUNT(*) FROM campground WHERE park_id = '1';", connection);
                numberOfAvailableCampgrounds = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand(@"SELECT COUNT(*) FROM campground 
                                     JOIN site ON site.campground_id = campground.campground_id 
                                     WHERE park.park_id = 1
                                     AND 02 BETWEEN(open_from_mm) AND(open_to_mm)
                                     AND 03 BETWEEN(open_from_mm) and(open_to_mm)
									 AND site.site_id NOT IN (SELECT site_id FROM reservation 
									 WHERE (from_date BETWEEN('2017-2-20') AND ('2017-03-01'))
                                     OR (to_date BETWEEN('2017-2-20') AND ('2017-03-01'))
                                     OR (('2017-2-20') BETWEEN from_date AND to_date) 
                                     OR(('2017-03-01') BETWEEN from_date AND to_date));", connection);
                cmd.ExecuteNonQuery();

            }

        }

      
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

       
        [TestMethod()]
        public void TestGetAllCampgroundsParkID()
        {
            //Arrange
            CampgroundSQLDAL campgroundDAL = new CampgroundSQLDAL(connectionString);

            //Act
            int campgroundCount = campgroundDAL.GetAllCampgroundsParkID(1).Count;

            //Assert
            Assert.IsNotNull(campgroundCount);
            Assert.AreEqual(numberOfAvailableCampgrounds, campgroundCount);
        }

        [TestMethod()]
        public void TestGetAllCampgroundsParkIDDate()
        {
            //Arrange
            CampgroundSQLDAL campgroundDAL = new CampgroundSQLDAL(connectionString);

            //Act
            int campgroundCount = campgroundDAL.GetAllCampgroundsParkID(1).Count;

            //Assert
            Assert.IsNotNull(campgroundCount);
            Assert.AreEqual(numberOfAvailableCampgrounds, campgroundCount);
        }
    }
}
