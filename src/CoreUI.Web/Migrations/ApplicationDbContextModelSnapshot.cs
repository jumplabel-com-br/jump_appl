﻿// <auto-generated />
using System;
using CoreUI.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreUI.Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CoreUI.Web.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Active");

                    b.Property<int>("Appointment");

                    b.Property<int>("Change_Password");

                    b.Property<string>("Contract_Mode");

                    b.Property<string>("Document");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<double>("Salary");

                    b.HasKey("Id");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Hour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Activies");

                    b.Property<DateTime>("Arrival_Time");

                    b.Property<DateTime>("Beginning_Of_The_Break");

                    b.Property<string>("Consultant");

                    b.Property<DateTime>("Creation_Date");

                    b.Property<DateTime>("Date");

                    b.Property<int>("Employee_Id");

                    b.Property<DateTime>("End_Of_The_Break");

                    b.Property<DateTime>("Exit_Time");

                    b.Property<int>("Id_Project");

                    b.Property<string>("Project");

                    b.Property<TimeSpan>("Start_Time");

                    b.Property<TimeSpan>("Start_Time_2");

                    b.Property<TimeSpan>("Stop_Time");

                    b.Property<TimeSpan>("Stop_Time_2");

                    b.Property<TimeSpan>("Total_Activies_Hours");

                    b.Property<DateTime>("Total_Hours_In_Activity");

                    b.HasKey("Id");

                    b.ToTable("Hour");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Active");

                    b.Property<int>("Client_Id");

                    b.Property<long>("Cost_Center_Id");

                    b.Property<string>("Project_Name");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });
#pragma warning restore 612, 618
        }
    }
}
