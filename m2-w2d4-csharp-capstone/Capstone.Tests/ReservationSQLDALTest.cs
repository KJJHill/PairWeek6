using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.Models;
using Capstone.DAL;
using System.Configuration;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationSQLDALTest
    {
        private TransactionScope tran;
        string connectionString = @"Data Source=DESKTOP-ICT08NO\sqlexpress;Initial Catalog = ParkReservationSystem; Integrated Security = True";

        private string SQL_InsertReservation =
        @"if (@siteId in (SELECT distinct site.site_id FROM campground
                
                JOIN site ON site.campground_id = campground.campground_id
                JOIN reservation ON reservation.site_id = site.site_id
                
                WHERE @reservationFromMonth BETWEEN (open_from_mm) AND (open_to_mm) 
                AND @reservationToMonth BETWEEN (open_from_mm) AND (open_to_mm)
                
                AND(from_date NOT BETWEEN @fromDate AND @toDate)
                AND(to_date NOT BETWEEN @fromDate AND @toDate)

                AND((@fromDate NOT BETWEEN (from_date) AND(to_date))
                AND(@toDate NOT BETWEEN (from_date) AND(to_date)))))

                INSERT INTO reservation (site_id, name, from_date, to_date)
                VALUES (@siteId, @name, @fromDate, @toDate)";

        /*
        * INITIALIZE
        *
        */
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
        public void AddNewReservation()
        {
            Reservation myReservationTest = new Reservation();
            myReservationTest.CampgroundId = 1;
            myReservationTest.FromDate = new DateTime(2017, 02, 20);
            myReservationTest.ToDate = new DateTime(2017, 03, 01);
            myReservationTest.SiteId = 9;
            myReservationTest.Name = "Test Johnson";

            ReservationSQLDAL mySQLTest = new ReservationSQLDAL(connectionString);
            myReservationTest.ReservationId = mySQLTest.AddNewReservation(myReservationTest);

        }

        [TestMethod]
        public void GetAllReservations()
        {
            List<Reservation> myReservationsTest = new List<Reservation>();

            ReservationSQLDAL mySQLTest = new ReservationSQLDAL(connectionString);

            myReservationsTest = mySQLTest.GetAllReservations();

        }
    }
}

