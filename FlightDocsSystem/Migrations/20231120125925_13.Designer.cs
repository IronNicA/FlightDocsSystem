﻿// <auto-generated />
using System;
using FlightDocsSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlightDocsSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231120125925_13")]
    partial class _13
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FlightDocsSystem.Models.ManagementModels.Doc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("create_date");

                    b.Property<string>("Creator")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("create_by");

                    b.Property<string>("DocTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("doc_title");

                    b.Property<string>("DocType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("doc_type");

                    b.Property<string>("DocumentVer")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("doc_type_id");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("file_name");

                    b.HasKey("Id");

                    b.ToTable("doc");
                });

            modelBuilder.Entity("FlightDocsSystem.Models.ManagementModels.DocType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DocTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("doc_type_name");

                    b.HasKey("Id");

                    b.ToTable("doc_type");
                });

            modelBuilder.Entity("FlightDocsSystem.Models.ManagementModels.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime")
                        .HasColumnName("create_date");

                    b.Property<string>("Creator")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("create_by");

                    b.Property<string>("FlightNo")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("flight_no");

                    b.Property<string>("PoL")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("point_of_loading");

                    b.Property<string>("PoU")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("point_of_unloading");

                    b.HasKey("Id");

                    b.ToTable("flight");
                });

            modelBuilder.Entity("FlightDocsSystem.Models.ManagementModels.Role", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Creator")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("role_creator");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("role_name");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("role_normalized_name");

                    b.Property<int?>("Permission")
                        .HasColumnType("int")
                        .HasColumnName("permission_level");

                    b.HasKey("Id");

                    b.ToTable("role");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Creator = "System",
                            Name = "Admin",
                            NormalizedName = "ADMIN",
                            Permission = 2
                        },
                        new
                        {
                            Id = 2,
                            Creator = "System",
                            Name = "Crew",
                            NormalizedName = "CREW",
                            Permission = 1
                        },
                        new
                        {
                            Id = 3,
                            Creator = "System",
                            Name = "Pilot",
                            NormalizedName = "PILOT",
                            Permission = 2
                        });
                });

            modelBuilder.Entity("FlightDocsSystem.Models.ManagementModels.RolePermission", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int>("DocTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("Permission")
                        .HasColumnType("int")
                        .HasColumnName("permission");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("role");

                    b.HasKey("Id");

                    b.HasIndex("DocTypeId");

                    b.ToTable("role_permission");
                });

            modelBuilder.Entity("FlightDocsSystem.Models.ManagementModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("password_hash");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("password_salt");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("phone");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("role");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("user_name");

                    b.HasKey("Id");

                    b.ToTable("user");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@example.com",
                            PasswordHash = new byte[] { 167, 250, 207, 17, 127, 199, 147, 83, 183, 231, 237, 136, 177, 170, 14, 216, 40, 54, 186, 0, 43, 133, 2, 139, 110, 205, 115, 4, 78, 145, 252, 160, 109, 200, 73, 156, 5, 245, 34, 85, 251, 236, 223, 235, 70, 158, 167, 5, 144, 133, 100, 112, 140, 126, 204, 91, 224, 232, 222, 17, 0, 163, 148, 88 },
                            PasswordSalt = new byte[] { 136, 134, 244, 135, 221, 247, 160, 53, 196, 32, 105, 86, 202, 156, 142, 239, 19, 107, 156, 206, 116, 11, 169, 86, 34, 66, 186, 243, 80, 190, 16, 128, 204, 58, 4, 47, 106, 93, 81, 215, 164, 42, 193, 24, 36, 188, 154, 147, 240, 33, 204, 48, 88, 235, 40, 142, 242, 87, 116, 8, 67, 241, 23, 8, 116, 90, 17, 202, 120, 158, 57, 179, 48, 166, 230, 125, 103, 62, 20, 149, 174, 236, 50, 227, 222, 100, 93, 113, 229, 81, 93, 90, 179, 71, 196, 4, 206, 21, 171, 54, 233, 239, 184, 71, 104, 60, 92, 168, 49, 247, 40, 74, 145, 162, 31, 199, 152, 173, 60, 169, 88, 184, 120, 214, 78, 68, 121, 150 },
                            Phone = "1234567890",
                            Role = "Admin",
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("FlightDocsSystem.Models.ManagementModels.RolePermission", b =>
                {
                    b.HasOne("FlightDocsSystem.Models.ManagementModels.DocType", "DocType")
                        .WithMany("RolePermissions")
                        .HasForeignKey("DocTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DocType");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlightDocsSystem.Models.ManagementModels.DocType", b =>
                {
                    b.Navigation("RolePermissions");
                });
#pragma warning restore 612, 618
        }
    }
}
