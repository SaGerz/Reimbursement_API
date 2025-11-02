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
    }
}