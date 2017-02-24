using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class CampsiteTest
    {
        [TestMethod]
        public void CampsiteDisplayTest()
        {
            Campsite myCampsite = new Campsite();
            myCampsite.SiteId = 1;
            myCampsite.CampgroundId = 1;
            myCampsite.SiteNumber = 2;
            myCampsite.MaxOccupancy = 6;
            myCampsite.Accessible = "Y";
            myCampsite.MaxRVLength = 30;
            myCampsite.Utilities = "Y";
            myCampsite.ParkId = 1;
            myCampsite.ParkName = "Glacier";
            myCampsite.CampgroundName = "Running Brook";
            myCampsite.DailyFee = 35.00M;

            //Console.WriteLine(myCampsite.ToString());
            //Console.ReadKey();
        }
    }
}
