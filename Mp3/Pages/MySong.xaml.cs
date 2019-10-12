using Mp3.Constant;
using Mp3.Entity;
using Mp3.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Mp3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MySong : Page
    {
        private ISongService songService;
        ObservableCollection<Song> _songs;
        MemberServiceImp memberService;
        private string loginToken;
        private bool _isPlaying;
        private int _currentIndex = 0;
        public MySong()
        {
            this.memberService = new MemberServiceImp();
            loginToken = memberService.ReadTokenFromLocalStorage();
            if (loginToken == null)
            {
            }
            else
            {
                this.InitializeComponent();
                this.songService = new SongServiceImp();
                LoadSongs();
                if (_songs.Count == 0)
                {
                    this.empty.Visibility = Visibility.Visible;
                    this.media.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void LoadSongs()
        {
            _songs = songService.GetSongs(loginToken, ApiUrl.GET_MINE_SONG_URL);
            MyListSong.ItemsSource = _songs;
            _currentIndex = 0;
        }
        private void SelectSong(object sender, TappedRoutedEventArgs e)
        {
            var selectItem = sender as StackPanel;
            MyMediaPlayer.Pause();
            if (selectItem != null)
            {
                if (selectItem.Tag is Song currentSong)
                {
                    _currentIndex = _songs.IndexOf(currentSong);
                    MyMediaPlayer.Source = new Uri(currentSong.link);
                    Play();
                }
            }
        }
        private void Play()
        {
            MyMediaPlayer.Source = new Uri(_songs[_currentIndex].link);
            ControlLabel.Text = "Now Playing: " + _songs[_currentIndex].name;
            MyListSong.SelectedIndex = _currentIndex;
            MyMediaPlayer.Play();
            StatusButton.Icon = new SymbolIcon(Symbol.Pause);
            _isPlaying = true;
        }
        private void Pause()
        {
            ControlLabel.Text = "Paused";
            MyMediaPlayer.Pause();
            StatusButton.Icon = new SymbolIcon(Symbol.Play);
            _isPlaying = false;
        }
        private void StatusButton_OnClick(object sender, RoutedEventArgs e)
        {

            if (_isPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }
        private void PreviousButton_OnClick(object sender, RoutedEventArgs e)
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = _songs.Count - 1;
            }
            else if (_currentIndex >= _songs.Count)
            {
                _currentIndex = 0;
            }
            Play();
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            _currentIndex++;
            if (_currentIndex >= _songs.Count || _currentIndex < 0)
            {
                _currentIndex = 0;
            }
            Play();
        }

        private void RedirectUpload(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Upload));
        }
    }
}
