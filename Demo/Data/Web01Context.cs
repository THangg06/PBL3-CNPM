﻿
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Demo.ModelViews;
using Demo.Data;

namespace Demo.Data
{
    public partial class Web01Context : DbContext
    {
        public Web01Context()
        {
        }

        public Web01Context(DbContextOptions<Web01Context> options)
            : base(options)
        {
        }
        public virtual DbSet<ReviewProduct> ReviewProduct { get; set; }
        public virtual DbSet<ThongKe> ThongKe { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Website> Websites { get; set; }
        public DbSet<RegisterViewModel> RegisterViewModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA586F4B202B7");

                entity.Property(e => e.AccountId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccountID");
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.FullName).HasMaxLength(255);
                entity.Property(e => e.LastLogin).HasColumnType("datetime");
                entity.Property(e => e.Password).HasMaxLength(50);
                entity.Property(e => e.Avatar).HasMaxLength(255);
                entity.Property(e => e.Avatar).HasMaxLength(255);
                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.Salt)
                    .HasMaxLength(6)
                    .IsFixedLength();

                entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Accounts__RoleID__4AB81AF0");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CatId).HasName("PK__Categori__6A1C8ADA9C16BFB9");

                entity.Property(e => e.CatId)
                    .ValueGeneratedNever()
                    .HasColumnName("CatID");
                entity.Property(e => e.Alias).HasMaxLength(50);
                entity.Property(e => e.CatName).HasMaxLength(50);
                entity.Property(e => e.Cover).HasMaxLength(50);
                entity.Property(e => e.MetaDesc).HasMaxLength(50);
                entity.Property(e => e.MetaKey).HasMaxLength(50);
                entity.Property(e => e.ParentId).HasColumnName("ParentID");
                entity.Property(e => e.SchemaMarkup).HasMaxLength(50);
                entity.Property(e => e.Thumb).HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B85DE34277");

                entity.Property(e => e.CustomerId)
                    .ValueGeneratedNever()
                    .HasColumnName("CustomerID");
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.Avatar).HasMaxLength(255);
                entity.Property(e => e.Birthday).HasColumnType("datetime");
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsFixedLength();
                entity.Property(e => e.FullName).HasMaxLength(255);
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.LastLogin).HasColumnType("datetime");
                entity.Property(e => e.Password).HasMaxLength(50);
                entity.Property(e => e.Salt)
                    .HasMaxLength(8)
                    .IsFixedLength();
                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Order>(entity =>
            {

                entity.HasKey(e => e.OrderID).HasName("PK__Orders__C3905BAFD28E05B5");

                entity.Property(e => e.OrderID)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("OrderID");
                //entity.HasKey(e => e.OrderID).HasName("PK__Orders__C3905BAFD28E05B5");

                entity.Property(e => e.TongTien)
     .HasColumnType("decimal(18, 2)")
     .HasDefaultValue(0.00m);
                base.OnModelCreating(modelBuilder);
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
                entity.Property(e => e.ShipDate).HasColumnType("datetime");
                entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");

                entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Orders__Customer__440B1D61");
            });

            //modelBuilder.Entity<ReviewProduct>(entity =>
            //{

             

            //    entity.Property(e => e.Id)
            //        .ValueGeneratedOnAdd()
            //        .HasColumnName("Id");
            //    entity.Property(e => e.ProductId).HasColumnName("ProductID");
            //    entity.Property(e => e.UserName).HasColumnName("UserName");
            //    entity.Property(e => e.Email)
            //       .HasMaxLength(50)
            //       .IsFixedLength();
            //    entity.Property(e => e.FullName).HasMaxLength(255);
            //    entity.Property(e => e.Avatar).HasMaxLength(255);
            //    entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            //    entity.Property(e => e.Content).HasMaxLength(250);
            //    entity.Property(e => e.Rate).HasColumnType("int");

            //});

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C1C354CDE");

                entity.Property(e => e.OrderDetailId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("OrderDetailID");
                entity.Property(e => e.OrderID).HasColumnName("OrderID");
                entity.Property(e => e.ProductId).HasColumnName("ProductID");
                entity.Property(e => e.ShipDate).HasColumnType("datetime");
                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValue(0.00m);

                entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderID)
                    .HasConstraintName("FK__OrderDeta__Order__46E78A0C");

                entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__OrderDeta__Produ__47DBAE45");
            });

            //modelBuilder.Entity<News>(entity =>
            //{
            //    //entity.HasKey(e => e.NewsID).HasName("PK__Pages__C565B12403AECA26");

            //    entity.Property(e => e.NewsID)
            //        .ValueGeneratedNever()
            //        .HasColumnName("NewsID");
            //    entity.Property(e => e.Title).HasMaxLength(250);
            //    entity.Property(e => e.DatePosted).HasColumnType("datetime");
            //    entity.Property(e => e.Content).HasMaxLength(250);
            //    entity.Property(e => e.Email)
            //          .HasMaxLength(50)
            //          .IsFixedLength();
            //    entity.Property(e => e.Phone)
            //        .HasMaxLength(10)
            //        .IsUnicode(false);
            //    entity.Property(e => e.Active).HasColumnName("Active");

            //});

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED5BAE84B5");

                entity.Property(e => e.ProductId)
                    .ValueGeneratedNever()
                    .HasColumnName("ProductID");
                entity.Property(e => e.Alias).HasMaxLength(255);
                entity.Property(e => e.CatId).HasColumnName("CatID");
               
                entity.Property(e => e.DateCreated).HasColumnType("datetime");
                entity.Property(e => e.DateModified).HasColumnType("datetime");
                entity.Property(e => e.MetaDesc).HasMaxLength(255);
                entity.Property(e => e.MetaKey).HasMaxLength(255);
                entity.Property(e => e.ProductName).HasMaxLength(255);
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValue(0.00m);
                entity.Property(e => e.ShortDesc).HasMaxLength(255);
                entity.Property(e => e.Thumb).HasMaxLength(255);
                entity.Property(e => e.Title).HasMaxLength(255);
                entity.Property(e => e.Video).HasMaxLength(255);

                entity.HasOne(d => d.Cat).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CatId)
                    .HasConstraintName("FK__Products__CatID__3D5E1FD2");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A8127F500");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoleID");
                entity.Property(e => e.Description).HasMaxLength(50);
                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.HasKey(e => e.ShipperId).HasName("PK__Shippers__1F8AFFB9DDA877E4");

                entity.Property(e => e.ShipperId)
                    .ValueGeneratedNever()
                    .HasColumnName("ShipperID");
                entity.Property(e => e.Company).HasMaxLength(255);
                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength();
                entity.Property(e => e.ShipDate).HasColumnType("datetime");
                entity.Property(e => e.ShipperName).HasMaxLength(255);
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(e => e.Idnv).HasName("PK__Staff__B87DC9B21D59429C");

                entity.Property(e => e.Idnv)
                    .HasMaxLength(20)
                    .HasColumnName("IDNV");
                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");
                entity.Property(e => e.EmailNv)
                    .HasMaxLength(150)
                    .HasColumnName("EmailNV");
                entity.Property(e => e.GioiTinh).HasMaxLength(10);
                entity.Property(e => e.NameNv)
                    .HasMaxLength(20)
                    .HasColumnName("NameNV");
                entity.Property(e => e.Password)
                    .HasMaxLength(32)
                    .IsUnicode(false);
                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.RoleId).HasColumnName("Role_id");
                entity.Property(e => e.Salary).HasColumnType("decimal(10, 3)");

                entity.HasOne(d => d.Role).WithMany(p => p.Staff)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Staff__Role_id__4D94879B");
            });

            modelBuilder.Entity<Website>(entity =>
            {
                entity.HasKey(e => e.Idweb).HasName("PK__Websites__A22F292165312360");

                entity.Property(e => e.Idweb)
                    .ValueGeneratedNever()
                    .HasColumnName("IDweb");
                entity.Property(e => e.Nameweb)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StaffIdnv)
                    .HasMaxLength(20)
                    .HasColumnName("Staff_IDNV");
                entity.Property(e => e.Url)
                    .HasMaxLength(250)
                    .HasColumnName("URL");

                entity.HasOne(d => d.StaffIdnvNavigation).WithMany(p => p.Websites)
                    .HasForeignKey(d => d.StaffIdnv)
                    .HasConstraintName("FK__Websites__Staff___5070F446");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public DbSet<Demo.Data.News> News { get; set; } = default!;
    }
}
