using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjectDB.DAL
{
    public class DepartmentSqlDAL
    {
        private const string SQL_GetDepartment = "SELECT department.department_id, department.name FROM Department; ";
        private const string SQL_CreateDepartment = @"IF NOT EXISTS (SELECT * FROM department WHERE department.name = @departmentname) INSERT INTO department(department.name) VALUES (@departmentname); ";
        private const string SQL_SelectDepartment = "SELECT department.department_id, department.name FROM Department WHERE name = @departmentname; ";
        private const string SQL_UpdateDepartment = @"UPDATE department SET department.name = @changeddepartmentname WHERE department.department_id= @updateddepartmentid";
        private string connectionString;

        // Single Parameter Constructor
        public DepartmentSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Department> GetDepartments()
        {
            List<Department> output = new List<Department>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetDepartment, conn);

                    output = GetCollectionDepartments(cmd);
                    //SqlDataReader reader = cmd.ExecuteReader();

                    //while (reader.Read())
                    //{
                    //    Department d = new Department();
                    //    d.Id= Convert.ToInt32(reader["department_id"]);
                    //    d.Name = Convert.ToString(reader["name"]);

                    //    output.Add(d);
                    //}
                }

            }
            catch (SqlException ex)
            {
                throw new NotImplementedException();
            }
            return output;
        }

        public bool CreateDepartment(Department newDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand updcmd = new SqlCommand(SQL_CreateDepartment, conn);

                    updcmd.Parameters.AddWithValue("@departmentname", newDepartment.Name);

                    int updaterowsAffected = updcmd.ExecuteNonQuery();

                    return (updaterowsAffected == 1);
                }

            }
            catch (SqlException ex)
            {
                throw new NotImplementedException();
            }

        }

        public bool UpdateDepartment(Department updatedDepartment)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_UpdateDepartment, conn);
                    cmd.Parameters.AddWithValue("@updateddepartmentid", updatedDepartment.Id);
                    cmd.Parameters.AddWithValue("@changeddepartmentname", updatedDepartment.Name);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return (rowsAffected == 1);
                }

            }
            catch (SqlException ex)
            {
                throw new NotImplementedException();
            }

        }

        public List<Department> GetCollectionDepartments(SqlCommand cmd)
        {
            List<Department> output = new List<Department>();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Department d = new Department();
                d.Id = Convert.ToInt32(reader["department_id"]);
                d.Name = Convert.ToString(reader["name"]);

                output.Add(d);
            }
            return output;
        }

    }
}
