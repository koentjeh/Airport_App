﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Airport.FlightManagementAPI.Model;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airport.FlightManagementAPI.DataAccess
{
    public class FlightManagementDBContext : DbContext
    {
        public FlightManagementDBContext(DbContextOptions<FlightManagementDBContext> options) : base(options)
        {

        }

        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Flight>().HasKey(m => m.FlightId);
            builder.Entity<Flight>().ToTable("Flight");
            base.OnModelCreating(builder);
        }

        public void MigrateDB()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(5, r => TimeSpan.FromSeconds(5))
                .Execute(() => Database.Migrate());
        }

    }
}
