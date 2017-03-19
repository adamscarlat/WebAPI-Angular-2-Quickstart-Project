using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using HeroAPI.Data;

namespace HeroAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
