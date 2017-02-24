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
            return " Campsite id: " + SiteId.ToString().PadRight(3) + CampgroundId.ToString().PadRight(3) + SiteNumber.ToString().PadRight(3)
                + MaxOccupancy.ToString().PadRight(3) + Accessible.PadRight(3) + MaxRVLength.ToString().PadRight(3)
                + Utilities.PadRight(3) + ParkName.PadRight(10) + CampgroundName.PadRight(10) + DailyFee.ToString().PadRight(5) ;
        }
    }
}
