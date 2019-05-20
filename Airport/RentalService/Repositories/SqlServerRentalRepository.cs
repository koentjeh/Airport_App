using System;
using System.Threading.Tasks;
using Dapper;
using Airport.RentalService.Model;
using Polly;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using Serilog;

namespace Airport.RentalService.Repositories
{
    public class SqlServerRentalRepository : IRentalRepository
    {
        private string _connectionString;

        public SqlServerRentalRepository(string connectionString)
        {
            _connectionString = connectionString;

            // init db
            Policy
            .Handle<Exception>()
            .WaitAndRetry(5, r => TimeSpan.FromSeconds(5), (ex, ts) => { Log.Error("Error connecting to DB. Retrying in 5 sec."); })
            .Execute(InitializeDB);
        }

        private async Task InitializeDB()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString.Replace("Rentals", "master")))
            {
                await conn.OpenAsync();

                // create database
                string sql =
                    "IF DB_ID('Rentals') IS NULL CREATE DATABASE Rental;";

                await conn.ExecuteAsync(sql);

                // create tables
                conn.ChangeDatabase("Rental");

                sql = "IF OBJECT_ID('Rental') IS NULL " +
                      "CREATE TABLE Rental (" +
                      "  RentalId varchar(50) NOT NULL," +
                      "  RenterId varchar(50) NOT NULL," +
                      "  Location varchar(50) NOT NULL," +
                      "  Price varchar(50) NOT NULL," +
                      "  StartDate datetime NOT NULL," +
                      "  EndDate datetime NOT NULL," +
                      "  PRIMARY KEY(RentalId));" +

                await conn.ExecuteAsync(sql);
            }
        }

        public async Task<Rental> GetRentalAsync(string rentalId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<Rental>("select * from Rental where RentalId = @RentalId",
                    new { RentalId = rentalId });
            }
        }

        public async Task RegisterRentalAsync(Rental rental)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql =
                    "insert into Rental(RentalId, RenterId, Location, Price, StartDate, EndDate) " +
                    "values(@RentalId, @RenterId, @Location, @Price, @StartDate, @EndDate);";
                await conn.ExecuteAsync(sql, rental);
            }
        }

        public async Task MarkMaintenanceJobAsFinished(string jobId, DateTime startTime, DateTime endTime)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query =
                    "update MaintenanceJob " +
                    "set StartTime = @StartTime, " +
                    "    EndTime = @EndTime, " +
                    "    Finished = 1 " +
                    "where JobId = @JobId";
                await conn.ExecuteAsync(query, new { JobId = jobId, StartTime = startTime, EndTime = endTime });
            }
        }

    }
}
