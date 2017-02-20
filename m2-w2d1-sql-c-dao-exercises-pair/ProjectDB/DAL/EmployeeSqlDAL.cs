using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjectDB.DAL
{
    public class EmployeeSqlDAL
    {
        private const string SQL_GetAllEmployees = "SELECT employee_id, department_id, first_name, last_name, job_title, birth_date, gender, hire_date FROM employee;";
        private const string SQL_GetEmployeesWithoutProjects = "SELECT employee.employee_id, department_id, first_name, last_name, job_title, birth_date, gender, hire_date FROM employee LEFT JOIN project_employee ON employee.employee_id = project_employee.employee_id WHERE project_employee.employee_id IS NULL;";
        private const string SQL_EmployeeSearch = @"SELECT employee_id, department_id, first_name, last_name, job_title, birth_date, gender, hire_date FROM employee WHERE first_name = @employeefirstname AND last_name = @employeelastname;";
        private string connectionString;

        // Single Parameter Constructor
        public EmployeeSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> output = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetAllEmployees, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee e = new Employee();
                        e.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        e.DepartmentId = Convert.ToInt32(reader["department_id"]);
                        e.FirstName = Convert.ToString(reader["first_name"]);
                        e.LastName = Convert.ToString(reader["last_name"]);
                        e.JobTitle = Convert.ToString(reader["job_title"]);
                        e.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        e.Gender = Convert.ToString(reader["gender"]);
                        e.HireDate = Convert.ToDateTime(reader["hire_date"]);


                        output.Add(e);
                    }
                }

            }
            catch (SqlException ex)
            {
                throw new NotImplementedException();
            }
            return output;
        }

        public List<Employee> Search(string firstname, string lastname)
        {
            List<Employee> output = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_EmployeeSearch, conn);
                    cmd.Parameters.AddWithValue("@employeefirstname", firstname);
                    cmd.Parameters.AddWithValue("@employeelastname", lastname);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee e = new Employee();
                        e.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        e.DepartmentId = Convert.ToInt32(reader["department_id"]);
                        e.FirstName = Convert.ToString(reader["first_name"]);
                        e.LastName = Convert.ToString(reader["last_name"]);
                        e.JobTitle = Convert.ToString(reader["job_title"]);
                        e.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        e.Gender = Convert.ToString(reader["gender"]);
                        e.HireDate = Convert.ToDateTime(reader["hire_date"]);


                        output.Add(e);
                    }
                }

            }
            catch (SqlException ex)
            {
                throw new NotImplementedException();
            }
            return output;
        }
    

    public List<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> output = new List<Employee>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetEmployeesWithoutProjects, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee e = new Employee();
                        e.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        e.DepartmentId = Convert.ToInt32(reader["department_id"]);
                        e.FirstName = Convert.ToString(reader["first_name"]);
                        e.LastName = Convert.ToString(reader["last_name"]);
                        e.JobTitle = Convert.ToString(reader["job_title"]);
                        e.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        e.Gender = Convert.ToString(reader["gender"]);
                        e.HireDate = Convert.ToDateTime(reader["hire_date"]);


                        output.Add(e);
                    }
                }

            }
            catch (SqlException ex)
            {
                throw new NotImplementedException();
            }
            return output;
        }
    }
}
