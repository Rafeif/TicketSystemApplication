using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketSystemApplication.Models;

namespace TicketSystemApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TicketStatus>().HasData(
                new TicketStatus() { Id = 1, Name = "New" },
                new TicketStatus() { Id = 2, Name = "Active" },
                new TicketStatus() { Id = 3, Name = "Redirect" },
                new TicketStatus() { Id = 4, Name = "Done" });

            //create administration role
            builder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = "13c0d132-cc9e-416d-944d-83e95e855f35",
                Name = "Admin",
                NormalizedName = "Admin".ToUpper()
            });
            //add password hasher for password encryption -hashing
            var hasher = new PasswordHasher<IdentityUser>();

            // create administation user account
            builder.Entity<IdentityUser>().HasData(
                new IdentityUser()
                {
                    Id= "13c0d132-cc9e-416d-944d-83e95e855f3y",
                    UserName="admin@sys.ps",
                    Email= "admin@sys.ps",
                    NormalizedUserName ="ADMIN@SYS.PS",
                    PasswordHash=hasher.HashPassword(null,"Pa$$word123")
                });

            // add admin user to administration role
            builder.Entity<IdentityUserRole<string>>().
                HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "13c0d132-cc9e-416d-944d-83e95e855f35",
                    UserId = "13c0d132-cc9e-416d-944d-83e95e855f3y"
                });
            base.OnModelCreating(builder);
        }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketLog> TicketLog { get; set; }
    }
}