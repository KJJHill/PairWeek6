using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using ProjectDB.DAL;
using ProjectDB.Models;

namespace ProjectDBTest
{
    [TestClass]
    public class EmployeeSQLDALTest
    {

        private TransactionScope tran;
        private string connectionString = @"Data Source=DESKTOP-4EOMNFH\sqlexpress;Initial Catalog=ProjectExercise;Integrated Security=True";
        List<Employee> searchEmployee = new List<Employee>();
        int employeeCount = 0;
        int employeeWithoutProject = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                SqlCommand cmd2;
                SqlCommand cmd3;

                connection.Open();

                cmd = new SqlCommand("SELECT COUNT (*) FROM employee;", connection);
                
                employeeCount = (int)cmd.ExecuteScalar();

                cmd2 = new SqlCommand("SELECT COUNT(*) FROM employee LEFT JOIN project_employee ON employee.employee_id = project_employee.employee_id WHERE project_employee.employee_id IS NULL;", connection);
                employeeWithoutProject = (int)cmd2.ExecuteScalar();

                cmd3 = new SqlCommand("SELECT employee_id, department_id, first_name, last_name, job_title, birth_date, gender, hire_date FROM employee WHERE first_name = 'Delora' AND last_name = 'Coty';", connection);
                SqlDataReader reader = cmd3.ExecuteReader();

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

                    searchEmployee.Add(e);
                }
            }

        }

        /*
        * CLEANUP
        * Rollback the existing transaction.
        */
        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void TestGetAllEmployees()
        {
            //Arrange
            EmployeeSqlDAL employeeDAL = new EmployeeSqlDAL(connectionString);

            //Act
            List<Employee> employees = employeeDAL.GetAllEmployees();

            //Assert
            Assert.AreEqual(employeeCount, employees.Count);
        }

        [TestMethod]
        public void TestEmployeesWithoutProject()
        {
            //Arrange
            EmployeeSqlDAL employeeDAL = new EmployeeSqlDAL(connectionString);

            //Act
            List<Employee> employeesWOProject = employeeDAL.GetEmployeesWithoutProjects();

            //Assert
            Assert.AreEqual(employeeWithoutProject, employeesWOProject.Count);
        }

        [TestMethod]
        public void TestSearch()
        {
            //Arrange
            EmployeeSqlDAL employeeDAL = new EmployeeSqlDAL(connectionString);

            //Act
            List<Employee> foundEmployee = employeeDAL.Search("Delora", "Coty");

            //Assert
            Assert.AreEqual(searchEmployee[0].EmployeeId, foundEmployee[0].EmployeeId);

        }
    }
}
