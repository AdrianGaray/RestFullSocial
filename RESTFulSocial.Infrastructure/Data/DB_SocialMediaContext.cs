using Microsoft.EntityFrameworkCore;
using RESTFulSocial.Core.Entities;
using RESTFulSocial.Infrastructure.Data.Configurations;

namespace RESTFulSocial.Infrastructure.Data
{
    public partial class DB_SocialMediaContext : DbContext
    {
        public DB_SocialMediaContext()
        {
        }

        public DB_SocialMediaContext(DbContextOptions<DB_SocialMediaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CommentConfiguration());

            modelBuilder.ApplyConfiguration(new PostConfiguration());

            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
