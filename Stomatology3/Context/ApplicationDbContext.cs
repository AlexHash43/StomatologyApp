using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stomatology3.Models;

namespace Stomatology3.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public override DbSet<User> Users { get; set; }
        public DbSet<AppointmentModel> Appointments { get; set; }
        public DbSet<ProcedureType> ProcedureTypes { get; set; }
        //on tableCreation makes table names singular(whithout s)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity<User>().Property(b => b.Email).IsRequired();
                //modelBuilder.Entity<User>().Property(b => b.NormalizedEmail).IsRequired();
                modelBuilder.Entity<User>().Property(b => b.UserName).IsRequired();
                //modelBuilder.Entity<User>().Property(b => b.NormalizedUserName).IsRequired();
                modelBuilder.Entity<User>().Property(b => b.PasswordHash).IsRequired();
                modelBuilder.Entity<User>().Property(b => b.CreatedOn).IsRequired();
                //modelBuilder.Entity<User>().Property(b => b.EmailConfirmed).IsRequired(false);
                //modelBuilder.Entity<User>().Property(b => b.PhoneNumberConfirmed).IsRequired(false);
                //modelBuilder.Entity<User>().Property(b => b.TwoFactorEnabled).IsRequired(false);
                //modelBuilder.Entity<User>().Property(b => b.LockoutEnabled).IsRequired(false);
                //modelBuilder.Entity<User>().Property(b => b.AccessFailedCount).IsRequired(false);

            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
