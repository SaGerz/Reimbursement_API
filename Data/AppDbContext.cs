using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reimbursement_API.Models;

namespace Reimbursement_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Reimburstment> Reimburstments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ApprovalHistory> ApprovalHistories { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Reimburstment>().ToTable("reimburstments");
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<ApprovalHistory>().ToTable("approvalhistories");
            modelBuilder.Entity<BankAccount>().ToTable("bankaccounts");
            modelBuilder.Entity<PaymentTransaction>().ToTable("paymenttransactions");
        }
    }
}