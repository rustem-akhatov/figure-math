﻿// <auto-generated />
using FigureMath.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FigureMath.Data.Migrations
{
    [DbContext(typeof(FigureMathDbContext))]
    partial class FigureMathDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("FigureMath.Data.Entities.Figure", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("FigureProps")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("figure_props");

                    b.Property<string>("FigureType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("figure_type");

                    b.HasKey("Id")
                        .HasName("pk_figure");

                    b.ToTable("figure");
                });
#pragma warning restore 612, 618
        }
    }
}
