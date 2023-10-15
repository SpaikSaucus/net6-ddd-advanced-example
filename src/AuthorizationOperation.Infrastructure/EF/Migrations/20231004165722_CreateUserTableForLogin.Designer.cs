﻿// <auto-generated />
using System;
using AuthorizationOperation.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuthorizationOperation.Infrastructure.EF.Migrations
{
    [DbContext(typeof(AuthorizationDbContext))]
    [Migration("20231004165722_CreateUserTableForLogin")]
    partial class CreateUserTableForLogin
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AuthorizationOperation.Domain.Authorization.Models.Authorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Customer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("UUID")
                        .IsRequired()
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("Authorizations");
                });

            modelBuilder.Entity("AuthorizationOperation.Domain.Authorization.Models.AuthorizationStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("authorization_status", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Name = "WAITING_FOR_SIGNERS"
                        },
                        new
                        {
                            Id = 1,
                            Name = "AUTHORIZED"
                        },
                        new
                        {
                            Id = 2,
                            Name = "EXPIRED"
                        },
                        new
                        {
                            Id = 3,
                            Name = "CANCELLED"
                        });
                });

            modelBuilder.Entity("AuthorizationOperation.Domain.User.Models.User", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("username");

                    b.HasKey("Guid");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("AuthorizationOperation.Domain.Authorization.Models.Authorization", b =>
                {
                    b.HasOne("AuthorizationOperation.Domain.Authorization.Models.AuthorizationStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");
                });
#pragma warning restore 612, 618
        }
    }
}