using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
namespace kilyrics_serverSide.Models.DAL;

public class DBservices
{
    public DBservices()
    {
        
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    // Create the SqlCommand using a stored procedure
    private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Dictionary<string, object> paramDic)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        if (paramDic != null)
            foreach (KeyValuePair<string, object> param in paramDic)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);

            }


        return cmd;
    }


    //===============================================================
    //                        *                    *
    //                        *    USERS  METHODS  *
    //                        *                    *
    //===============================================================

    // This method insert a user to the users table 
    public int InsertUser(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();

        paramDic.Add("@country", user.Country);
        paramDic.Add("@name", user.Name);
        paramDic.Add("@email", user.Email);
        paramDic.Add("@password", user.Password);
        paramDic.Add("@phoneNumber", user.PhoneNumber);
        //paramDic.Add("@regDate", user.RegistrationTime);



        cmd = CreateCommandWithStoredProcedure("SP_UserRegistration", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    // This method is for the login user
    public User LoginUser(string email, string password)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userEmail", email);
        paramDic.Add("@userPassword", password);

        cmd = CreateCommandWithStoredProcedure("SP_userLogin", con, paramDic);             // create the command
        var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

        returnParameter.Direction = ParameterDirection.ReturnValue;

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            dataReader.Read();

            User u = new User();
            u.Name = dataReader["name"].ToString();
            u.Country = dataReader["country"].ToString();
            u.Email = dataReader["email"].ToString();
            u.UserId = Convert.ToInt32(dataReader["userId"]);
            u.Password = dataReader["password"].ToString();
            u.PhoneNumber = dataReader["phoneNumber"].ToString();
            u.RegistrationTime = Convert.ToDateTime(dataReader["registrationDate"]);


            return u;
        }
        catch (Exception ex)
        {
            // write to log
            return null;
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }

        }

    }

    // This method Reads all favotites of a user 
    public List<Song> GetAllFavoritesOfUser(int userId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            //write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userId", userId);


        cmd = CreateCommandWithStoredProcedure("SP_GetFavoriteSongsOfUser", con, paramDic);             // create the command
        var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

        returnParameter.Direction = ParameterDirection.ReturnValue;


        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Song> songsList = new List<Song>();

            while (dataReader.Read())
            {
                Song s = new Song();

                s.SongId = Convert.ToInt32(dataReader["songId"]);
                s.SongInFavorite = Convert.ToInt32(dataReader["songInFavorite"]);
                s.Artist = dataReader["artist"].ToString();
                s.SongName = dataReader["song"].ToString();
                s.Lyrics = dataReader["text"].ToString();
                songsList.Add(s);

            }
            return songsList;

        }
        catch (Exception ex)
        {
            //write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                //close the db connection
                con.Close();
            }
            //note that the return value appears only after closing the connection
            var result = returnParameter.Value;
        }

    }

    // This method insert song to the favorites
    public int InsertSongToFavorites(int songId, int userId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@songId", songId);
        paramDic.Add("@userId", userId);



        cmd = CreateCommandWithStoredProcedure("SP_InsertSongToFavorites", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //this methode delete song from favorites of the user
    public int deleteSongFromFav(int songId, int userId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@songId", songId);
        paramDic.Add("@userId", userId);




        cmd = CreateCommandWithStoredProcedure("SP_deleteSongFromFavoritesOfUser", con, paramDic);             // create the command
        var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

        returnParameter.Direction = ParameterDirection.ReturnValue;


        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
            // note that the return value appears only after closing the connection
            var result = returnParameter.Value;
        }
    }

    //this method returns all users
    public List<User> GetAllUsers()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("SP_GetAllUsers", con, null);             // create the command


        List<User> userList = new List<User>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                User u = new User();
                u.UserId = Convert.ToInt32(dataReader["userId"]);
                u.Country = dataReader["country"].ToString();
                u.Name = dataReader["name"].ToString();
                u.Email = dataReader["email"].ToString();
                u.Password = dataReader["password"].ToString();
                u.RegistrationTime = Convert.ToDateTime(dataReader["registrationDate"]);
                u.PhoneNumber = dataReader["phoneNumber"].ToString();

                userList.Add(u);
            }
            return userList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //===============================================================
    //                        *                    *
    //                        *    SONGS  METHODS  *
    //                        *                    *
    //===============================================================

    // This method is is getting all the songs
    public List<Song> ReadAllSongs()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("SP_getAllSongs", con, null);             // create the command


        List<Song> songsList = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song s = new Song();
                s.SongId = Convert.ToInt32(dataReader["songId"]);
                s.SongInFavorite = Convert.ToInt32(dataReader["songInFavorite"]);
                s.Artist = dataReader["artist"].ToString();
                s.SongName = dataReader["song"].ToString();
                s.Lyrics = dataReader["text"].ToString();
                songsList.Add(s);
            }
            return songsList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    // This method Reads songs by specific artist
    public List<Song> ReadSongsByArtist(string artistName)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artistName);


        cmd = CreateCommandWithStoredProcedure("SP_GetSongsByArtistName", con, paramDic);             // create the command
        var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

        returnParameter.Direction = ParameterDirection.ReturnValue;


        List<Song> songList = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song s = new Song();
                s.SongId = Convert.ToInt32(dataReader["songId"]);
                s.SongInFavorite = Convert.ToInt32(dataReader["songInFavorite"]);
                s.Artist = dataReader["artist"].ToString();
                s.SongName = dataReader["song"].ToString();
                s.Lyrics = dataReader["text"].ToString();
                songList.Add(s);
            }

            return songList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
            // note that the return value appears only after closing the connection
            var result = returnParameter.Value;
        }

    }

    // This method Reads songs by specific name
    public List<Song> ReadSongsByName(string songName)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@name", songName);


        cmd = CreateCommandWithStoredProcedure("SP_GetSongsByName", con, paramDic);             // create the command
        var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

        returnParameter.Direction = ParameterDirection.ReturnValue;


        List<Song> songList = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song s = new Song();
                s.SongId = Convert.ToInt32(dataReader["songId"]);
                s.SongInFavorite = Convert.ToInt32(dataReader["songInFavorite"]);
                s.Artist = dataReader["artist"].ToString();
                s.SongName = dataReader["song"].ToString();
                s.Lyrics = dataReader["text"].ToString();
                songList.Add(s);
            }

            return songList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
            // note that the return value appears only after closing the connection
            var result = returnParameter.Value;
        }

    }

    public List<Song> ReadSongsByLyrics(string lyrics)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@lyrics", lyrics);


        cmd = CreateCommandWithStoredProcedure("SP_GetSongsByLyrics", con, paramDic);             // create the command
        var returnParameter = cmd.Parameters.Add("@returnValue", SqlDbType.Int);

        returnParameter.Direction = ParameterDirection.ReturnValue;


        List<Song> songList = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song s = new Song();
                s.SongId = Convert.ToInt32(dataReader["songId"]);
                s.SongInFavorite = Convert.ToInt32(dataReader["songInFavorite"]);
                s.Artist = dataReader["artist"].ToString();
                s.SongName = dataReader["song"].ToString();
                s.Lyrics = dataReader["text"].ToString();
                songList.Add(s);
            }

            return songList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
            // note that the return value appears only after closing the connection
            var result = returnParameter.Value;
        }

    }

    //===============================================================
    //                        *                     *
    //                        *    ARTIST  METHODS  *
    //                        *                     *
    //===============================================================

    //this method returns all artists names
    public List<string> GetAllArtists()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("SP_GetAllArtists", con, null);             // create the command


        List<string> ArtistList = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string nameOfArtist = dataReader["artist"].ToString(); ;

                ArtistList.Add(nameOfArtist);
            }
            return ArtistList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //this method returns the number of times an artist apears in favorites
    public int GetArtistFavoriteCount(string artistName)
    {
        SqlConnection con = null;
        SqlCommand cmd = null;

        try
        {
            con = connect("myProjDB"); // create the connection

            // Create and configure the SqlCommand
            cmd = new SqlCommand("SP_GetArtistFavoritesCount", con);
            cmd.CommandType = CommandType.StoredProcedure;

            // Add the @artistName parameter
            cmd.Parameters.Add("@artistName", SqlDbType.NVarChar, 255).Value = artistName;

            // Add the @totalFavoritesCount OUTPUT parameter
            SqlParameter totalFavoritesCountParam = new SqlParameter("@totalFavoritesCount", SqlDbType.Int);
            totalFavoritesCountParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(totalFavoritesCountParam);

            // Open the connection and execute the command
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            cmd.ExecuteNonQuery();

            // Get the value of the @totalFavoritesCount parameter
            int res = Convert.ToInt32(totalFavoritesCountParam.Value);

            return res;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (cmd != null)
            {
                cmd.Dispose();
            }
            if (con != null && con.State == ConnectionState.Open)
            {
                // close the db connection
                con.Close();
            }
        }
    }

}