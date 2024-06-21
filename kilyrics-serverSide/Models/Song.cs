using kilyrics_serverSide.Models.DAL;

namespace kilyrics_serverSide.Models
{
    public class Song
    {
        int songId;
        string songName;
        string artist;
        string lyrics;
        int songInFavorite;

        public int SongId { get => songId; set => songId = value; }
        public string SongName { get => songName; set => songName = value; }
        public string Artist { get => artist; set => artist = value; }
        public string Lyrics { get => lyrics; set => lyrics = value; }
        public int SongInFavorite { get => songInFavorite; set => songInFavorite = value; }

        static public List<Song> GetAllSongs()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadAllSongs();
        }

        static public List<Song> ReadSongsByArtist(string artistName)
        {
            DBservices dbs = new DBservices();
            return dbs.ReadSongsByArtist(artistName);
        }

        static public List<Song> ReadSongsByName(string songName)
        {
            DBservices dbs = new DBservices();
            return dbs.ReadSongsByName(songName);
        }

        static public List<Song> ReadSongsByLyrics(string lyrics)
        {
            DBservices dbs = new DBservices();
            return dbs.ReadSongsByLyrics(lyrics);
        }
    }
}

