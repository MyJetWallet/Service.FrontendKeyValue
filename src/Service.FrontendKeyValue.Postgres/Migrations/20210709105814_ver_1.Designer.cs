﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Service.FrontendKeyValue.Postgres;

namespace Service.FrontendKeyValue.Postgres.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20210709105814_ver_1")]
    partial class ver_1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("frontend_key_value")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Service.FrontendKeyValue.Postgres.FrontKeyValueDbEntity", b =>
                {
                    b.Property<string>("ClientId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Key")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("ClientId", "Key");

                    b.HasIndex("ClientId");

                    b.ToTable("key_value");
                });
#pragma warning restore 612, 618
        }
    }
}
