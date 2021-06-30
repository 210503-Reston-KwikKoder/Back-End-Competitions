using CBEModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBEDL
{
    public class CBEDbContext : DbContext
    {
        public CBEDbContext(DbContextOptions options) : base(options) { }
        protected CBEDbContext() { }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<CompetitionStat> CompetitionStats { get; set; }
        public DbSet<LiveCompetition> LiveCompetitions { get; set; }
        public DbSet<LiveCompetitionTest> LiveCompetitionTests { get; set; }
        public DbSet<UserQueue> UserQueues { get; set; }
        public DbSet<LiveCompStat> LiveCompStats { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(user => user.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .HasIndex(user => user.Auth0Id)
                .IsUnique();
            modelBuilder.Entity<Category>()
                .Property(cat => cat.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>()
                .HasIndex(cat => cat.Name)
                .IsUnique();
            modelBuilder.Entity<Competition>()
                .Property(comp => comp.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<CompetitionStat>()
                .HasKey(cS => new { cS.UserId, cS.CompetitionId });
            modelBuilder.Entity<LiveCompetition>()
                .Property(lC => lC.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<LiveCompetitionTest>()
                .Property(lC => lC.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<UserQueue>()
                .HasKey(uQ => new { uQ.UserId, uQ.LiveCompetitionId });
            modelBuilder.Entity<LiveCompStat>()
                .HasKey(lCS => new { lCS.UserId, lCS.LiveCompetitionId });
            

        }
    }
}
