using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PixelIt.Models;
using Microsoft.EntityFrameworkCore;

namespace PixelIt.Data
{
    public class ApplicationDbContext : IdentityDbContext<
            ApplicationUser,
            ApplicationRole,
            string,
            IdentityUserClaim<string>,
            ApplicationUserRole,
            IdentityUserLogin<string>,
            IdentityRoleClaim<string>,
            IdentityUserToken<string>
        >
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ImageCollection> ImageCollections { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           base.OnModelCreating(builder);
            builder
                 .Entity<ApplicationUserRole>()
                 .HasOne(ur => ur.User)
                 .WithMany(ur => ur.UserRoles)
                 .HasForeignKey(ur => ur.UserId)
                 .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<ApplicationUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<ApplicationUser>()
                .HasMany(ic => ic.ImageCollections)
                .WithOne(u => u.User)
                .HasForeignKey(ic => ic.IdUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<ApplicationUser>()
                .HasMany(p => p.Posts)
                .WithOne(u => u.User)
                .HasForeignKey(p => p.IdUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<ApplicationUser>()
                .HasMany(c => c.Comments)
                .WithOne(u => u.User)
                .HasForeignKey(c => c.IdUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<ApplicationUser>()
                .HasMany(l => l.Likes)
                .WithOne(u => u.User)
                .HasForeignKey(l => l.IdUser)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<ApplicationUser>()
                .HasMany(f => f.Followers)
                .WithOne(u => u.Followed)
                .HasForeignKey(f => f.IdFollowed)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<ApplicationUser>()
                .HasMany(f => f.Followings)
                .WithOne(u => u.Follower)
                .HasForeignKey(f => f.IdFollower)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<PostCategory>()
                .HasOne(p => p.Post)
                .WithMany(pc => pc.PostCategories)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<Post>()
                .HasMany(c => c.Comments)
                .WithOne(p => p.Post)
                .HasForeignKey(c => c.IdPost)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<Post>()
                .HasMany(l => l.Likes)
                .WithOne(p => p.Post)
                .HasForeignKey(l => l.IdPost)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<PostCategory>()
                .HasOne(c => c.Category)
                .WithMany(pc => pc.PostCategories)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
