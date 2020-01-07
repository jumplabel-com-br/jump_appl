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

            modelBuilder.Entity("CoreUI.Web.Models.Access_Level", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Access_level");

                    b.HasKey("Id");

                    b.ToTable("Access_Level");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Description", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Active");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Description");
                });

            modelBuilder.Entity("CoreUI.Web.Models.DetailsPricing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AgeYears");

                    b.Property<double>("Cust");

                    b.Property<double>("HourConsultant");

                    b.Property<double>("HourSale");

                    b.Property<int>("HoursMonth");

                    b.Property<int>("Pricing_Id");

                    b.Property<string>("SpecialtyName");

                    b.Property<int>("TypeContract");

                    b.Property<double>("VT");

                    b.Property<double>("ValueCLTType");

                    b.HasKey("Id");

                    b.ToTable("DetailsPricing");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Access_LevelId");

                    b.Property<int>("Active");

                    b.Property<int>("Appointment");

                    b.Property<int>("Change_Password");

                    b.Property<string>("Contract_Mode");

                    b.Property<string>("Document");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Access_LevelId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Hiring", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Active");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Hiring");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Hour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Activies");

                    b.Property<int>("Approval");

                    b.Property<int>("Approver");

                    b.Property<int>("Approver_Update");

                    b.Property<DateTime>("Arrival_Time");

                    b.Property<DateTime>("Beginning_Of_The_Break");

                    b.Property<int>("Billing");

                    b.Property<string>("Consultant");

                    b.Property<DateTime>("Creation_Date");

                    b.Property<DateTime>("Date");

                    b.Property<int>("Description");

                    b.Property<int>("Employee_Id");

                    b.Property<DateTime>("End_Of_The_Break");

                    b.Property<DateTime>("Exit_Time");

                    b.Property<string>("File");

                    b.Property<int>("Id_Project");

                    b.Property<int>("LocalityId");

                    b.Property<string>("Project");

                    b.Property<int>("Register");

                    b.Property<TimeSpan>("Start_Time");

                    b.Property<TimeSpan>("Start_Time_2");

                    b.Property<TimeSpan>("Stop_Time");

                    b.Property<TimeSpan>("Stop_Time_2");

                    b.Property<TimeSpan>("Total_Activies_Hours");

                    b.Property<DateTime>("Total_Hours_In_Activity");

                    b.HasKey("Id");

                    b.ToTable("Hour");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Locality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Active");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Locality");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Outlays", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Client_Id");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<int>("Employee_Id");

                    b.Property<string>("File");

                    b.Property<string>("NoteNumber");

                    b.Property<double>("NoteValue");

                    b.Property<int>("Project_Id");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("Outlays");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Pricing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountExecutive_Id");

                    b.Property<int>("Administrator_Id");

                    b.Property<string>("Allocation");

                    b.Property<int>("AllocationManager_Id");

                    b.Property<int>("Client_Id");

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("InitialDate");

                    b.Property<int>("NumberProposal");

                    b.Property<int>("Risk");

                    b.Property<string>("TimeBetweenInitialAndEndDate");

                    b.Property<int>("TypePricing");

                    b.HasKey("Id");

                    b.ToTable("Pricing");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Active");

                    b.Property<int>("Client_Id");

                    b.Property<long>("Cost_Center_Id");

                    b.Property<int>("Manager_Id");

                    b.Property<int>("Project_Manager_Id");

                    b.Property<string>("Project_Name");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Project_team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Employee_Id");

                    b.Property<DateTime>("End_Date");

                    b.Property<int>("Project_Id");

                    b.Property<DateTime>("Start_Date");

                    b.HasKey("Id");

                    b.ToTable("Project_team");
                });

            modelBuilder.Entity("CoreUI.Web.Models.TypePricing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Active");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("TypePricing");
                });

            modelBuilder.Entity("CoreUI.Web.Models.Employee", b =>
                {
                    b.HasOne("CoreUI.Web.Models.Access_Level", "Access_Level")
                        .WithMany()
                        .HasForeignKey("Access_LevelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
