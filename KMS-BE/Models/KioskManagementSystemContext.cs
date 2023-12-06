using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KMS.Models
{
    public partial class KioskManagementSystemContext : DbContext
    {
        public KioskManagementSystemContext()
        {
        }

        public KioskManagementSystemContext(DbContextOptions<KioskManagementSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Lmember> Lmembers { get; set; } = null!;
        public virtual DbSet<TaccessRule> TaccessRules { get; set; } = null!;
        public virtual DbSet<TactivityLog> TactivityLogs { get; set; } = null!;
        public virtual DbSet<Taudit> Taudits { get; set; } = null!;
        public virtual DbSet<Tkiosk> Tkiosks { get; set; } = null!;
        public virtual DbSet<TnotificationLog> TnotificationLogs { get; set; } = null!;
        public virtual DbSet<Tslideshow> Tslideshows { get; set; } = null!;
        public virtual DbSet<Tstation> Tstations { get; set; } = null!;
        public virtual DbSet<Tuser> Tusers { get; set; } = null!;
        public virtual DbSet<TuserGroup> TuserGroups { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=kmsdatabase.database.windows.net;Database=KioskManagementSystem;User Id=kmsadmin;Password=pa55w0rd!@#;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lmember>(entity =>
            {
                entity.ToTable("LMember");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address1)
                    .HasMaxLength(100)
                    .HasColumnName("address1");

                entity.Property(e => e.Address2)
                    .HasMaxLength(100)
                    .HasColumnName("address2");

                entity.Property(e => e.BankName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bankName");

                entity.Property(e => e.BankNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("bankNumber");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasColumnName("birthday");

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("city");

                entity.Property(e => e.CompanyAddress)
                    .HasMaxLength(100)
                    .HasColumnName("companyAddress");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .HasColumnName("companyName");

                entity.Property(e => e.CreditLimit).HasColumnName("creditLimit");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.DebtStatus)
                    .HasMaxLength(50)
                    .HasColumnName("debtStatus");

                entity.Property(e => e.Department)
                    .HasMaxLength(30)
                    .HasColumnName("department");

                entity.Property(e => e.District)
                    .HasMaxLength(30)
                    .HasColumnName("district");

                entity.Property(e => e.Fingerprint1)
                    .HasColumnType("image")
                    .HasColumnName("fingerprint1");

                entity.Property(e => e.Fingerprint2)
                    .HasColumnType("image")
                    .HasColumnName("fingerprint2");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(30)
                    .HasColumnName("firstName");

                entity.Property(e => e.FullName)
                    .HasMaxLength(30)
                    .HasColumnName("fullName");

                entity.Property(e => e.IdenNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("idenNumber");

                entity.Property(e => e.Image)
                    .HasColumnType("image")
                    .HasColumnName("image");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.LastName)
                    .HasMaxLength(30)
                    .HasColumnName("lastName");

                entity.Property(e => e.Phone)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.RefCode).HasColumnName("refCode");

                entity.Property(e => e.SalaryAmount).HasColumnName("salaryAmount");

                entity.Property(e => e.SettleDate)
                    .HasColumnType("date")
                    .HasColumnName("settleDate");

                entity.Property(e => e.StatementDate)
                    .HasColumnType("date")
                    .HasColumnName("statementDate");

                entity.Property(e => e.Ward)
                    .HasMaxLength(30)
                    .HasColumnName("ward");
            });

            modelBuilder.Entity<TaccessRule>(entity =>
            {
                entity.ToTable("TAccessRule");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.FeatureName).HasColumnName("featureName");

                entity.Property(e => e.GroupId).HasColumnName("groupId");

                entity.Property(e => e.IsActive).HasColumnName("isActive");
            });

            modelBuilder.Entity<TactivityLog>(entity =>
            {
                entity.ToTable("TActivityLog");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.HardwareName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("hardwareName");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.KioskId).HasColumnName("kioskId");

                entity.Property(e => e.StationId).HasColumnName("stationId");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Taudit>(entity =>
            {
                entity.ToTable("TAudit");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("action");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.Field)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("field");

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ipAddress");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.KioskId).HasColumnName("kioskId");

                entity.Property(e => e.MacAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("macAddress");

                entity.Property(e => e.Script)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("script");

                entity.Property(e => e.TableName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("tableName");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<Tkiosk>(entity =>
            {
                entity.ToTable("TKiosk");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AutoUpdate).HasColumnName("autoUpdate");

                entity.Property(e => e.AvailableMemory).HasColumnName("availableMemory");

                entity.Property(e => e.CameraPort)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cameraPort");

                entity.Property(e => e.CameraStatus).HasColumnName("cameraStatus");

                entity.Property(e => e.CashDepositPort)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cashDepositPort");

                entity.Property(e => e.CashDepositStatus).HasColumnName("cashDepositStatus");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.DiskSizeC).HasColumnName("diskSizeC");

                entity.Property(e => e.DiskSizeD).HasColumnName("diskSizeD");

                entity.Property(e => e.FreeSpaceC).HasColumnName("freeSpaceC");

                entity.Property(e => e.FreeSpaceD).HasColumnName("freeSpaceD");

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ipAddress");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.KioskName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("kioskName");

                entity.Property(e => e.KioskStatus).HasColumnName("kioskStatus");

                entity.Property(e => e.Location)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("location");

                entity.Property(e => e.MacAddress)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("macAddress");

                entity.Property(e => e.Osname)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OSName");

                entity.Property(e => e.Osplatform)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("OSPlatform");

                entity.Property(e => e.Osversion)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OSVersion");

                entity.Property(e => e.PrinterPort)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("printerPort");

                entity.Property(e => e.PrinterStatus).HasColumnName("printerStatus");

                entity.Property(e => e.Processor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("processor");

                entity.Property(e => e.RefCode)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("refCode");

                entity.Property(e => e.ScannerPort)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("scannerPort");

                entity.Property(e => e.ScannerStatus).HasColumnName("scannerStatus");

                entity.Property(e => e.SlidePackage).HasColumnName("slidePackage");

                entity.Property(e => e.StationCode).HasColumnName("stationCode");

                entity.Property(e => e.TotalMemory).HasColumnName("totalMemory");

                entity.Property(e => e.UpTime)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("upTime");

                entity.Property(e => e.VersionApp)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("versionApp");

                entity.Property(e => e.WebServices)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("webServices");
            });

            modelBuilder.Entity<TnotificationLog>(entity =>
            {
                entity.ToTable("TNotificationLog");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.MemberId).HasColumnName("memberId");

                entity.Property(e => e.SendType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("sendType");

                entity.Property(e => e.SocketKey)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("socketKey");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Tslideshow>(entity =>
            {
                entity.ToTable("TSlideshow");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateByUserId).HasColumnName("createByUserID");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("endDate");

                entity.Property(e => e.FileType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("fileType");

                entity.Property(e => e.Imagevideo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("imagevideo");

                entity.Property(e => e.PackageName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("packageName");

                entity.Property(e => e.Scrolltext1)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("scrolltext1");

                entity.Property(e => e.Scrolltext2)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("scrolltext2");

                entity.Property(e => e.Sequence).HasColumnName("sequence");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("startDate");

                entity.Property(e => e.StationId).HasColumnName("stationID");

                entity.Property(e => e.Timer).HasColumnName("timer");

                entity.Property(e => e.UpdatedByUserId).HasColumnName("updatedByUserID");
            });

            modelBuilder.Entity<Tstation>(entity =>
            {
                entity.ToTable("TStation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("companyName");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.StationName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("stationName");
            });

            modelBuilder.Entity<Tuser>(entity =>
            {
                entity.ToTable("TUser");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CardNum)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cardNum");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(30)
                    .HasColumnName("fullname");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("lastLogin");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RefCode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("refCode");

                entity.Property(e => e.UserGroupId).HasColumnName("userGroupId");

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<TuserGroup>(entity =>
            {
                entity.ToTable("TUserGroup");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccessRuleId).HasColumnName("accessRuleId");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("groupName");

                entity.Property(e => e.IsActive).HasColumnName("isActive");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
