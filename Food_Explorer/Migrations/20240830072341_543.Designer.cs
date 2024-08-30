﻿// <auto-generated />
using System;
using Food_Explorer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Food_Explorer.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20240830072341_543")]
    partial class _543
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Food_Explorer.Entities.Basket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId")
                        .IsUnique();

                    b.ToTable("Baskets");
                });

            modelBuilder.Entity("Food_Explorer.Entities.BasketItem", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("BasketId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BasketId");

                    b.ToTable("BasketItems");
                });

            modelBuilder.Entity("Food_Explorer.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<bool>("IsComplited")
                        .HasColumnType("bit");

                    b.Property<int>("OrderStateNum")
                        .HasColumnType("int");

                    b.Property<int>("PaymentType")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Food_Explorer.Entities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Food_Explorer.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Ingredients")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("ProductType")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasDiscriminator<int>("ProductType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Food_Explorer.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<int>("UserType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Food_Explorer.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ZipCOde")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("Food_Explorer.Entities.Drink", b =>
                {
                    b.HasBaseType("Food_Explorer.Entities.Product");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("Food_Explorer.Entities.Entree", b =>
                {
                    b.HasBaseType("Food_Explorer.Entities.Product");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Food_Explorer.Entities.Salad", b =>
                {
                    b.HasBaseType("Food_Explorer.Entities.Product");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("Food_Explorer.Entities.Soup", b =>
                {
                    b.HasBaseType("Food_Explorer.Entities.Product");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Food_Explorer.Entities.Admin", b =>
                {
                    b.HasBaseType("Food_Explorer.Entities.User");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Food_Explorer.Entities.Client", b =>
                {
                    b.HasBaseType("Food_Explorer.Entities.User");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("Food_Explorer.Entities.Basket", b =>
                {
                    b.HasOne("Food_Explorer.Entities.User", "User")
                        .WithOne("Basket")
                        .HasForeignKey("Food_Explorer.Entities.Basket", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Food_Explorer.Entities.BasketItem", b =>
                {
                    b.HasOne("Food_Explorer.Entities.Basket", "Basket")
                        .WithMany("BasketItems")
                        .HasForeignKey("BasketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Food_Explorer.Entities.Product", "Product")
                        .WithOne("BasketItem")
                        .HasForeignKey("Food_Explorer.Entities.BasketItem", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Basket");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Food_Explorer.Entities.Order", b =>
                {
                    b.HasOne("Food_Explorer.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Food_Explorer.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Food_Explorer.Entities.OrderItem", b =>
                {
                    b.HasOne("Food_Explorer.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Food_Explorer.Entities.Product", "Product")
                        .WithMany("OrderItem")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Food_Explorer.Entities.Basket", b =>
                {
                    b.Navigation("BasketItems");
                });

            modelBuilder.Entity("Food_Explorer.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Food_Explorer.Entities.Product", b =>
                {
                    b.Navigation("BasketItem")
                        .IsRequired();

                    b.Navigation("OrderItem");
                });

            modelBuilder.Entity("Food_Explorer.Entities.User", b =>
                {
                    b.Navigation("Basket")
                        .IsRequired();

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
