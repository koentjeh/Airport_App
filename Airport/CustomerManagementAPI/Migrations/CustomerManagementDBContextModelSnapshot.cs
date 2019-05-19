using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Airport.CustomerManagementAPI.DataAccess;

namespace Airport.CustomerManagementAPI.Migrations
{
    [DbContext(typeof(CustomerManagementDBContext))]
    partial class CustomerManagementDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0");

            modelBuilder.Entity("Airport.CustomerManagementAPI.Model.Customer", b =>
            {
                b.Property<string>("CustomerId")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Name");

                b.Property<string>("Address");

                b.Property<string>("City");

                b.Property<string>("Phone");

                b.Property<bool>("Luggage");

                b.HasKey("CustomerId");

                b.ToTable("Customer");
            });
        }
    }
}
