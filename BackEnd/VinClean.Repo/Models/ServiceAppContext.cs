﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VinClean.Repo.Models;

public partial class ServiceAppContext : DbContext
{
    public ServiceAppContext()
    {
    }

    public ServiceAppContext(DbContextOptions<ServiceAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<FinshedBy> FinshedBies { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<ProcessDetail> ProcessDetails { get; set; }

    public virtual DbSet<ProcessSlot> ProcessSlots { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceManage> ServiceManages { get; set; }

    public virtual DbSet<ServiceWorkIn> ServiceWorkIns { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<WorkingSlot> WorkingSlots { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=26.114.230.87;Initial Catalog=ServiceApp;User ID=sa;Password=12345;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__46A222CDF79FF15E");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__AB6E61640ACF4B95").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Account__role_id__68487DD7");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blog__2975AA2896391D7D");

            entity.ToTable("Blog");

            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Content)
                .HasColumnType("ntext")
                .HasColumnName("content");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.ModifiedDate).HasColumnType("date");
            entity.Property(e => e.Sumarry)
                .HasMaxLength(255)
                .HasColumnName("sumarry");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.Category).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Blog__category_i__693CA210");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BlogCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Blog__CreatedBy__6A30C649");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BlogModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Blog__ModifiedBy__6B24EA82");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B4757E975C");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Category1)
                .HasMaxLength(100)
                .HasColumnName("category");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__E795768756D7894E");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.Content)
                .HasColumnType("ntext")
                .HasColumnName("content");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.ModifiedDate).HasColumnType("date");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK__Comment__blog_id__6C190EBB");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CommentCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Comment__Created__6D0D32F4");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CommentModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Comment__Modifie__6E01572D");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB85DDFF69D3");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Phone, "UQ__Customer__B43B145FF2EADE3C").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalMoney)
                .HasColumnType("money")
                .HasColumnName("total_money");
            entity.Property(e => e.TotalPoint).HasColumnName("total_point");

            entity.HasOne(d => d.Account).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Customer__accoun__6EF57B66");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__C52E0BA84BA556BD");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Account).WithMany(p => p.Employees)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Employee__accoun__6FE99F9F");
        });

        modelBuilder.Entity<FinshedBy>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("FinshedBy");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");

            entity.HasOne(d => d.Employee).WithMany()
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__FinshedBy__emplo__70DDC3D8");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__FinshedBy__order__71D1E811");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__4659622911F0F658");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.FinishedDate).HasColumnType("date");
            entity.Property(e => e.Note)
                .HasColumnType("ntext")
                .HasColumnName("note");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.Total)
                .HasColumnType("money")
                .HasColumnName("total");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Order__customer___72C60C4A");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Order_Detail");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Slot)
                .HasDefaultValueSql("((1))")
                .HasColumnName("slot");
            entity.Property(e => e.Total)
                .HasColumnType("money")
                .HasColumnName("total");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Order_Det__order__73BA3083");

            entity.HasOne(d => d.Service).WithMany()
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Order_Det__servi__74AE54BC");
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.HasKey(e => e.ProcessId).HasName("PK__Process__9446C3E11F8CDEF3");

            entity.ToTable("Process");

            entity.Property(e => e.ProcessId).HasColumnName("process_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.ModifiedDate).HasColumnType("date");
            entity.Property(e => e.Note)
                .HasColumnType("ntext")
                .HasColumnName("note");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Customer).WithMany(p => p.Processes)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Process__custome__75A278F5");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.Processes)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Process__Modifie__76969D2E");
        });

        modelBuilder.Entity<ProcessDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Process_Detail");

            entity.Property(e => e.ProcessId).HasColumnName("process_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Process).WithMany()
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK__Process_D__proce__778AC167");

            entity.HasOne(d => d.Service).WithMany()
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Process_D__servi__787EE5A0");
        });

        modelBuilder.Entity<ProcessSlot>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Process_Slot");

            entity.Property(e => e.ProcessId).HasColumnName("process_id");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");

            entity.HasOne(d => d.Process).WithMany()
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK__Process_S__proce__797309D9");

            entity.HasOne(d => d.Slot).WithMany()
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("FK__Process_S__slot___7A672E12");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RateId).HasName("PK__Rating__75920B42162C020F");

            entity.ToTable("Rating");

            entity.Property(e => e.RateId).HasColumnName("rate_id");
            entity.Property(e => e.Comment)
                .HasColumnType("ntext")
                .HasColumnName("comment");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.ModifiedDate).HasColumnType("date");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Rating__customer__7B5B524B");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Rating__Modified__7C4F7684");

            entity.HasOne(d => d.Service).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Rating__service___7D439ABD");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CC0DB6348E");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__3E0DB8AF7370F740");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Avaiable)
                .HasDefaultValueSql("((1))")
                .HasColumnName("avaiable");
            entity.Property(e => e.CostPerSlot)
                .HasColumnType("money")
                .HasColumnName("cost_per_slot");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.Description)
                .HasColumnType("ntext")
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.MinimalSlot).HasColumnName("minimal_slot");
            entity.Property(e => e.ModifiedDate).HasColumnType("date");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasColumnName("status");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ServiceCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Service__Created__7E37BEF6");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ServiceModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Service__Modifie__7F2BE32F");

            entity.HasOne(d => d.Type).WithMany(p => p.Services)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Service__type_id__00200768");
        });

        modelBuilder.Entity<ServiceManage>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Service_Manage");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");

            entity.HasOne(d => d.Employee).WithMany()
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Service_M__emplo__01142BA1");

            entity.HasOne(d => d.Service).WithMany()
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Service_M__servi__02084FDA");
        });

        modelBuilder.Entity<ServiceWorkIn>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Service_WorkIn");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");

            entity.HasOne(d => d.Employee).WithMany()
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Service_W__emplo__02FC7413");

            entity.HasOne(d => d.Service).WithMany()
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Service_W__servi__03F0984C");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__Slot__971A01BBF999849F");

            entity.ToTable("Slot");

            entity.Property(e => e.SlotId).HasColumnName("slot_id");
            entity.Property(e => e.DayOfweek).HasColumnName("dayOfweek");
            entity.Property(e => e.EndTime).HasColumnName("endTime");
            entity.Property(e => e.SlotName)
                .HasMaxLength(50)
                .HasColumnName("slot_name");
            entity.Property(e => e.StartTime).HasColumnName("startTime");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Type__2C00059809F82ADD");

            entity.ToTable("Type");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.Avaiable)
                .HasDefaultValueSql("((1))")
                .HasColumnName("avaiable");
            entity.Property(e => e.Type1)
                .HasMaxLength(100)
                .HasColumnName("type");
        });

        modelBuilder.Entity<WorkingSlot>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Working_Slot");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");

            entity.HasOne(d => d.Employee).WithMany()
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Working_S__emplo__04E4BC85");

            entity.HasOne(d => d.Slot).WithMany()
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("FK__Working_S__slot___05D8E0BE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
