﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Events.Data;

#nullable disable

namespace ThAmCo.Events.Data.Migrations
{
    [DbContext(typeof(EventsDBContext))]
    [Migration("20221215133329_AdditionOfFirstAiderToEvents")]
    partial class AdditionOfFirstAiderToEvents
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("ThAmCo.Events.Data.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientReferenceId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EventTypeId")
                        .HasMaxLength(3)
                        .HasColumnType("TEXT");

                    b.Property<bool?>("HasFirstAider")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Reference")
                        .HasMaxLength(13)
                        .HasColumnType("TEXT");

                    b.HasKey("EventId");

                    b.ToTable("Event");

                    b.HasData(
                        new
                        {
                            EventId = 1,
                            ClientReferenceId = 1,
                            EventDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EventName = "The North Family Birthday bash",
                            EventTypeId = "PTY",
                            IsDeleted = false,
                            Reference = "Reference1234"
                        },
                        new
                        {
                            EventId = 2,
                            ClientReferenceId = 2,
                            EventDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EventName = "The annual golf meet up",
                            EventTypeId = "MET",
                            IsDeleted = false,
                            Reference = "Reference2345"
                        });
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Guest", b =>
                {
                    b.Property<int>("GuestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Forename")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Telephone")
                        .HasColumnType("INTEGER");

                    b.HasKey("GuestId");

                    b.ToTable("Guest");

                    b.HasData(
                        new
                        {
                            GuestId = 1,
                            Address = "21 jump street",
                            Email = "RichardNorth@email.com",
                            Forename = "Richard",
                            IsDeleted = false,
                            Surname = "North",
                            Telephone = 164266223
                        },
                        new
                        {
                            GuestId = 2,
                            Address = "22 hop street",
                            Email = "DanielNorth@email.com",
                            Forename = "Daniel",
                            IsDeleted = false,
                            Surname = "North",
                            Telephone = 164263223
                        },
                        new
                        {
                            GuestId = 3,
                            Address = "23 skip street",
                            Email = "MichealNorth@email.com",
                            Forename = "Micheal",
                            IsDeleted = false,
                            Surname = "North",
                            Telephone = 1642642223
                        },
                        new
                        {
                            GuestId = 4,
                            Address = "24 run street",
                            Email = "RobertNorth@email.com",
                            Forename = "Robert",
                            IsDeleted = false,
                            Surname = "North",
                            Telephone = 163366223
                        },
                        new
                        {
                            GuestId = 5,
                            Address = "25 skid street",
                            Email = "AidanNorth@email.com",
                            Forename = "Aidan",
                            IsDeleted = false,
                            Surname = "North",
                            Telephone = 164266213
                        },
                        new
                        {
                            GuestId = 6,
                            Address = "26 slide street",
                            Email = "ThomasNorth@email.com",
                            Forename = "Thomas",
                            IsDeleted = false,
                            Surname = "North",
                            Telephone = 164266923
                        });
                });

            modelBuilder.Entity("ThAmCo.Events.Data.GuestBooking", b =>
                {
                    b.Property<int>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GuestId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Attended")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.HasKey("EventId", "GuestId");

                    b.HasIndex("GuestId");

                    b.ToTable("GuestBooking");

                    b.HasData(
                        new
                        {
                            EventId = 1,
                            GuestId = 1,
                            Attended = false,
                            IsDeleted = false
                        },
                        new
                        {
                            EventId = 1,
                            GuestId = 2,
                            Attended = false,
                            IsDeleted = false
                        },
                        new
                        {
                            EventId = 1,
                            GuestId = 3,
                            Attended = false,
                            IsDeleted = false
                        },
                        new
                        {
                            EventId = 1,
                            GuestId = 4,
                            Attended = false,
                            IsDeleted = false
                        },
                        new
                        {
                            EventId = 1,
                            GuestId = 5,
                            Attended = false,
                            IsDeleted = false
                        },
                        new
                        {
                            EventId = 2,
                            GuestId = 1,
                            Attended = false,
                            IsDeleted = false
                        },
                        new
                        {
                            EventId = 2,
                            GuestId = 6,
                            Attended = false,
                            IsDeleted = false
                        },
                        new
                        {
                            EventId = 2,
                            GuestId = 4,
                            Attended = false,
                            IsDeleted = false
                        });
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Staff", b =>
                {
                    b.Property<int>("StaffId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("FirstAidQualified")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Forename")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("JobRole")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("StaffId");

                    b.ToTable("Staff");

                    b.HasData(
                        new
                        {
                            StaffId = 1,
                            Email = "chelseacopeland@email.com",
                            FirstAidQualified = true,
                            Forename = "Chelsea",
                            IsDeleted = false,
                            JobRole = 0,
                            Password = "password",
                            Surname = "Copland"
                        },
                        new
                        {
                            StaffId = 2,
                            Email = "michellecopeland@email.com",
                            FirstAidQualified = false,
                            Forename = "michelle",
                            IsDeleted = false,
                            JobRole = 2,
                            Password = "password",
                            Surname = "Copland"
                        },
                        new
                        {
                            StaffId = 3,
                            Email = "Carlycopeland@email.com",
                            FirstAidQualified = false,
                            Forename = "Carly",
                            IsDeleted = false,
                            JobRole = 2,
                            Password = "password",
                            Surname = "Copland"
                        },
                        new
                        {
                            StaffId = 4,
                            Email = "ciaracopeland@email.com",
                            FirstAidQualified = true,
                            Forename = "ciara",
                            IsDeleted = false,
                            JobRole = 2,
                            Password = "password",
                            Surname = "Copland"
                        },
                        new
                        {
                            StaffId = 5,
                            Email = "annacopeland@email.com",
                            FirstAidQualified = false,
                            Forename = "anna",
                            IsDeleted = false,
                            JobRole = 2,
                            Password = "password",
                            Surname = "Copland"
                        },
                        new
                        {
                            StaffId = 6,
                            Email = "deecopeland@email.com",
                            FirstAidQualified = true,
                            Forename = "dee",
                            IsDeleted = false,
                            JobRole = 1,
                            Password = "password",
                            Surname = "Copland"
                        });
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Staffing", b =>
                {
                    b.Property<int>("StaffId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.HasKey("StaffId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("Staffing");

                    b.HasData(
                        new
                        {
                            StaffId = 1,
                            EventId = 1,
                            IsDeleted = false
                        },
                        new
                        {
                            StaffId = 2,
                            EventId = 1,
                            IsDeleted = false
                        },
                        new
                        {
                            StaffId = 3,
                            EventId = 1,
                            IsDeleted = false
                        },
                        new
                        {
                            StaffId = 4,
                            EventId = 1,
                            IsDeleted = false
                        },
                        new
                        {
                            StaffId = 3,
                            EventId = 2,
                            IsDeleted = false
                        },
                        new
                        {
                            StaffId = 1,
                            EventId = 2,
                            IsDeleted = false
                        },
                        new
                        {
                            StaffId = 6,
                            EventId = 2,
                            IsDeleted = false
                        });
                });

            modelBuilder.Entity("ThAmCo.Events.Data.GuestBooking", b =>
                {
                    b.HasOne("ThAmCo.Events.Data.Event", "Event")
                        .WithMany("GuestBookings")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThAmCo.Events.Data.Guest", "Guest")
                        .WithMany("Events")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Guest");
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Staffing", b =>
                {
                    b.HasOne("ThAmCo.Events.Data.Event", "Event")
                        .WithMany("Staffs")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThAmCo.Events.Data.Staff", "Staff")
                        .WithMany("Events")
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Event", b =>
                {
                    b.Navigation("GuestBookings");

                    b.Navigation("Staffs");
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Guest", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Staff", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
