﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlaystationWishlist.DataAccess.Data;

namespace PlaystationWishlist.DataAccess.Migrations
{
    [DbContext(typeof(PlaystationWishlistContext))]
    [Migration("20201126094946_2020-11-26-02")]
    partial class _2020112602
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("PlaystationWishlist.DataAccess.Models.PlaystationGame", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DiscountDescriptor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("FinalPrice")
                        .HasColumnType("decimal (18,2)");

                    b.Property<DateTime>("LastUpdataded")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("OriginalPrice")
                        .HasColumnType("decimal (18,2)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PlaystationGames");
                });
#pragma warning restore 612, 618
        }
    }
}
