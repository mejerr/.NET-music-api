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
    public class ArtistsController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public ArtistsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("{id}/avatar")]
        public async Task<IActionResult> PostImage(int id, [FromForm] Artist artistObj)
        {
            var artist = await _dbContext.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                artist.ImageUrl = artistObj.Image.FileName;
                await _dbContext.SaveChangesAsync();
                return Ok("Avatar uploaded successfully");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetArtists()
        {
            var artists = await (from artist in _dbContext.Artists
            select new
            {
                Id = artist.Id,
                Name = artist.Name,
                ImageUrl = artist.ImageUrl
            }).ToListAsync();
            return Ok(artists);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtist([FromBody] Artist artist)
        {
            await _dbContext.Artists.AddAsync(artist);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ArtistDetails(int artistId)
        {
            var artistDetails = await _dbContext.Albums.Where(a => a.Id == artistId).Include(a => a.Songs).ToListAsync();
            return Ok(artistDetails);
        }
    }
}
