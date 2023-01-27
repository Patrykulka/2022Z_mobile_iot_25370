using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtistHaven.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtistHaven.API.Data
{
    public class ArtistHavenAPIContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ArtistHavenAPIContext (DbContextOptions<ArtistHavenAPIContext> options) : base(options) { }

        public DbSet<User> User { get; set; } = default!;

        public DbSet<SubscriptionTier> SubscriptionTier { get; set; }

        public DbSet<Subscription> Subscription { get; set; }

        public DbSet<Post> Post { get; set; }
        
        public DbSet<Media> Media { get; set; }

        public DbSet<PostMedia> PostMedia { get; set; }
        
        public DbSet<PostTag> PostTag { get; set; }

        public DbSet<Tag> Tag { get; set; }
    }
}
