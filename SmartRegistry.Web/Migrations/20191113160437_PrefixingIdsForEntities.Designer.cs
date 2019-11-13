﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartRegistry.Web.Data;

namespace SmartRegistry.Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191113160437_PrefixingIdsForEntities")]
    partial class PrefixingIdsForEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressType");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Postal")
                        .IsRequired();

                    b.Property<string>("PostalCode")
                        .IsRequired();

                    b.Property<string>("Residential")
                        .IsRequired();

                    b.HasKey("AddressId");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Announcement", b =>
                {
                    b.Property<int>("AnnouncementId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnnouncementTypeId");

                    b.Property<int>("AnnouncementTypeOwnerId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("LecturerId");

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<string>("Title");

                    b.HasKey("AnnouncementId");

                    b.HasIndex("LecturerId");

                    b.ToTable("Announcement");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.AnnouncementType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("AnnouncementType");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Attended", b =>
                {
                    b.Property<int>("AttendedId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<bool>("HasAttended");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("ScheduleId");

                    b.Property<int>("StudentId");

                    b.HasKey("AttendedId");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("StudentId");

                    b.ToTable("Attendee");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Contact", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CellPhone")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("TelePhone")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("ContactId");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<int>("DepartmentId");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("CourseId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("FacultyId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("DepartmentId");

                    b.HasIndex("FacultyId");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.EnrolledSubject", b =>
                {
                    b.Property<int>("EnrolledSubjectId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("StudentId");

                    b.Property<int>("SubjectId");

                    b.HasKey("EnrolledSubjectId");

                    b.HasIndex("StudentId");

                    b.HasIndex("SubjectId");

                    b.ToTable("EnrolledSubject");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Events", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Description")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Faculty", b =>
                {
                    b.Property<int>("FacultyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("FacultyId");

                    b.ToTable("Faculty");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Lecturer", b =>
                {
                    b.Property<int>("LecturerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<int>("Age");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DOB");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.HasKey("LecturerId");

                    b.ToTable("Lecturer");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Schedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ColorTheme");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<bool>("IsConfirmed");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("LectureRoom")
                        .IsRequired();

                    b.Property<DateTime>("ScheduleFor");

                    b.Property<DateTime>("ScheduleTo");

                    b.Property<int>("SubjectId");

                    b.HasKey("ScheduleId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Schedule");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Sensor", b =>
                {
                    b.Property<int>("SensorId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<bool>("IsAssigned");

                    b.Property<DateTime>("LastModifiedAt");

                    b.Property<string>("LastModifiedBy");

                    b.HasKey("SensorId");

                    b.ToTable("Sensor");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountId");

                    b.Property<int>("Age");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DOB");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.Property<bool>("IsComplete");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("SensorId");

                    b.Property<long>("StudentNumber");

                    b.HasKey("StudentId");

                    b.HasIndex("SensorId");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Subject", b =>
                {
                    b.Property<int>("SubjectId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<int>("CourseId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DeletedAt");

                    b.Property<string>("DeletedBy");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastUpdatedAt");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("LecturerId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("SubjectId");

                    b.HasIndex("CourseId");

                    b.HasIndex("LecturerId");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SmartRegistry.Web.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SmartRegistry.Web.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartRegistry.Web.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SmartRegistry.Web.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Announcement", b =>
                {
                    b.HasOne("SmartRegistry.Web.Models.Lecturer", "Lecturer")
                        .WithMany()
                        .HasForeignKey("LecturerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Attended", b =>
                {
                    b.HasOne("SmartRegistry.Web.Models.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartRegistry.Web.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Course", b =>
                {
                    b.HasOne("SmartRegistry.Web.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Department", b =>
                {
                    b.HasOne("SmartRegistry.Web.Models.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.EnrolledSubject", b =>
                {
                    b.HasOne("SmartRegistry.Web.Models.Student", "Student")
                        .WithMany("EnrolledSubjects")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartRegistry.Web.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Schedule", b =>
                {
                    b.HasOne("SmartRegistry.Web.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Student", b =>
                {
                    b.HasOne("SmartRegistry.Web.Models.Sensor", "Sensor")
                        .WithMany()
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartRegistry.Web.Models.Subject", b =>
                {
                    b.HasOne("SmartRegistry.Web.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartRegistry.Web.Models.Lecturer", "Lecturer")
                        .WithMany()
                        .HasForeignKey("LecturerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
