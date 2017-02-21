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
    public class DepartmentSQLDALTest
    {

        private TransactionScope tran;
        private string connectionString = @"Data Source=DESKTOP-4EOMNFH\sqlexpress;Initial Catalog=ProjectExercise;Integrated Security=True";
        List<string> allDepartments = new List<string>();
        int departmentCount = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand("SELECT COUNT (*) FROM department;", connection);

                departmentCount = (int)cmd.ExecuteScalar();
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
        public void TestUpdateDepartment()
        {
            DepartmentSqlDAL testUpdate = new DepartmentSqlDAL(connectionString);
            Department updatedDepartment = new Department();
            updatedDepartment.Id = 1;
            updatedDepartment.Name = "UpdatedDepartmentName";
            testUpdate.UpdateDepartment(updatedDepartment);
            string altereddepartment = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand("SELECT name FROM department WHERE department_id = 1;", connection);

                altereddepartment = (string)cmd.ExecuteScalar();
            }

            Assert.AreEqual(updatedDepartment.Name, altereddepartment);

        }

        [TestMethod]
        public void TestGetAllDepartments()
        {
            //Arrange
            DepartmentSqlDAL departmentDAL = new DepartmentSqlDAL(connectionString);

            //Act
            List<Department> departments = departmentDAL.GetDepartments();

            //Assert
            Assert.AreEqual(departmentCount, departments.Count);

        }

        [TestMethod]
        public void TestCreateDepartment()
        {
            //Arrange
            DepartmentSqlDAL departmentDAL = new DepartmentSqlDAL(connectionString);
            Department updatedDepartment = new Department();
            updatedDepartment.Name = "UpdatedDepartmentName";
            
            //Act
            bool successChange = departmentDAL.CreateDepartment(updatedDepartment);
            List<Department> departments = departmentDAL.GetDepartments();
            
            //Assert
            Assert.AreEqual(departmentCount +1, departments.Count);
        }


    }
}
