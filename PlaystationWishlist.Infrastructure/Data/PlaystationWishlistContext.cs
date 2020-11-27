using Microsoft.EntityFrameworkCore;
using PlaystationWishlist.Core.Interfaces;
using PlaystationWishlist.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlaystationWishlist.DataAccess.Data
{
    public class PlaystationWishlistContext : DbContext, IPlaystationWishlistDbContext
    {
        public DbSet<PlaystationGame> PlaystationGames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:playstationwishlist.database.windows.net,1433;Initial Catalog=PlaystationWishlistDataBase;Persist Security Info=False;User ID=JeffersonAmori;Password=M@sterk3y;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
