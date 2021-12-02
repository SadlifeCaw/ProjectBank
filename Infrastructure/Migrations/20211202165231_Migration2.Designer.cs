﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectBank.Infrastructure;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ProjectBankContext))]
    [Migration("20211202165231_Migration2")]
    partial class Migration2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CourseTeachingProgram", b =>
                {
                    b.Property<int>("CoursesId")
                        .HasColumnType("int");

                    b.Property<int>("ProgramsId")
                        .HasColumnType("int");

                    b.HasKey("CoursesId", "ProgramsId");

                    b.HasIndex("ProgramsId");

                    b.ToTable("CourseTeachingProgram");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ProjectId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("InstitutionId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("InstitutionId");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.CodedCategory", b =>
                {
                    b.HasBaseType("ProjectBank.Infrastructure.Category");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FacultyId")
                        .HasColumnType("int");

                    b.HasIndex("FacultyId");

                    b.ToTable("CodedCategories", (string)null);
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Faculty", b =>
                {
                    b.HasBaseType("ProjectBank.Infrastructure.Category");

                    b.Property<int>("InstitutionId")
                        .HasColumnType("int");

                    b.HasIndex("InstitutionId");

                    b.ToTable("Faculties", (string)null);
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Institution", b =>
                {
                    b.HasBaseType("ProjectBank.Infrastructure.Category");

                    b.ToTable("Institutions", (string)null);
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Student", b =>
                {
                    b.HasBaseType("ProjectBank.Infrastructure.User");

                    b.Property<int?>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("ProgramId")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("Student_ProjectId");

                    b.HasIndex("CourseId");

                    b.HasIndex("ProgramId");

                    b.HasIndex("ProjectId");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Supervisor", b =>
                {
                    b.HasBaseType("ProjectBank.Infrastructure.User");

                    b.Property<int>("FacultyId")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.HasIndex("FacultyId");

                    b.HasIndex("ProjectId");

                    b.HasDiscriminator().HasValue("Supervisor");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Course", b =>
                {
                    b.HasBaseType("ProjectBank.Infrastructure.CodedCategory");

                    b.ToTable("Courses", (string)null);
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.TeachingProgram", b =>
                {
                    b.HasBaseType("ProjectBank.Infrastructure.CodedCategory");

                    b.ToTable("Programs", (string)null);
                });

            modelBuilder.Entity("CourseTeachingProgram", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectBank.Infrastructure.TeachingProgram", null)
                        .WithMany()
                        .HasForeignKey("ProgramsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Project", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.User", null)
                        .WithMany("Projects")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Tag", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Project", null)
                        .WithMany("Tags")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.User", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Institution", "Institution")
                        .WithMany()
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Institution");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.CodedCategory", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectBank.Infrastructure.Category", null)
                        .WithOne()
                        .HasForeignKey("ProjectBank.Infrastructure.CodedCategory", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Faculty", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Category", null)
                        .WithOne()
                        .HasForeignKey("ProjectBank.Infrastructure.Faculty", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("ProjectBank.Infrastructure.Institution", "Institution")
                        .WithMany()
                        .HasForeignKey("InstitutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Institution");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Institution", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Category", null)
                        .WithOne()
                        .HasForeignKey("ProjectBank.Infrastructure.Institution", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Student", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Course", null)
                        .WithMany("Students")
                        .HasForeignKey("CourseId");

                    b.HasOne("ProjectBank.Infrastructure.TeachingProgram", "Program")
                        .WithMany()
                        .HasForeignKey("ProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectBank.Infrastructure.Project", null)
                        .WithMany("Students")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Program");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Supervisor", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectBank.Infrastructure.Project", null)
                        .WithMany("Collaborators")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Course", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.CodedCategory", null)
                        .WithOne()
                        .HasForeignKey("ProjectBank.Infrastructure.Course", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.TeachingProgram", b =>
                {
                    b.HasOne("ProjectBank.Infrastructure.CodedCategory", null)
                        .WithOne()
                        .HasForeignKey("ProjectBank.Infrastructure.TeachingProgram", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Project", b =>
                {
                    b.Navigation("Collaborators");

                    b.Navigation("Students");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.User", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("ProjectBank.Infrastructure.Course", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
