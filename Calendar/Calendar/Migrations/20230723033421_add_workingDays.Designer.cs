﻿// <auto-generated />
using System;
using Calendar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Calendar.Migrations
{
    [DbContext(typeof(CalendarContext))]
    [Migration("20230723033421_add_workingDays")]
    partial class add_workingDays
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Calendar.Models.Holiday", b =>
                {
                    b.Property<int?>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("holidays")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Number");

                    b.ToTable("Holidays");
                });

            modelBuilder.Entity("Calendar.Models.WorkingDay", b =>
                {
                    b.Property<int?>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Selection")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Number");

                    b.ToTable("workingDays");
                });
#pragma warning restore 612, 618
        }
    }
}