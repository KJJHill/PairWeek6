using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int SiteId { get; set; }
        public int CampgroundId { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int SiteNumber { get; set; }
        public int ParkId { get; set; }
        public string ParkName { get; set; }
        public string CampgroundName { get; set; }
        public decimal DailyFee { get; set; }

        public override string ToString()
        {
            return "Reservation confirmation #" + ReservationId.ToString()
                + " is for " + Name.PadRight(7)
                + " \n at " + ParkName + " park and at "
                + CampgroundName + " campground (#" + CampgroundId
                + ") at site #" + SiteId.ToString().PadRight(3)
                + "\n from " + FromDate.ToString("D")
                + " to " + ToDate.ToString("D")
                + " and was created on " + CreateDate.ToString("D")
                + "\n Total Cost is of stay: " + String.Format("{0:C2}", ((ToDate - FromDate).Days * DailyFee));
        }
    }
}
