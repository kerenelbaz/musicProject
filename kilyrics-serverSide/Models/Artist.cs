using kilyrics_serverSide.Models.DAL;

namespace kilyrics_serverSide.Models
{
    public class Artist
    {

        string artistName;
        string img;
        string artistBiography;
        int artistInFavorite;

        public string ArtistName { get => artistName; set => artistName = value; }
        public string ArtistIMG { get => img; set => img = value; }

        public string ArtistBiography { get => artistBiography; set => artistBiography = value; }
        public int ArtistInFavorite { get => artistInFavorite; set => artistInFavorite = value; }

        public static List<string> GetAllArtists()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllArtists();
        }

        public static int getFavoriteCount(string artistName)
        {
            DBservices dbs = new DBservices();
            return dbs.GetArtistFavoriteCount(artistName);
        }


    }
}
