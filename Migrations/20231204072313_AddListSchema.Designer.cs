﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using csci340_iseegreen.Data;

#nullable disable

namespace csci340_iseegreen.Migrations
{
    [DbContext(typeof(ISeeGreenContext))]
    [Migration("20231204072313_AddListSchema")]
    partial class AddListSchema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("ISeeGreenUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Categories", b =>
                {
                    b.Property<string>("Category")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<long>("APG4sort")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("Sort")
                        .HasColumnType("INTEGER");

                    b.HasKey("Category");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Families", b =>
                {
                    b.Property<string>("Family")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("CategoryID")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("TaxonomicOrderID")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("TranslateTo")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("Family");

                    b.HasIndex("CategoryID");

                    b.HasIndex("TaxonomicOrderID");

                    b.ToTable("Families", (string)null);
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Genera", b =>
                {
                    b.Property<string>("GenusID")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("FamilyID")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("KewID")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("GenusID");

                    b.HasIndex("FamilyID");

                    b.ToTable("Genera", (string)null);
                });

            modelBuilder.Entity("csci340_iseegreen.Models.ListItems", b =>
                {
                    b.Property<string>("KewID")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("ListID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LocationID")
                        .HasMaxLength(255)
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TimeDiscovered")
                        .HasColumnType("TEXT");

                    b.HasKey("KewID", "ListID");

                    b.HasIndex("ListID");

                    b.HasIndex("LocationID");

                    b.ToTable("ListItems");
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Lists", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerID")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerID");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Locations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double?>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerID")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerID");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Synonyms", b =>
                {
                    b.Property<string>("KewID")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Authors")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Genus")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("InfraspecificEpithet")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Species")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("TaxaID")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("TaxonRank")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("KewID");

                    b.HasIndex("TaxaID");

                    b.ToTable("Synonyms", (string)null);
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Taxa", b =>
                {
                    b.Property<string>("KewID")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Authors")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("GenusID")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("HybridGenus")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("HybridSpecies")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("InfraspecificEpithet")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("SpecificEpithet")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("TaxonRank")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("USDAsymbol")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("USDAsynonym")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("KewID");

                    b.HasIndex("GenusID");

                    b.ToTable("Taxa", (string)null);
                });

            modelBuilder.Entity("csci340_iseegreen.Models.TaxonomicOrders", b =>
                {
                    b.Property<string>("TaxonomicOrder")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("SortLevel1")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SortLevel1Heading")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("SortLevel2")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SortLevel2Heading")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("SortLevel3")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SortLevel3Heading")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("SortLevel4")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SortLevel4Heading")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("SortLevel5")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SortLevel5Heading")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("SortLevel6")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SortLevel6Heading")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("TaxonomicOrder");

                    b.ToTable("TaxonomicOrders", (string)null);
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

            modelBuilder.Entity("csci340_iseegreen.Models.Families", b =>
                {
                    b.HasOne("csci340_iseegreen.Models.Categories", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID");

                    b.HasOne("csci340_iseegreen.Models.TaxonomicOrders", "TaxonomicOrder")
                        .WithMany()
                        .HasForeignKey("TaxonomicOrderID");

                    b.Navigation("Category");

                    b.Navigation("TaxonomicOrder");
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Genera", b =>
                {
                    b.HasOne("csci340_iseegreen.Models.Families", "Family")
                        .WithMany()
                        .HasForeignKey("FamilyID");

                    b.Navigation("Family");
                });

            modelBuilder.Entity("csci340_iseegreen.Models.ListItems", b =>
                {
                    b.HasOne("csci340_iseegreen.Models.Taxa", "Plant")
                        .WithMany()
                        .HasForeignKey("KewID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("csci340_iseegreen.Models.Lists", "List")
                        .WithMany()
                        .HasForeignKey("ListID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("csci340_iseegreen.Models.Locations", "Location")
                        .WithMany()
                        .HasForeignKey("LocationID");

                    b.Navigation("List");

                    b.Navigation("Location");

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Lists", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Locations", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerID");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Synonyms", b =>
                {
                    b.HasOne("csci340_iseegreen.Models.Taxa", "Taxa")
                        .WithMany()
                        .HasForeignKey("TaxaID");

                    b.Navigation("Taxa");
                });

            modelBuilder.Entity("csci340_iseegreen.Models.Taxa", b =>
                {
                    b.HasOne("csci340_iseegreen.Models.Genera", "Genus")
                        .WithMany()
                        .HasForeignKey("GenusID");

                    b.Navigation("Genus");
                });
#pragma warning restore 612, 618
        }
    }
}
