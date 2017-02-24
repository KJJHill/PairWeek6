using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.Models;
using Capstone.DAL;



namespace Capstone.Tests
{
    [TestClass]
    public class CampsiteSQLDALTest
    {
        private TransactionScope tran;
        string connectionString = @"Data Source=DESKTOP-ICT08NO\sqlexpress;Initial Catalog = ParkReservationSystem; Integrated Security = True";

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();
        }

        /*
        * CLEANUP
        * Rollback the existing transaction.
        */
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetCampsitesByParkId()
        {
            List<Campsite> myListOfReturnedCampsites = new List<Campsite>();

            CampsiteSQLDAL mySQLTest = new CampsiteSQLDAL(connectionString, false);

            myListOfReturnedCampsites = mySQLTest.GetCampsitesByParkId(1);
            
        }

        [TestMethod]
        public void GetCampsitesByParkAndCampground()
        {
            List<Campsite> myListOfReturnedCampsites = new List<Campsite>();

            CampsiteSQLDAL mySQLTest = new CampsiteSQLDAL(connectionString, false);

            myListOfReturnedCampsites = mySQLTest.GetCampsitesByParkIdAndCampgroundId(1, 3);

        }
    }
}

