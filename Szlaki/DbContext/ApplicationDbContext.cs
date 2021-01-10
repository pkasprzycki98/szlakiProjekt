using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Szlaki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Szlaki.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }          
        public DbSet<Trail> Trails { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Trail>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired();

            builder.Entity<Photo>()
                .HasOne(x => x.Trail)
                .WithMany(x => x.Photos)
                .HasForeignKey(x => x.TrailId)
                .IsRequired();
            builder.Entity<Video>()
                .HasOne(x => x.Trail)
                .WithMany(x => x.Videos)
                .HasForeignKey(x => x.TrailId);

            base.OnModelCreating(builder);
        }
    }
}
