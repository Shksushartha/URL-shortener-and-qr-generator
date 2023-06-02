using System;
using Microsoft.EntityFrameworkCore;
using urlShortener.Models;

namespace urlShortener.Data
{
    public class UrlDbContext : DbContext
    {

        public DbSet<Url> urls { get; set; }


        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Url>().HasIndex(x => x.urlIdentifier).IsUnique();
        }
    }


}

