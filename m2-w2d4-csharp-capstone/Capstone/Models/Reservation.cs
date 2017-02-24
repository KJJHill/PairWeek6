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

        public override string ToString()
        {
            return "The reservation confirmation #" + ReservationId.ToString().PadRight(3) 
                + "is at "+ ParkName + " park and at " 
                + CampgroundName + "which is campground #" + CampgroundId 
                + " at site #" + SiteId.ToString().PadRight(3)  
                + "for " + Name.PadRight(15)  
                + " from " + FromDate.ToString("D") 
                + " to " + ToDate.ToString("D")
                + " created on " + CreateDate.ToString("D");
        }
    }
}
