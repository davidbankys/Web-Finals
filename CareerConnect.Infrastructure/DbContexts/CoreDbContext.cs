using CareerConnect.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace CareerConnect.Infrastructure.DbContexts
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Applicant> Applicants { get; set; }
        public virtual DbSet<Employer> Employers { get; set; }
        public virtual DbSet<Resume> Resumes { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }

    }
}