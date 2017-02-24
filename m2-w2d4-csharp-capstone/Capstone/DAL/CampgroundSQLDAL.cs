using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampgroundSQLDAL
    {
        private string connectionString;
        private const string SQLGetAllCampgroundsParkID = @"SELECT * FROM campground WHERE campground.park_id = @parkid;";
        private const string SQLGetAllCampgroundsParkIDDate = @"SELECT * FROM campground 
                            JOIN site ON site.campground_id = campground.campground_id 
                            JOIN reservation ON reservation.site_id = site.site_id
                            WHERE park.park_id = @parkid
                            AND @startdatemonth BETWEEN(open_from_mm) AND(open_to_mm)
                            AND @enddatemonth BETWEEN(open_from_mm) and(open_to_mm)
							AND site.site_id NOT IN (SELECT site_id FROM reservation 
							WHERE (from_date BETWEEN(@startdate) AND (@enddate))
                            OR (to_date BETWEEN(@startdate) AND (@enddate))
                            OR ((@startdate) BETWEEN from_date AND to_date) 
                            OR((@enddate) BETWEEN from_date AND to_date));";



        public CampgroundSQLDAL(string databaseconnectionString)
        {
            connectionString = databaseconnectionString;
        }

        public List<Campground> GetAllCampgroundsParkID(int parkid)
        {
            List<Campground> output = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQLGetAllCampgroundsParkID, conn);

                    cmd.Parameters.AddWithValue("@parkid", parkid);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground c = new Campground();
                        c.CampID = Convert.ToInt32(reader["campground_id"]);
                        c.ParkID = Convert.ToInt32(reader["park_id"]);
                        c.Name = Convert.ToString(reader["name"]);
                        c.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
                        c.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
                        c.DailyFee = Convert.ToDouble(reader["daily_fee"]);

                        output.Add(c);
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

        public List<Campground> GetAllCampgroundsParkIDDate(string parkid)
        {
            List<Campground> output = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQLGetAllCampgroundsParkID, conn);

                    cmd.Parameters.AddWithValue("@parkid", parkid);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground c = new Campground();
                        c.CampID = Convert.ToInt32(reader["campground_id"]);
                        c.ParkID = Convert.ToInt32(reader["park_id"]);
                        c.Name = Convert.ToString(reader["name"]);
                        c.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
                        c.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
                        c.DailyFee = Convert.ToInt32(reader["daily_fee"]);

                        output.Add(c);
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

        //get all campgrounds (parkid)
        //get all campgrounds (parkid, date)


    }
}
