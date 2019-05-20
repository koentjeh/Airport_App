using System;
using System.Threading.Tasks;
using Dapper;
using Airport.InvoiceService.Model;
using Polly;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using Serilog;

namespace Airport.InvoiceService.Repositories
{
    public class SqlServerInvoiceRepository : IInvoiceRepository
    {
        private string _connectionString;

        public SqlServerInvoiceRepository(string connectionString)
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
            using (SqlConnection conn = new SqlConnection(_connectionString.Replace("Invoicing", "master")))
            {
                await conn.OpenAsync();

                // create database
                string sql =
                    "IF DB_ID('Invoicing') IS NULL CREATE DATABASE Invoicing;";

                await conn.ExecuteAsync(sql);

                // create tables
                conn.ChangeDatabase("Invoicing");

                sql = "IF OBJECT_ID('Customer') IS NULL " +
                      "CREATE TABLE Customer (" +
                      "  CustomerId varchar(50) NOT NULL," +
                      "  Name varchar(50) NOT NULL," +
                      "  Address varchar(50)," +
                      "  City varchar(50)," +
                      "  Phone varchar(50)," +
                      "  Luggage BIT," +
                      "  PRIMARY KEY(CustomerId));" +

                      "IF OBJECT_ID('Invoice') IS NULL " +
                      "CREATE TABLE Invoice (" +
                      "  InvoiceId varchar(50) NOT NULL," +
                      "  InvoiceDate datetime2 NOT NULL," +
                      "  CustomerId varchar(50) NOT NULL," +
                      "  Amount decimal(5,2) NOT NULL," +
                      "  Specification text," +
                      "  FlightId varchar(50)," +
                      "  PRIMARY KEY(InvoiceId));" +

                "IF OBJECT_ID('Flight') IS NULL " +
                      "CREATE TABLE Flight (" +
                      "  FlightId varchar(50) NOT NULL," +
                      "  DepartureDate datetime NOT NULL," +
                      "  Gate varchar(50)," +
                      "  CheckInGate varchar(50)," +
                      "  ArrivalDate datetime," +
                      "  City varchar(50)," +
                      " Pilot varchar(50)," +
                      "  PRIMARY KEY(FlightId));";

                await conn.ExecuteAsync(sql);
            }
        }

        public async Task<Customer> GetCustomerAsync(string customerId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryFirstOrDefaultAsync<Customer>("select * from Customer where CustomerId = @CustomerId",
                    new { CustomerId = customerId });
            }
        }

        public async Task RegisterCustomerAsync(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql =
                    "insert into Customer(CustomerId, Name, Address, PostalCode, City) " +
                    "values(@CustomerId, @Name, @Address, @PostalCode, @City);";
                await conn.ExecuteAsync(sql, customer);
            }
        }

        public async Task RegisterFlightAsync(Flight flight)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql =
                    "insert into Flight(FlightId, DepartureDate, Gate, CheckInCounter ArrivalDate, City, Pilot) " +
                    "values(@FlightId, @DepartureDate, @Gate, @CheckInCounter, @ArrivalDate, @City, @Pilot);";
                await conn.ExecuteAsync(sql, flight);
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

        public async Task RegisterInvoiceAsync(Invoice invoice)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // persist invoice
                string sql =
                    "insert into Invoice(InvoiceId, InvoiceDate, CustomerId, FlightId, Amount, Specification) " +
                    "values(@InvoiceId, @InvoiceDate, @CustomerId, @FlightId @Amount, @Specification);";
                await conn.ExecuteAsync(sql, invoice);
            }
        }
    }
}
