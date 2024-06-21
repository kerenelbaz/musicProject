using kilyrics_serverSide.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace kilyrics_serverSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {

            return Models.User.getAllUsers();
        }

        [HttpPost("registration")]
        public bool PostRegistration([FromBody] User u)
        {
            return u.Registration();
        }

        [HttpPost("login")]
        public User PostLogin(string email, [FromBody] string password)
        {
            try
            {
                return Models.User.logIn(email, password);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        [HttpPost("insertSongToFavorites")]

        public bool insertSongToFavorites(int songId, int userId)
        {
            return Models.User.InsertSongToFavorites(songId, userId);

        }

        [HttpGet("GetFavoriteSongsOfUser")]
        public IEnumerable<Song> GetFavoriteSongsOfUser(int userId)
        {
            return Models.User.GetFavoriteSongsOfUser(userId);
        }


        // DELETE api/<UsersController>/5
        [HttpDelete("deleteSongFromFavorites")]
        public List<Song> Delet(int songId, int userId)
        {
            return Models.User.deleteSongFromFavorites(songId, userId);
        }
    }

}