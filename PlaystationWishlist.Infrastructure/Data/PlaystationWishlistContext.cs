using Microsoft.EntityFrameworkCore;
using PlaystationWishlist.Core.Interfaces;
using PlaystationWishlist.DataAccess.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PlaystationWishlist.DataAccess.Data
{
    public sealed class PlaystationWishlistContext : DbContext, IPlaystationWishlistDbContext
    {
        public PlaystationWishlistContext() { }
        public PlaystationWishlistContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<PlaystationGame> PlaystationGames { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetCreatedAndModified();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetCreatedAndModified()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is PlaystationGame && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((PlaystationGame)entity.Entity).LastUpdated = DateTime.UtcNow;
                }

                ((PlaystationGame)entity.Entity).LastUpdated = DateTime.UtcNow;
            }
        }
    }
}
