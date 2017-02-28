using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ReservationSQLDAL
    {
        private string connectionString;

        private string SQL_GetAllReservations = @"SELECT r.reservation_id, r.create_date, 
                r.from_date, r.to_date, r.name, r.site_id, s.site_number, c.campground_id, 
                c.name as campground_name, p.park_id, p.name as park_name
                FROM reservation r
                JOIN site s on r.site_id = s.site_id
                JOIN campground c on s.campground_id = c.campground_id
                JOIN park p on p.park_id = c.park_id
				WHERE r.from_date between GETDATE() and DATEADD(month, 1, GETDATE())";

        private string SQL_GetReservationNumber = @"SELECT reservation_id, campground_id from reservation join site on reservation.site_id = site.site_id where name = @name and from_date = @fromDate and to_date = @toDate";

        //this verifies that the site id is not in a list of sites where the 
        //campground is open and that the reservations are not for the date range entered
        private string SQL_InsertReservation = @"if (@siteId not in
                                             
                                             (SELECT site_id FROM reservation
                                              WHERE from_date BETWEEN(@fromDate) AND(@toDate)
                                              OR(to_date BETWEEN(@fromDate) AND(@toDate))
                                              OR((@fromDate) BETWEEN from_date AND to_date)
                                              OR((@toDate) BETWEEN from_date AND to_date))) 

                                              INSERT INTO reservation(site_id, name, from_date, to_date)
                                              VALUES(@siteId, @name, @fromDate, @toDate)";

        public ReservationSQLDAL(string databaseConnectionString)
        {
            connectionString = databaseConnectionString;
        }

        public void AddNewReservation(Reservation newReservation)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_InsertReservation, conn);
                    cmd.Parameters.AddWithValue("@siteId", newReservation.SiteId);
                    cmd.Parameters.AddWithValue("@name", newReservation.Name);
                    cmd.Parameters.AddWithValue("@fromDate", newReservation.FromDate);
                    cmd.Parameters.AddWithValue("@reservationFromMonth", newReservation.FromDate.Month);
                    cmd.Parameters.AddWithValue("@toDate", newReservation.ToDate);
                    cmd.Parameters.AddWithValue("@reservationToMonth", newReservation.ToDate.Month);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        // get the confirmation number
                        cmd = new SqlCommand(SQL_GetReservationNumber, conn);
                        cmd.Parameters.AddWithValue("@toDate", newReservation.ToDate);
                        cmd.Parameters.AddWithValue("@name", newReservation.Name);
                        cmd.Parameters.AddWithValue("@fromDate", newReservation.FromDate);

                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();

                        newReservation.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        newReservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
                    }
                }
            }
            catch (SqlException ex)
            {
                //log
                throw;
            }

        }

        public List<Reservation> GetAllReservations()
        {
            List<Reservation> output = new List<Reservation>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetAllReservations, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation r = new Reservation();
                        r.SiteId = Convert.ToInt32(reader["site_id"]);
                        r.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        r.Name = Convert.ToString(reader["name"]); ;
                        r.ReservationId = Convert.ToInt32(reader["reservation_id"]);
                        r.FromDate = Convert.ToDateTime(reader["from_date"]);
                        r.ToDate = Convert.ToDateTime(reader["to_date"]);
                        r.CreateDate = Convert.ToDateTime(reader["create_date"]);
                        r.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        r.ParkId = Convert.ToInt32(reader["park_id"]);
                        r.ParkName = Convert.ToString(reader["park_name"]);
                        r.CampgroundName = Convert.ToString(reader["campground_name"]);

                        output.Add(r);
                    }
                }
            }
            catch (SqlException ex)
            {
                //log
                throw;
            }

            return output;
        }
    }
}
