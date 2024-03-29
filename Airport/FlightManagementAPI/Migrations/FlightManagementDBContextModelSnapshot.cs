﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Airport.FlightManagementAPI.DataAccess;

namespace Airport.FlightManagementAPI.Migrations
{
    [DbContext(typeof(FlightManagementDBContext))]
    partial class FlightManagementDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Airport.FlightManagementAPI.Model.Flight", b =>
            {
                b.Property<string>("FlightId")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("DepartureDate");

                b.Property<string>("Gate");

                b.Property<string>("CheckInCounter");

                b.Property<DateTime>("ArrivalDate");

                b.Property<string>("City");

                b.Property<string>("Pilot");

            });
        }
    }
}
