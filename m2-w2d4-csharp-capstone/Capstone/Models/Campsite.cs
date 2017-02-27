using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campsite
    {
        public int SiteId { get; set; }
        public int ParkId { get; set; }
        public string ParkName { get; set; }
        public int CampgroundId { get; set; }
        public string CampgroundName { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public string Accessible { get; set; }
        public int MaxRVLength { get; set; }
        public string Utilities { get; set; }
        public decimal DailyFee { get; set; }

        public override string ToString()
        {
            //return "\r\n" + "Campsite id: " + SiteId.ToString().PadRight(3) + "Campground- " + CampgroundId.ToString() + ":" + CampgroundName.ToString().PadRight(5)
            //    + "Park: ".PadRight(5) + ParkName.PadLeft(10) + "\r\n" + "Max Occupancy: " + MaxOccupancy.ToString().PadRight(3) + "WheelChair Accessible: " + Accessible.PadRight(3) + "Max RV Length: " + MaxRVLength.ToString().PadRight(3)
            //    + "Utilities Available:" + Utilities.PadRight(3) + "Daily Fee: " + DailyFee.ToString().PadRight(5) + "\r\n";

            return string.Format("\r\nCampsite ID: {0}     Campground ID and Name: {1}-{2}     Park Name: {3} \r\nMax Occupancy: {4}     WheelChair Accessible: {5}     Max RV Length: {6}     Utilities Available: {7}     Daily Fee: {8}\r\n", SiteId, CampgroundId, CampgroundName, ParkName, MaxOccupancy, Accessible, MaxRVLength, Utilities, DailyFee);
        }
    }
}
