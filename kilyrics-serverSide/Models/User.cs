using kilyrics_serverSide.Models.DAL;

namespace kilyrics_serverSide.Models
{

    //תעיפיייי אותוווווו!!!!
    public class User
    {

        int userId;
        string name;
        string country;
        string email;
        string password;
        string phoneNumber;
        DateTime registrationTime;



        public int UserId { get => userId; set => userId = value; }
        public string Name { get => name; set => name = value; }
        public string Country { get => country; set => country = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public DateTime RegistrationTime { get => registrationTime; set => registrationTime = value; }


        //public int Update()
        //{
        //    DBservices dbs = new DBservices();
        //    return dbs.UpdateUser(this);

        //}

        public override bool Equals(object? obj)
        {
            return obj is User user &&
                   (email == user.email ||
                    phoneNumber == user.phoneNumber);

        }

        public bool Registration()
        {
            DBservices dbs = new DBservices();
            if (dbs.InsertUser(this) > 0)
            {
                return true;
            }
            return false;
        }

        static public List<User> getAllUsers()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllUsers();
        }

        static public User logIn(string email, string password)
        {
            DBservices dbs = new DBservices();
            return dbs.LoginUser(email, password);

            throw new Exception("The data you have entered are not true");
        }


        public static bool InsertSongToFavorites(int songId, int userId)
        {
            DBservices dbs = new DBservices();
            if (dbs.InsertSongToFavorites(songId, userId) > 0)
            {
                return true;
            }
            return false;
        }

        static public List<Song> GetFavoriteSongsOfUser(int userId)
        {

            DBservices dbs = new DBservices();
            return dbs.GetAllFavoritesOfUser(userId);
        }

        static public List<Song> deleteSongFromFavorites(int songId, int userId)
        {
            DBservices db = new DBservices();
            if (db.deleteSongFromFav(songId, userId) != 0)
            {
                return db.GetAllFavoritesOfUser(userId);
            }


            throw new Exception("Exception from 'deleteSongFromFavorites' methode");

        }
    }
}
