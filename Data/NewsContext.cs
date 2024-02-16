using Microsoft.EntityFrameworkCore;

namespace News.Data
{
    public partial class NewsContext : DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options) : base(options)
        {
            
        }

        #region DbSet
        public virtual DbSet<Role> Roles {get; set;}
        public virtual DbSet<User> Users {get; set;}
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(r => r.Role)
                .HasForeignKey(r => r.RoleId)
                .IsRequired();


            base.OnModelCreating(modelBuilder);
        }
    }
}