using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Airport.RentalManagementAPI.DataAccess;

namespace Airport.RentalManagementAPI.Migrations
{
    [DbContext(typeof(RentalManagementDBContext))]
    partial class RentalManagementDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0");

            modelBuilder.Entity("Airport.RentalManagementAPI.Model.Rental", b =>
            {
                b.Property<string>("RentalId")
                    .ValueGeneratedOnAdd();

                b.Property<string>("RenterId");

                b.Property<string>("Location");

                b.Property<string>("Price");

                b.Property<string>("StartDate");

                b.Property<string>("EndDate");

                b.HasKey("RentalId");

                b.ToTable("Rental");
            });
        }
    }
}
