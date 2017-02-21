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
    public class ProjectSQLDALTest
    {

        private TransactionScope tran;
        private string connectionString = @"Data Source=DESKTOP-4EOMNFH\sqlexpress;Initial Catalog=ProjectExercise;Integrated Security=True";
        List<Project> allProjects = new List<Project>();
        int projectCount = 0;
        int employeeProjectCount = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand("SELECT COUNT (*) FROM project;", connection);
                projectCount = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("SELECT COUNT (*) FROM project_employee;", connection);
                employeeProjectCount = (int)cmd.ExecuteScalar();
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
        public void TestGetAllProjects()
        {
            //Arrange
            ProjectSqlDAL projectDAL = new ProjectSqlDAL(connectionString);

            //Act
            List<Project> projects = projectDAL.GetAllProjects();

            //Assert
            Assert.AreEqual(projectCount, projects.Count);

        }

        [TestMethod]
        public void TestCreateProject()
        {
            //Arrange
            ProjectSqlDAL projectDAL = new ProjectSqlDAL(connectionString);
            Project updatedProject = new Project();
            updatedProject.Name = "UpdatedProjectName";
            updatedProject.StartDate = DateTime.Now;
            updatedProject.EndDate = updatedProject.StartDate.AddYears(1);


            //Act
            bool successChange = projectDAL.CreateProject(updatedProject);
            List<Project> projects = projectDAL.GetAllProjects();

            //Assert
            Assert.AreEqual(projectCount + 1, projects.Count);
        }

        [TestMethod]
        public void TestAssignEmployee()
        {
            //Arrange
            ProjectSqlDAL projectDAL = new ProjectSqlDAL(connectionString);

            //Act
            bool employeeAssigned = projectDAL.AssignEmployeeToProject(1,1);

            //Assert
            int alteredprojects = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand("SELECT COUNT(*) FROM project_employee;", connection);

                alteredprojects = (int)cmd.ExecuteScalar();
            }

            Assert.AreEqual(employeeProjectCount + 1, alteredprojects);

        }
        [TestMethod]
        public void TestRemoveEmployee()
        {
            //Arrange
            ProjectSqlDAL projectDAL = new ProjectSqlDAL(connectionString);

            //Act
            bool employeeRemoved = projectDAL.RemoveEmployeeFromProject(1, 1);

            //Assert
            int alteredprojects = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand("SELECT COUNT(*) FROM project_employee;", connection);

                alteredprojects = (int)cmd.ExecuteScalar();
            }

            Assert.AreEqual(employeeProjectCount - 1, alteredprojects);

        }
    }
}
