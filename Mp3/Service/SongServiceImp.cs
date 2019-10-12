using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mp3.Constant;
using Mp3.Entity;
using Mp3.Pages;
using Newtonsoft.Json;

namespace Mp3.Service
{
    class SongServiceImp : ISongService
    {
        
        public ObservableCollection<Song> GetSongs(string token, string apiUrl)
        {
            ObservableCollection<Song> songs = new ObservableCollection<Song>();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + token);
            var responseContent = client.GetAsync(apiUrl).Result.Content.ReadAsStringAsync().Result;
            songs = JsonConvert.DeserializeObject<ObservableCollection<Song>>(responseContent);
            return songs;
        }

        public ObservableCollection<Song> GetSongs(string apiUrl)
        {
            ObservableCollection<Song> songs = new ObservableCollection<Song>();
            var client = new HttpClient();
            var responseContent = client.GetAsync(apiUrl).Result.Content.ReadAsStringAsync().Result;
            songs = JsonConvert.DeserializeObject<ObservableCollection<Song>>(responseContent);
            return songs;
        }

        public Song PostSong(Song song,string token)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + token);
                HttpContent content = new StringContent(JsonConvert.SerializeObject(song), Encoding.UTF8,
                    "application/json");
                Task<HttpResponseMessage> httpRequestMessage = httpClient.PostAsync(ApiUrl.SONG_URL, content);
                var responseContent = httpRequestMessage.Result.Content.ReadAsStringAsync().Result;
                var resSong = JsonConvert.DeserializeObject<Song>(responseContent);
                Debug.WriteLine(responseContent);
                
                return resSong;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
