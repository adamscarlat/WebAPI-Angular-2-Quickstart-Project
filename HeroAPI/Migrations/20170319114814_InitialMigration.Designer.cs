using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HeroAPI.Data;

namespace HeroAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170319114814_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.2");

            modelBuilder.Entity("Hero", b =>
                {
                    b.Property<int>("HeroId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HeroName")
                        .IsRequired();

                    b.HasKey("HeroId");

                    b.ToTable("Hero");
                });
        }
    }
}
