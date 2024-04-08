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

        public virtual DbSet<AgeRange> AgeRanges { get; set; } = null!;
        public virtual DbSet<Beneficiary> Beneficiaries { get; set; } = null!;
        public virtual DbSet<Benefit> Benefits { get; set; } = null!;
        public virtual DbSet<BenefitDetail> BenefitDetails { get; set; } = null!;
        public virtual DbSet<InsurancePackage> InsurancePackages { get; set; } = null!;
        public virtual DbSet<InsurancePackageDetail> InsurancePackageDetails { get; set; } = null!;
        public virtual DbSet<InsurancePackageGroup> InsurancePackageGroups { get; set; } = null!;
        public virtual DbSet<InsurancePackageHeader> InsurancePackageHeaders { get; set; } = null!;
        public virtual DbSet<InsuranceProvider> InsuranceProviders { get; set; } = null!;
        public virtual DbSet<InsuranceTransaction> InsuranceTransactions { get; set; } = null!;
        public virtual DbSet<InsuranceType> InsuranceTypes { get; set; } = null!;
        public virtual DbSet<Laccount> Laccounts { get; set; } = null!;
        public virtual DbSet<Lmember> Lmembers { get; set; } = null!;
        public virtual DbSet<LoanStatement> LoanStatements { get; set; } = null!;
        public virtual DbSet<LoanTransaction> LoanTransactions { get; set; } = null!;
        public virtual DbSet<LoanTransactionDetail> LoanTransactionDetails { get; set; } = null!;
        public virtual DbSet<LtransactionLog> LtransactionLogs { get; set; } = null!;
        public virtual DbSet<SavingStatement> SavingStatements { get; set; } = null!;
        public virtual DbSet<SavingTransaction> SavingTransactions { get; set; } = null!;
        public virtual DbSet<SavingTransactionDetail> SavingTransactionDetails { get; set; } = null!;
        public virtual DbSet<TaccessRule> TaccessRules { get; set; } = null!;
        public virtual DbSet<TactivityLog> TactivityLogs { get; set; } = null!;
        public virtual DbSet<Taudit> Taudits { get; set; } = null!;
        public virtual DbSet<Term> Terms { get; set; } = null!;
        public virtual DbSet<Tkiosk> Tkiosks { get; set; } = null!;
        public virtual DbSet<TnotificationLog> TnotificationLogs { get; set; } = null!;
        public virtual DbSet<TslideDetail> TslideDetails { get; set; } = null!;
        public virtual DbSet<TslideHeader> TslideHeaders { get; set; } = null!;
        public virtual DbSet<Tstation> Tstations { get; set; } = null!;
        public virtual DbSet<Tuser> Tusers { get; set; } = null!;
        public virtual DbSet<TuserGroup> TuserGroups { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("ConnectionStrings:DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgeRange>(entity =>
            {
                entity.ToTable("AgeRange");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.EndAge).HasColumnName("endAge");

                entity.Property(e => e.StartAge).HasColumnName("startAge");
            });

            modelBuilder.Entity<Beneficiary>(entity =>
            {
                entity.ToTable("Beneficiary");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BeneficiaryId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("beneficiaryId");

                entity.Property(e => e.BeneficiaryName)
                    .HasMaxLength(50)
                    .HasColumnName("beneficiaryName");

                entity.Property(e => e.MemberId).HasColumnName("memberId");

                entity.Property(e => e.Birthday).HasColumnName("birthday");

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .HasColumnName("gender");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");

                entity.Property(e => e.Occupation)
                    .HasMaxLength(255)
                    .HasColumnName("occupation");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .HasColumnName("phone");

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(255)
                    .HasColumnName("taxCode");

                entity.Property(e => e.Relationship)
                    .HasMaxLength(20)
                    .HasColumnName("relationship");

                entity.Property(e => e.TransactionId).HasColumnName("transactionId");
            });


            modelBuilder.Entity<Benefit>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Benefit");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.Coverage).HasColumnName("coverage");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.PackageId).HasColumnName("packageId");
            });

            modelBuilder.Entity<BenefitDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BenefitDetail");

                entity.Property(e => e.BenefitId).HasColumnName("benefitId");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.Coverage).HasColumnName("coverage");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");
            });

            modelBuilder.Entity<InsurancePackage>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("InsurancePackage");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.Fee).HasColumnName("fee");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.InsuranceType).HasColumnName("insuranceType");

                entity.Property(e => e.PackageName)
                    .HasMaxLength(50)
                    .HasColumnName("packageName");

                entity.Property(e => e.PayType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("payType");

                entity.Property(e => e.Provider)
                    .HasMaxLength(50)
                    .HasColumnName("provider");
            });

            modelBuilder.Entity<InsurancePackageDetail>(entity =>
            {
                entity.ToTable("InsurancePackageDetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AgeRangeId).HasColumnName("ageRangeId");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.Fee).HasColumnName("fee");

                entity.Property(e => e.PackageHeaderId).HasColumnName("packageHeaderId");
            });

            modelBuilder.Entity<InsurancePackageGroup>(entity =>
            {
                entity.ToTable("InsurancePackageGroup");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<InsurancePackageHeader>(entity =>
            {
                entity.ToTable("InsurancePackageHeader");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.InsuranceProviderId).HasColumnName("insuranceProviderId");

                entity.Property(e => e.InsuranceTypeId).HasColumnName("insuranceTypeId");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.PackageName).HasColumnName("packageName");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.TermId).HasColumnName("termId");
            });

            modelBuilder.Entity<InsuranceProvider>(entity =>
            {
                entity.ToTable("InsuranceProvider");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.Provider).HasColumnName("provider");
            });

            modelBuilder.Entity<InsuranceTransaction>(entity =>
            {
                entity.ToTable("InsuranceTransaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AnnualPay).HasColumnName("annualPay");

                entity.Property(e => e.ContractId).HasColumnName("contractId");

                entity.Property(e => e.ExpireDate)
                    .HasColumnType("datetime")
                    .HasColumnName("expireDate");

                entity.Property(e => e.MemberId).HasColumnName("memberId");

                entity.Property(e => e.PackageDetailId).HasColumnName("packageDetailId");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("registrationDate");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TransactionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("transactionDate");
            });

            modelBuilder.Entity<InsuranceType>(entity =>
            {
                entity.ToTable("InsuranceType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(50)
                    .HasColumnName("typeName");
            });

            modelBuilder.Entity<Laccount>(entity =>
            {
                entity.ToTable("LAccount");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("accountName");

                entity.Property(e => e.AccountType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("accountType");

                entity.Property(e => e.Balance).HasColumnName("balance");

                entity.Property(e => e.ContractId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("contractId");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateDue)
                    .HasColumnType("date")
                    .HasColumnName("dateDue");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.MemberId).HasColumnName("memberId");

                entity.Property(e => e.Rate).HasColumnName("rate");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Lmember>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LMember");

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

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.Fingerprint1).HasColumnName("fingerprint1");

                entity.Property(e => e.Fingerprint2).HasColumnName("fingerprint2");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(30)
                    .HasColumnName("firstName");

                entity.Property(e => e.FullName)
                    .HasMaxLength(30)
                    .HasColumnName("fullName");

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .HasColumnName("gender");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.IdenNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("idenNumber");

                entity.Property(e => e.ImageIdCard).HasColumnName("imageIdCard");

                entity.Property(e => e.ImageMember).HasColumnName("imageMember");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.LastName)
                    .HasMaxLength(30)
                    .HasColumnName("lastName");

                entity.Property(e => e.Occupation).HasColumnName("occupation");

                entity.Property(e => e.Phone)
                    .HasMaxLength(30)
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

            modelBuilder.Entity<LoanStatement>(entity =>
            {
                entity.ToTable("LoanStatement");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.AccountId)
                    .HasColumnName("accountId");

                entity.Property(e => e.Debt)
                    .HasColumnName("debt");

                entity.Property(e => e.Paid)
                    .HasColumnName("paid");

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transactionDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("description");

                entity.Property(e => e.Status)
                    .HasColumnName("status");
            });


            modelBuilder.Entity<LoanTransaction>(entity =>
            {
                entity.ToTable("LoanTransaction");

                
                

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberId");

                

                entity.Property(e => e.LoanTerm)
                    .HasColumnName("LoanTerm");

                entity.Property(e => e.Debt)
                    .HasColumnName("Debt");

                

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("TransactionDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.DueDate)
                    .HasColumnName("DueDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("Status");
            });



            modelBuilder.Entity<LoanTransactionDetail>(entity =>
            {
                entity.ToTable("LoanTransactionDetail");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.LoanHeaderId)
                    .HasColumnName("loanHeaderId");

                entity.Property(e => e.PaidAmount)
                    .HasColumnName("paidAmount");

                entity.Property(e => e.DebtRemaining)
                    .HasColumnName("debtRemaining");

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("transactionDate");
            });


            modelBuilder.Entity<LtransactionLog>(entity =>
            {
                entity.ToTable("LTransactionLog");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Content)
                    .IsUnicode(false)
                    .HasColumnName("content");

                entity.Property(e => e.Credit).HasColumnName("credit");

                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("currency");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.Debit).HasColumnName("debit");

                entity.Property(e => e.DueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("dueDate");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.KioskId).HasColumnName("kioskId");

                entity.Property(e => e.KioskRemainingMoney).HasColumnName("kioskRemainingMoney");

                entity.Property(e => e.MemberId).HasColumnName("memberId");

                entity.Property(e => e.MonthlyPaymentAmount).HasColumnName("monthlyPaymentAmount");

                entity.Property(e => e.Payback).HasColumnName("payback");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("paymentMethod");

                entity.Property(e => e.Period)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("period");

                entity.Property(e => e.Rate).HasColumnName("rate");

                entity.Property(e => e.StationId).HasColumnName("stationId");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TaxAmount).HasColumnName("taxAmount");

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("taxCode");

                entity.Property(e => e.TaxName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("taxName");

                entity.Property(e => e.TaxType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("taxType");

                entity.Property(e => e.TransactionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("transactionDate");

                entity.Property(e => e.TransactionId).HasColumnName("transactionId");

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("transactionType");
            });

            modelBuilder.Entity<SavingStatement>(entity =>
            {
                entity.ToTable("SavingStatement");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.AnnualRate).HasColumnName("annualRate");

                entity.Property(e => e.Balance).HasColumnName("balance");

                entity.Property(e => e.DateSaving)
                    .HasColumnType("datetime")
                    .HasColumnName("dateSaving");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.MemberId).HasColumnName("memberId");

                entity.Property(e => e.Period).HasColumnName("period");

                entity.Property(e => e.SavingId).HasColumnName("savingId");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<SavingTransaction>(entity =>
            {
                entity.ToTable("SavingTransaction");

                

                

                entity.Property(e => e.MemberId)
                    .HasColumnName("MemberId");

                

                entity.Property(e => e.TopUp)
                    .HasColumnName("TopUp");

                

                entity.Property(e => e.SavingRate)
                    .HasColumnName("SavingRate");

                entity.Property(e => e.SavingTerm)
                    .HasColumnName("SavingTerm");

                entity.Property(e => e.TransactionDate)
                    .HasColumnName("TransactionDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.DueDate)
                    .HasColumnName("DueDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("Status");
            });


            modelBuilder.Entity<SavingTransactionDetail>(entity =>
            {
                entity.ToTable("SavingTransactionDetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.AnnualRate).HasColumnName("annualRate");

                entity.Property(e => e.Balance).HasColumnName("balance");

                entity.Property(e => e.ContractId).HasColumnName("contractId");

                entity.Property(e => e.DueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("dueDate");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.MemberId).HasColumnName("memberId");

                entity.Property(e => e.SavingId).HasColumnName("savingId");

                entity.Property(e => e.StartDateOfSaving)
                    .HasColumnType("datetime")
                    .HasColumnName("startDateOfSaving");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TopUp).HasColumnName("topUp");

                entity.Property(e => e.TransactionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("transactionDate");

                entity.Property(e => e.Withdraw).HasColumnName("withdraw");
            });

            modelBuilder.Entity<TaccessRule>(entity =>
            {
                entity.ToTable("TAccessRule");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CanAdd).HasColumnName("canAdd");

                entity.Property(e => e.CanDelete).HasColumnName("canDelete");

                entity.Property(e => e.CanUpdate).HasColumnName("canUpdate");

                entity.Property(e => e.CanView).HasColumnName("canView");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");

                entity.Property(e => e.FeatureName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("featureName");

                entity.Property(e => e.GroupId).HasColumnName("groupId");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Site).HasColumnName("site");
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

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("status");
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
                    .HasMaxLength(50)
                    .HasColumnName("tableName");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<Term>(entity =>
            {
                entity.ToTable("Term");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasColumnName("dateModified");
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

                entity.Property(e => e.Component)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("component");

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

                entity.Property(e => e.KioskRemainingMoney).HasColumnName("kioskRemainingMoney");

                entity.Property(e => e.KioskStatus).HasColumnName("kioskStatus");

                entity.Property(e => e.Location)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("location");

                entity.Property(e => e.MacAddress)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("macAddress");

                entity.Property(e => e.OfflineTime)
                    .HasColumnType("datetime")
                    .HasColumnName("offlineTime");

                entity.Property(e => e.OnlineTime)
                    .HasColumnType("datetime")
                    .HasColumnName("onlineTime");

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

            modelBuilder.Entity<TslideDetail>(entity =>
            {
                entity.ToTable("TSlideDetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ContentUrl)
                    .HasMaxLength(100)
                    .HasColumnName("contentUrl");

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

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Sequence).HasColumnName("sequence");

                entity.Property(e => e.SlideHeaderId).HasColumnName("slideHeaderId");

                entity.Property(e => e.TypeContent)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("typeContent");
            });

            modelBuilder.Entity<TslideHeader>(entity =>
            {
                entity.ToTable("TSlideHeader");

                entity.Property(e => e.Id).HasColumnName("id");

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

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("startDate");

                entity.Property(e => e.TimeNext).HasColumnName("timeNext");
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
