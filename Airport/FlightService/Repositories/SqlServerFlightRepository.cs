using System;
using System.Threading.Tasks;
using Dapper;
using Airport.FlightService.Model;
using Polly;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using Serilog;

namespace Airport.FlightService.Repositories
{
    public class SqlServerFlightRepository : IFlightRepository
    {
        private string _connectionString;

        public SqlServerFlightRepository(string connectionString)
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
            using (SqlConnection conn = new SqlConnection(_connectionString.Replace("Flights", "master")))
            {
                await conn.OpenAsync();

                // create database
                string sql =
                    "IF DB_ID('Flights') IS NULL CREATE DATABASE Flights;";

                await conn.ExecuteAsync(sql);

                // create tables
                conn.ChangeDatabase("Flights");

                sql = "IF OBJECT_ID('Flight') IS NULL " +
                      "CREATE TABLE Flight (" +
                      "  FlightId varchar(50) NOT NULL," +
                      "  DepartureDate datetime NOT NULL," +
                      "  Runway varchar(50)," +
                      "  ArrivalDate datetime," +
                      "  City varchar(50)," +
                      " Pilot varchar(50)," +
                      "  PRIMARY KEY(CustomerId));";

                await conn.ExecuteAsync(sql);
            }
        }

        public async Task<Flight> GetFlightAsync(string flightId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<Flight>("select * from Flight where FlightId = @FlightId",
                    new { FlightId = flightId });
            }
        }

        public async Task RegisterFlightAsync(Flight flight)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql =
                    "insert into Flight(FlightId, DepartureDate, Runway, ArrivalDate, City, Pilot) " +
                    "values(@FlightId, @DepartureDate, @Runway, @ArrivalDate, @City, @Pilot);";
                await conn.ExecuteAsync(sql, flight);
            }
        }
    }
}
