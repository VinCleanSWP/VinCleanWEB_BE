using System;
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

    public virtual DbSet<ProcessImage> ProcessImages { get; set; }

    public virtual DbSet<ProcessSlot> ProcessSlots { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceManage> ServiceManages { get; set; }

    public virtual DbSet<ServiceWorkIn> ServiceWorkIns { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<WorkingBy> WorkingBies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=trongps-swp.database.windows.net;Initial Catalog=ServiceApp;Persist Security Info=True;User ID=swp;Password=GNBUbgCAZ857m2;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__46A222CD0311171A");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__AB6E61646867CFAC").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.Dob).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
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
            entity.Property(e => e.PasswordResetToken).IsUnicode(false);
            entity.Property(e => e.ResetTokenExpires).HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.VerificationToken).IsUnicode(false);
            entity.Property(e => e.VerifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Account__role_id__10566F31");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blog__2975AA288E30DB72");

            entity.ToTable("Blog");

            entity.Property(e => e.BlogId).HasColumnName("blog_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Content)
                .HasColumnType("ntext")
                .HasColumnName("content");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
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
                .HasConstraintName("FK__Blog__category_i__114A936A");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BlogCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Blog__CreatedBy__123EB7A3");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BlogModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Blog__ModifiedBy__1332DBDC");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B4D90B8F43");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Category1)
                .HasMaxLength(100)
                .HasColumnName("category");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__E79576878C7676ED");

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
                .HasConstraintName("FK__Comment__blog_id__14270015");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CommentCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK__Comment__Created__151B244E");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CommentModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Comment__Modifie__160F4887");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB85FB8B1017");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Phone, "UQ__Customer__B43B145FC71E2BC1").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("dob");
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
                .HasConstraintName("FK__Customer__accoun__17036CC0");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__C52E0BA845F41A8C");

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
                .HasConstraintName("FK__Employee__accoun__17F790F9");
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
                .HasConstraintName("FK__FinshedBy__emplo__18EBB532");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__FinshedBy__order__19DFD96B");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__46596229AA1568DA");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DateWork)
                .HasColumnType("date")
                .HasColumnName("date_work");
            entity.Property(e => e.FinishedDate).HasColumnType("date");
            entity.Property(e => e.Note)
                .HasColumnType("ntext")
                .HasColumnName("note");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.PointUsed).HasColumnName("Point_used");
            entity.Property(e => e.SubPrice).HasColumnType("money");
            entity.Property(e => e.Total)
                .HasColumnType("money")
                .HasColumnName("total");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Order__customer___1AD3FDA4");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Order_Detail");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.RateId).HasColumnName("rate_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Slot)
                .HasDefaultValueSql("((1))")
                .HasColumnName("slot");
            entity.Property(e => e.Total)
                .HasColumnType("money")
                .HasColumnName("total");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Order_Det__order__6477ECF3");

            entity.HasOne(d => d.Rate).WithMany()
                .HasForeignKey(d => d.RateId)
                .HasConstraintName("FK_Order_Detail_Rating");

            entity.HasOne(d => d.Service).WithMany()
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Order_Det__servi__656C112C");
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.HasKey(e => e.ProcessId).HasName("PK__Process__9446C3E126BC7ECB");

            entity.ToTable("Process");

            entity.Property(e => e.ProcessId).HasColumnName("process_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.ModifiedDate).HasColumnType("date");
            entity.Property(e => e.Note)
                .HasColumnType("ntext")
                .HasColumnName("note");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.PointUsed).HasColumnName("Point_used");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.SubPrice).HasColumnType("money");

            entity.HasOne(d => d.Customer).WithMany(p => p.Processes)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Process__custome__1EA48E88");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.Processes)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Process__Modifie__1F98B2C1");
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
                .HasConstraintName("FK__Process_D__proce__208CD6FA");

            entity.HasOne(d => d.Service).WithMany()
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Process_D__servi__2180FB33");
        });

        modelBuilder.Entity<ProcessImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProcessI__3213E83F4D0AD281");

            entity.ToTable("ProcessImage");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProcessId).HasColumnName("process_id");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("type");

            entity.HasOne(d => d.Order).WithMany(p => p.ProcessImages)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__ProcessIm__order__2739D489");

            entity.HasOne(d => d.Process).WithMany(p => p.ProcessImages)
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK__ProcessIm__proce__282DF8C2");
        });

        modelBuilder.Entity<ProcessSlot>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Process_Slot");

            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.CreateBy).HasColumnName("create_by");
            entity.Property(e => e.NewEmployeeId).HasColumnName("newEmployee_id");
            entity.Property(e => e.Note)
                .IsUnicode(false)
                .HasColumnName("note");
            entity.Property(e => e.OldEmployeeId).HasColumnName("oldEmployee_id");
            entity.Property(e => e.ProcessId).HasColumnName("process_id");
            entity.Property(e => e.Satus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("satus");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");

            entity.HasOne(d => d.CreateByNavigation).WithMany()
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("fkcreateBy_Process_Slot_Account");

            entity.HasOne(d => d.NewEmployee).WithMany()
                .HasForeignKey(d => d.NewEmployeeId)
                .HasConstraintName("fknewEmp_Process_Slot_Employee");

            entity.HasOne(d => d.OldEmployee).WithMany()
                .HasForeignKey(d => d.OldEmployeeId)
                .HasConstraintName("fk_Process_Slot_Employee");

            entity.HasOne(d => d.Process).WithMany()
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK__Process_S__proce__22751F6C");

            entity.HasOne(d => d.Slot).WithMany()
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("FK__Process_S__slot___236943A5");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RateId).HasName("PK__Rating__75920B42141D0118");

            entity.ToTable("Rating");

            entity.Property(e => e.RateId).HasColumnName("rate_id");
            entity.Property(e => e.Comment)
                .HasColumnType("ntext")
                .HasColumnName("comment");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("((0))")
                .HasColumnName("isDeleted");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Rating__customer__29221CFB");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Rating__Modified__2A164134");

            entity.HasOne(d => d.Service).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Rating__service___2B0A656D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__760965CCD59A6A89");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__3E0DB8AF1364088D");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Avaiable)
                .HasDefaultValueSql("((1))")
                .HasColumnName("avaiable");
            entity.Property(e => e.Cost)
                .HasColumnType("money")
                .HasColumnName("cost");
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
                .HasConstraintName("FK__Service__Created__2BFE89A6");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.ServiceModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__Service__Modifie__2CF2ADDF");

            entity.HasOne(d => d.Type).WithMany(p => p.Services)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Service__type_id__2DE6D218");
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
                .HasConstraintName("FK__Service_M__emplo__2EDAF651");

            entity.HasOne(d => d.Service).WithMany()
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Service_M__servi__2FCF1A8A");
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
                .HasConstraintName("FK__Service_W__emplo__30C33EC3");

            entity.HasOne(d => d.Service).WithMany()
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Service_W__servi__31B762FC");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__Slot__971A01BB21672D94");

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
            entity.HasKey(e => e.TypeId).HasName("PK__Type__2C00059814793444");

            entity.ToTable("Type");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.Avaiable)
                .HasDefaultValueSql("((1))")
                .HasColumnName("avaiable");
            entity.Property(e => e.Icon)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("icon");
            entity.Property(e => e.Img)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.Type1)
                .HasMaxLength(100)
                .HasColumnName("type");
        });

        modelBuilder.Entity<WorkingBy>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("WorkingBy");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longtitude).HasColumnName("longtitude");
            entity.Property(e => e.ProcessId).HasColumnName("process_id");

            entity.HasOne(d => d.Employee).WithMany()
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__WorkingBy__emplo__32AB8735");

            entity.HasOne(d => d.Process).WithMany()
                .HasForeignKey(d => d.ProcessId)
                .HasConstraintName("FK__WorkingBy__proce__339FAB6E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
