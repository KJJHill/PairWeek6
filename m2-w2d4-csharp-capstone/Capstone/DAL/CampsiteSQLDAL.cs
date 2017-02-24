using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampsiteSQLDAL
    {

        private string connectionString;
        bool limitOptions = false;

        private string SQL_MainClause =
            @"SELECT TOP 5 site.site_id, site.site_number, park.park_id, park.name as park_name, 
            campground.campground_id, campground.name as campground_name, site.accessible, site.utilities, 
            site.max_rv_length, site.max_occupancy, campground.daily_fee
            FROM park 
            JOIN campground on park.park_id = campground.park_id
            JOIN site on site.campground_id = campground.campground_id
            JOIN reservation ON reservation.site_id = site.site_id 
            WHERE ";

        private string SQL_AvailableDatesClause = " (reservation.from_date NOT BETWEEN @fromDate AND @toDate)" +
                    " AND (reservation.to_date NOT BETWEEN @fromDate AND @toDate)" +
                    " AND((@fromDate NOT BETWEEN(from_date) AND(to_date))" +
                    " AND(@toDate NOT BETWEEN (from_date) AND(to_date)))) " +

                    " AND @reservationFromMonth BETWEEN(campground.open_from_mm) AND (campground.open_to_mm) " +
                    " AND @reservationToMonth BETWEEN(campground.open_from_mm) AND (campground.open_to_mm) ";

        public CampsiteSQLDAL(string databaseConnectionString, bool lmtOptions)
        {
            connectionString = databaseConnectionString;
            limitOptions = lmtOptions;
        }

        public List<Campsite> GetCampsitesByParkId(int parkId)
        {
            try
            {
                SQL_MainClause += "park.park_id = @parkId ";

                SQL_MainClause += (limitOptions) ? SQL_AddLimitOptionsClause() : "";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_MainClause, conn);
                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    return GetListOfCampsites(cmd);
                }
            }
            catch (SqlException ex)
            {
                //log
                throw;
            }
        }

        public List<Campsite> GetCampsitesByAvailableDates(DateTime fromDate, DateTime toDate)
        {
            try
            {
                SQL_MainClause += SQL_AvailableDatesClause;

                SQL_MainClause += (limitOptions) ? SQL_AddLimitOptionsClause() : "";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_MainClause, conn);

                    cmd = AddAvailableDateParameters(cmd, fromDate, toDate);

                    //cmd.Parameters.AddWithValue("@fromDate", fromDate.ToShortDateString());
                    //cmd.Parameters.AddWithValue("@toDate", toDate.ToShortDateString());
                    //cmd.Parameters.AddWithValue("@reservationFromMonth", fromDate.Month);
                    //cmd.Parameters.AddWithValue("@reservationToMonth", toDate.Month);

                    return GetListOfCampsites(cmd);
                }
            }
            catch (SqlException ex)
            {
                //log
                throw;
            }
        }

        public List<Campsite> GetCampsitesByParkIdAndAvailableDates(int parkId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                SQL_MainClause += " park.park_id = {parkId} " + SQL_AvailableDatesClause;

                SQL_MainClause += (limitOptions) ? SQL_AddLimitOptionsClause() : "";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_MainClause, conn);

                    cmd = AddAvailableDateParameters(cmd, fromDate, toDate);

                    //cmd.Parameters.AddWithValue("@parkId", parkId);
                    //cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    //cmd.Parameters.AddWithValue("@toDate", toDate);
                    //cmd.Parameters.AddWithValue("@reservationFromMonth", fromDate.Month);
                    //cmd.Parameters.AddWithValue("@reservationToMonth", toDate.Month);

                    return GetListOfCampsites(cmd);
                }
            }
            catch (SqlException ex)
            {
                //log
                throw;
            }
        }

        public List<Campsite> GetCampsitesByParkIdAndCampgroundId(int parkId, int campgroundId)
        {
            try
            {
                SQL_MainClause += "park.park_id = @parkId AND campground.campground_id = @campgroundId";

                SQL_MainClause += (limitOptions) ? SQL_AddLimitOptionsClause() : "";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_MainClause, conn);
                    cmd.Parameters.AddWithValue("@parkId", parkId);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);

                    return GetListOfCampsites(cmd);
                }
            }
            catch (SqlException ex)
            {
                //log
                throw;
            }

        }

        public List<Campsite> GetCampsitesByParkCampgroundAndAvailableDates(int parkId, int campgroundId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                SQL_MainClause += " park.park_id = {parkId} AND campground.campground_id = {campgroundId} AND "
                                + SQL_AvailableDatesClause;

                SQL_MainClause += (limitOptions) ? SQL_AddLimitOptionsClause() : "";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_MainClause, conn);

                    cmd = AddAvailableDateParameters(cmd, fromDate, toDate);

                    //cmd.Parameters.AddWithValue("@parkId", parkId);
                    //cmd.Parameters.AddWithValue("@campgroundId", campgroundId);

                    //cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    //cmd.Parameters.AddWithValue("@toDate", toDate);
                    //cmd.Parameters.AddWithValue("@reservationFromMonth", fromDate.Month);
                    //cmd.Parameters.AddWithValue("@reservationToMonth", toDate.Month);

                    return GetListOfCampsites(cmd);
                }
            }
            catch (SqlException ex)
            {
                //log
                throw;
            }
        }

        private List<Campsite> GetListOfCampsites(SqlCommand cmd)
        {
            List<Campsite> output = new List<Campsite>();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Campsite c = new Campsite();
                c.SiteId = Convert.ToInt32(reader["site_id"]);
                c.SiteNumber = Convert.ToInt32(reader["site_number"]);
                c.ParkId = Convert.ToInt32(reader["park_id"]);
                c.ParkName = Convert.ToString(reader["park_name"]);
                c.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                c.CampgroundName = Convert.ToString(reader["campground_name"]);
                c.Accessible = (Convert.ToBoolean(reader["accessible"])) ? "Yes" : "No";
                c.Utilities = (Convert.ToBoolean(reader["utilities"])) ? "Yes" : "No";
                c.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                c.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                c.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

                output.Add(c);
            }
            return output;
        }
        
        private SqlCommand AddAvailableDateParameters(SqlCommand cmd, DateTime fromDate, DateTime toDate)
        {
            cmd.Parameters.AddWithValue("@fromDate", fromDate);
            cmd.Parameters.AddWithValue("@toDate", toDate);
            cmd.Parameters.AddWithValue("@reservationFromMonth", fromDate.Month);
            cmd.Parameters.AddWithValue("@reservationToMonth", toDate.Month);
            return cmd;
        }

        private string SQL_AddLimitOptionsClause()
        {
            string output = "";
            //if accessible = 1 then output += " and site.accessible = 1"
            //if utilities = 1 then output += " and site.utilities = 1"
            //if max_rv_length > 0 then output += " site.max_rv_length <= @maxrvlength"
            //if max_occupancy > 0 then output += " site.max_occupancy <= @maxoccupancy"
            return output;
        }

    }
}
