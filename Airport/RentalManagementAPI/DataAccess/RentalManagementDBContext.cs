using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Airport.RentalManagementAPI.Model;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airport.RentalManagementAPI.DataAccess
{
    public class RentalManagementDBContext : DbContext
    {
        public RentalManagementDBContext(DbContextOptions<RentalManagementDBContext> options) : base(options)
        {
        }

        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Rental>().HasKey(m => m.RentalId);
            builder.Entity<Rental>().ToTable("Rental");
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
