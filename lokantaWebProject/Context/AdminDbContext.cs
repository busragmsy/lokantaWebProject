using lokantaWebProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace lokantaWebProject.Context
{
    public class AdminDbContext: DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
        { }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
