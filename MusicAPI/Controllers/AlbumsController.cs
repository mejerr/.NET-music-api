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
    public class AlbumsController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public AlbumsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("{id}/avatar")]
        public async Task<IActionResult> PostImage(int id, [FromForm] Album albumObj)
        {
            var album = await _dbContext.Artists.FindAsync(id);
            if (album == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                album.ImageUrl = albumObj.Image.FileName;
                await _dbContext.SaveChangesAsync();
                return Ok("Avatar uploaded successfully");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAlbums()
        {
            var albums = await (from album in _dbContext.Albums
                select new
                {
                    Id = album.Id,
                    Name = album.Name,
                    ArtistId = album.ArtistId
                }).ToListAsync();
            return Ok(albums);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlbum([FromBody] Album album)
        {
            await _dbContext.Albums.AddAsync(album);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> AlbumDetails(int albumId)
        {
            var albumDetails = await _dbContext.Albums.Where(a => a.Id == albumId).Include(a => a.Songs).ToListAsync();
            return Ok(albumDetails);
        }
    }
}
