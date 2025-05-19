using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YallaR7la.Data.Models;
using YallaR7la.Data.Models.YallaR7la.Data.Models;

namespace YallaR7la.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<BusinessOwner> BusinessOwners { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<AnalyticsReport> AnalyticsReports { get; set; }
        public DbSet<DestinationImages> DestinationImages { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Chat> Chat { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  Destination with BusinessOwner - Cascade Delete
            modelBuilder.Entity<Destination>()
                .HasOne(d => d.BusinessOwner)
                .WithMany(bo => bo.Destinations)
                .HasForeignKey(d => d.BusinessOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // AnalyticsReport -> BusinessOwner
            modelBuilder.Entity<AnalyticsReport>()
                .HasOne(ar => ar.BusinessOwner)
                .WithMany(bo => bo.Analytics)
                .HasForeignKey(ar => ar.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // AnalyticsReport -> Admin
            modelBuilder.Entity<AnalyticsReport>()
                .HasOne(ar => ar.Admin)
                .WithMany(a => a.Analytics)
                .HasForeignKey(ar => ar.GeneratedByAdminId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Identity composite keys
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            // Message --> Chat
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            // Message --> User
            modelBuilder.Entity<Message>()
               .HasOne(m => m.Admin)
               .WithMany()
               .HasForeignKey(m => m.AdminId)
               .OnDelete(DeleteBehavior.Restrict); // Or .ClientSetNull

            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Owner)
                .WithMany()
                .HasForeignKey(m => m.BusinessOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Chat --> User
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Chat --> Owner
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Chat --> Admin
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Admin)
                .WithMany()
                .HasForeignKey(c => c.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(c => c.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
