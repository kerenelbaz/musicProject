using kilyrics_serverSide.Models;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace kilyrics_serverSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        // GET: api/<SongController>
        [HttpGet]
        public IEnumerable<Song> Get()
        {
            return Song.GetAllSongs();
        }

        // GET api/<SongController>
        [HttpGet("byArtistName")]
        public IEnumerable<Song> GetByArtist(string artistName)
        {
            return Song.ReadSongsByArtist(artistName);
        }


        [HttpGet("bySongName")]
        public IEnumerable<Song> GetByName(string songName)
        {
            return Song.ReadSongsByName(songName);
        }

        [HttpGet("bySongLyrics")]
        public IEnumerable<Song> GetByLyrics(string lyrics)
        {
            return Song.ReadSongsByLyrics(lyrics);
        }

        
    }
}

