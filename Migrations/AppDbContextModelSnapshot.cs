﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using zulu.Data;
using zulu.Models;

namespace zulu.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("zulu.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllDay");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("End");

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name");

                    b.Property<DateTime>("Start");

                    b.Property<int>("State");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("zulu.Models.EventReport", b =>
                {
                    b.Property<int>("EventId");

                    b.Property<int>("ReportId");

                    b.HasKey("EventId", "ReportId");

                    b.HasIndex("ReportId")
                        .IsUnique();

                    b.ToTable("EventReport");
                });

            modelBuilder.Entity("zulu.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Content");

                    b.Property<DateTime>("Created");

                    b.Property<int?>("EventId");

                    b.Property<DateTime>("Modified");

                    b.Property<int>("State");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("zulu.Models.EventReport", b =>
                {
                    b.HasOne("zulu.Models.Event", "Event")
                        .WithMany("EventReports")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("zulu.Models.Report", "Report")
                        .WithOne("EventReport")
                        .HasForeignKey("zulu.Models.EventReport", "ReportId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("zulu.Models.Report", b =>
                {
                    b.HasOne("zulu.Models.Event", "Event")
                        .WithMany("Reports")
                        .HasForeignKey("EventId");
                });
#pragma warning restore 612, 618
        }
    }
}