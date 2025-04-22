using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YallaR7la.Data.Models;

namespace YallaR7la.Data
{
    public class AppDbContext: IdentityDbContext<User>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) 
        {
            
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<BusinessOwner> BusinessOwners { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<AnalyticsReport> AnalyticsReports { get; set; }
        public DbSet<DestinationImages> DestinationImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnalyticsReport>()
                .HasOne(ar => ar.BusinessOwner)
                .WithMany(bo => bo.Analytics)
                .HasForeignKey(ar => ar.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull); //  Prevents delete conflicts

            modelBuilder.Entity<AnalyticsReport>()
                .HasOne(ar => ar.Admin)
                .WithMany(bo=>bo.Analytics)
                .HasForeignKey(ar => ar.GeneratedByAdminId)
                .OnDelete(DeleteBehavior.ClientSetNull); //  Prevents delete conflicts


            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        }

    }
}
