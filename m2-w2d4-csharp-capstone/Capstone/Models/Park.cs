using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Park
    {
        
        public int ParkID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstDate { get; set; }
        public int Area { get; set; }
        public int Visitors { get; set; }
        public string Description { get; set; }
        public int SiteID { get; set; }

        public string CampgroundName { get; set; }


        public override string ToString()
        {
            //return ParkID.ToString().PadRight(5) + Name.ToString().PadRight(20) + Location.PadRight(30) + EstDate.ToString().PadRight(10) + Area.ToString().PadRight(15) + Visitors.ToString().PadRight(30) + Description.ToString().PadRight(5);
            return string.Format("\r\nPark ID and Name: {0}-{1}     Location: {2}     Established in: {3}     Area: {4} \r\nDescription: {5}\r\n", ParkID, Name, Location, EstDate, Area, Description);
        }
    }
}

