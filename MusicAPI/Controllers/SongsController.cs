using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("{id}/avatar")]
        public async Task<IActionResult> PostImage(int id, [FromForm] Song songObj)
        {
            var song = await _dbContext.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                song.ImageUrl = songObj.Image.FileName;
                await _dbContext.SaveChangesAsync();
                return Ok("Avatar uploaded successfully");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetSongs(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentpageSize = pageSize ?? 5;

            var songs = await (from song in _dbContext.Songs
                select new
                {
                    Id = song.Id,
                    Title = song.Title,
                    ArtistId = song.ArtistId,
                    AlbumId = song.AlbumId,
                    Duration = song.Duration,
                    IsFeatured = song.IsFeatured
                }).ToListAsync();
            return Ok(songs.Skip((currentPageNumber - 1) * currentpageSize).Take(currentpageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            var songs = await (from song in _dbContext.Songs
                               where song.IsFeatured == true
                               // orderby song.UploadedDate descending
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   ArtistId = song.ArtistId,
                                   AlbumId = song.AlbumId,
                                   Duration = song.Duration,
                                   IsFeatured = song.IsFeatured
                               }).ToListAsync();
            return Ok(songs);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSongs(string query)
        {
            var songs = await (from song in _dbContext.Songs
                               where song.Title.StartsWith(query)
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   ArtistId = song.ArtistId,
                                   AlbumId = song.AlbumId,
                                   Duration = song.Duration,
                                   IsFeatured = song.IsFeatured
                               }).ToListAsync();
            return Ok(songs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSong([FromBody] Song song)
        {
            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
