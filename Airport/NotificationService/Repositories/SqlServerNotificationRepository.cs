using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Airport.NotificationService.Model;
using Dapper;
using Polly;

namespace Airport.NotificationService.Repositories
{
    public class SqlServerNotificationRepository : INotificationRepository
    {
        private string _connectionString;

        public SqlServerNotificationRepository(string connectionString)
        {
            _connectionString = connectionString;

            // init db
            Policy
                .Handle<Exception>()
                .WaitAndRetry(5, r => TimeSpan.FromSeconds(5), (ex, ts) => { Console.WriteLine("Error connecting to DB. Retrying in 5 sec."); })
                .Execute(InitializeDB);
        }

        private async Task InitializeDB()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString.Replace("Notification", "master")))
            {
                await conn.OpenAsync();

                // create database
                string sql =
                    "IF DB_ID('Notification') IS NULL CREATE DATABASE Notification;";

                await conn.ExecuteAsync(sql);

                // create tables
                conn.ChangeDatabase("Notification");

                sql = "IF OBJECT_ID('Customer') IS NULL " +
                      "CREATE TABLE Customer (" +
                      "  CustomerId varchar(50) NOT NULL," +
                      "  Name varchar(50) NOT NULL," +
                      "  Address varchar(50)," +
                      "  City varchar(50)," +
                      "  Phone varchar(50)," +
                      "  Luggage BIT," +
                      "  PRIMARY KEY(CustomerId));" +

                await conn.ExecuteAsync(sql);
            }
        }

        public async Task RegisterCustomerAsync(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql =
                    "insert into Customer(CustomerId, Name, Address, City, Phone, Luggage) " +
                    "values(@CustomerId, @Name, @Address, @City, @Phone, @Luggage);";
                await conn.ExecuteAsync(sql, customer);
            }
        }  
    }
}
