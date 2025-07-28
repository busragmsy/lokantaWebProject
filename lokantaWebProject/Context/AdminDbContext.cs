using lokantaWebProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lokantaWebProject.Context
{
    public class AdminDbContext: IdentityDbContext<Admin, IdentityRole<int>, int>
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
        { 
        }
        
        public DbSet<Message> Messages { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Category ve MenuItem arasındaki bir-çok ilişkiyi açıkça tanımla
            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // İlişkili MenuItem'lar varken kategori silinmesini engelle

            
        }
    }
}
