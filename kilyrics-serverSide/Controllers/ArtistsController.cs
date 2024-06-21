using kilyrics_serverSide.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace kilyrics_serverSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        // GET: api/<ArtistsController>
        [HttpGet]
        [Route("GetAllArtists")]
        public List<string> GetAllArtists()
        {
            return Artist.GetAllArtists();
        }

        [HttpGet("inFavoriteCount")]
        public int GetFavoriteCount(string artistName)
        {
            return Artist.getFavoriteCount(artistName);
        }
    }
}
