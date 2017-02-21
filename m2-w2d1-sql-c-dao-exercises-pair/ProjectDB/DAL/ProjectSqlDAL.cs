using ProjectDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ProjectDB.DAL
{
    public class ProjectSqlDAL
    {
        private const string SQL_GetProject = "SELECT project_id, name, from_date, to_date FROM project;";
        private const string SQL_CreateProject = @"IF NOT EXISTS (SELECT * FROM project WHERE project.name = @projectname) INSERT INTO project(project.name, project.from_date, project.to_date) VALUES (@projectname, @startdate, @enddate); ";
        private const string SQL_RemoveEmployee = @"DELETE FROM project_employee WHERE project_employee.employee_id= @employeeid;";
        private const string SQL_AssignEmployee = @"IF NOT EXISTS (SELECT * FROM project_employee WHERE project_employee.project_id = @projectid AND project_employee.employee_id = @employeeid) INSERT INTO project_employee(project_id, employee_id) VALUES (@projectid, @employeeid);";
        

        private string connectionString;

        // Single Parameter Constructor
        public ProjectSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Project> GetAllProjects()
        {
            List<Project> output = new List<Project>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetProject, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Project p = new Project();
                        p.ProjectId = Convert.ToInt32(reader["project_id"]);
                        p.Name = Convert.ToString(reader["name"]);
                        p.StartDate = Convert.ToDateTime(reader["from_date"]);
                        p.EndDate = Convert.ToDateTime(reader["to_date"]);

                        output.Add(p);
                    }
                }

            }
            catch (SqlException ex)
            {
                throw new NotImplementedException();
            }
            return output;
        }

        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_AssignEmployee, conn);
                    cmd.Parameters.AddWithValue("@projectid", projectId);
                    cmd.Parameters.AddWithValue("@employeeid", employeeId);
                    int selectedRowsAffected = cmd.ExecuteNonQuery();

                    
                    return (selectedRowsAffected == 1);
                }

            }
            catch (SqlException ex)
            {
                throw new NotImplementedException();
            }

        }

        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_RemoveEmployee, conn);
                    cmd.Parameters.AddWithValue("@projectid", projectId);
                    cmd.Parameters.AddWithValue("@employeeid", employeeId);
                    int selectedRowsAffected = cmd.ExecuteNonQuery();

                    return (selectedRowsAffected == 1);
                }

            }
            catch (SqlException ex)
            {
                throw new NotImplementedException();
            }

        }

        public bool CreateProject(Project newProject)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                   
                        SqlCommand updcmd = new SqlCommand(SQL_CreateProject, conn);

                        updcmd.Parameters.AddWithValue("@projectname", newProject.Name);
                        updcmd.Parameters.AddWithValue("@startdate", newProject.StartDate);
                        updcmd.Parameters.AddWithValue("@enddate", newProject.EndDate);

                        int updaterowsAffected = updcmd.ExecuteNonQuery();

                        return (updaterowsAffected == 1);
                    
                    
                }

            }
            catch (SqlException ex)
            {
                throw new NotImplementedException();
            }

        }

    }
}
