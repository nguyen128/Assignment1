using Mp3.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Service
{
    interface ISongService
    {
        Song PostSong(Song song, string token);
        ObservableCollection<Song> GetSongs(string token, string apiUrl);
        ObservableCollection<Song> GetSongs(string apiUrl);
    }
}
