using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationTest
    {
        [TestMethod]
        public void ReservationDisplayTest()
        {
            Reservation myReservation = new Reservation();
            myReservation.ReservationId = 1;
            myReservation.SiteId = 2;
            myReservation.CampgroundId = 4;
            myReservation.Name = "Johnson Family";
            myReservation.FromDate = DateTime.Today;
            myReservation.ToDate = DateTime.Today.AddDays(3);
            myReservation.CreateDate = DateTime.UtcNow;

            //Console.WriteLine(myReservation.ToString());
            //Console.ReadKey();
        }
    }
}
