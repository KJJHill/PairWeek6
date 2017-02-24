using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {
        public int CampID { get; set; }
        public int ParkID { get; set; }
        public string Name { get; set; }
        public int OpenFromMonth { get; set; }
        public int OpenToMonth { get; set; }
        public double DailyFee { get; set; }

        public override string ToString()
        {
            return CampID.ToString().PadRight(5) + ParkID.ToString().PadRight(20) + Name.PadRight(30) + OpenFromMonth.ToString().PadRight(10) + OpenToMonth.ToString().PadRight(15) + DailyFee.ToString().PadRight(30);
        }
    }
}
