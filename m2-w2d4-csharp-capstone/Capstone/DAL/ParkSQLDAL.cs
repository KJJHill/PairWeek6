using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ParkSQLDAL
    {
        private string connectionString;
        private const string SQLGetAllParksWithFullDescription = "SELECT * FROM park;";
        private const string SQLGetAllParksShortened = "SELECT park.park_id, park.name FROM park;";
        private const string SQLGetAllParksByDate = @"SELECT park.park_id, park.name, campground.name, site.site_id 
                                                    FROM park
                                                    JOIN campground ON park.park_id = campground.park_id
                                                    JOIN site ON campground.campground_id = site.campground_id
                                                    WHERE park.park_id = @parkid
                                                    AND @startdatemonth BETWEEN(open_from_mm) AND(open_to_mm)
                                                    AND @enddatemonth BETWEEN(open_from_mm) and(open_to_mm)
									                AND site.site_id NOT IN (SELECT site_id FROM reservation 
									                WHERE (from_date BETWEEN(@startdate) AND (@enddate))
                                                    OR (to_date BETWEEN(@startdate) AND (@enddate))
                                                    OR ((@startdate) BETWEEN from_date AND to_date) 
                                                    OR((@enddate) BETWEEN from_date AND to_date));";


        public ParkSQLDAL(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }

        public List<Park> GetAllParksWithFullDescription()
        {
            List<Park> output = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQLGetAllParksWithFullDescription, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park p = new Park();
                        p.ParkID = Convert.ToInt32(reader["park_id"]);
                        p.Name = Convert.ToString(reader["name"]);
                        p.Location = Convert.ToString(reader["location"]);
                        p.EstDate = Convert.ToInt32(reader["establish_date"]);
                        p.Area = Convert.ToInt32(reader["area"]);
                        p.Visitors = Convert.ToInt32(reader["visitors"]);
                        p.Description = Convert.ToString(reader["description"]);

                        output.Add(p);
                    }
                }
            }
            catch (SqlException ex)
            {
                //Log and throw the exception
                throw;
            }

            return output;
        }

        public List<Park> GetAllParksShortened()
        {
            List<Park> output = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQLGetAllParksShortened, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park p = new Park();
                        p.ParkID = Convert.ToInt32(reader["park_id"]);
                        p.Name = Convert.ToString(reader["name"]);
                        

                        output.Add(p);
                    }
                }
            }
            catch (SqlException ex)
            {
                //Log and throw the exception
                throw;
            }

            return output;
        }

        public List<Park> GetAllParksByDate(DateTime startdate, DateTime enddate, int parkid)
        {
            List<Park> output = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQLGetAllParksByDate, conn);

                    cmd.Parameters.AddWithValue("@parkid", parkid);
                    cmd.Parameters.AddWithValue("@startdate", startdate.ToShortDateString());
                    cmd.Parameters.AddWithValue("@enddate", enddate.ToShortDateString());
                    cmd.Parameters.AddWithValue("@startdatemonth", startdate.Month);
                    cmd.Parameters.AddWithValue("@enddatemonth", enddate.Month);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park p = new Park();
                        p.ParkID = Convert.ToInt32(reader["park_id"]);
                        p.Name = Convert.ToString(reader["name"]);
                        p.CampgroundName = Convert.ToString(reader["name"]);
                        p.SiteID = Convert.ToInt32(reader["site_id"]);

                        output.Add(p);
                    }
                }
            }
            catch (SqlException ex)
            {
                //Log and throw the exception
                throw;
            }

            return output;
        }
    }
}
