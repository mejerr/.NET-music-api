using Microsoft.EntityFrameworkCore;
using MusicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAPI.Data
{
    public class ApiDbContext: DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext>options) : base(options)
        {

        }
        public DbSet<Song> Songs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Song>().HasData(
                new Song
                {
                    Id = 1,
                    Title = "Willow",
                    Language = "Spanish",
                    Duration = "4.35"
                },
                 new Song
                 {
                     Id = 2,
                     Title = "Despacito",
                     Language = "Spanish",
                     Duration = "3.35"
                 }
                );
        }
    }
}
