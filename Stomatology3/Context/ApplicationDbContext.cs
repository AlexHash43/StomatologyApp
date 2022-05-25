using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stomatology3.Models;

namespace Stomatology3.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<AppointmentModel> Appointments { get; set; }
        public DbSet<ProcedureType> ProcedureTypes { get; set; }
        //on tableCreation makes table names singular(whithout s)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
