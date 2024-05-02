using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BattleShipAPI.Models;

    public class BattleshipContext : DbContext
    {
        public BattleshipContext (DbContextOptions<BattleshipContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=localhost;Database=BattleshipDB;User Id=SA;Password='m=3q>0Th%fJ7k;9%7ntf7@AQT';TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<Player> Player { get; set; } = default!;
        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<GameInstance> GameInstances { get; set; } = default!;
        public DbSet<Board> Boards { get; set; } = default!;
    }
