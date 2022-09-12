using System;
using System.Linq;
using Xamarin.Forms;
using YoutubeInWebView.Dtos.Api.Search;
using YoutubeInWebView.Services;
using YoutubeInWebView.UI.ViewModels;
using YoutubeInWebView.UI.Controls;

namespace YoutubeInWebView
{
    public partial class MainPage : ContentPage
    {
        private SearchResponseDto _searchResponse;
        private PlayerState _state;

        public MainPage()
        {
            InitializeComponent();r
            PlayButton.Clicked += PlayButton_Clicked;
            PauseButton.Clicked += PauseButton_Clicked;
            StopButton.Clicked += StopButton_Clicked;
            PlayButton.Clicked += PlayPause;
            NextButton.Clicked += NextButton_Clicked;
            PreviousButton.Clicked += PreviousButton_Clicked;

            SearchButton.Clicked += SearchButton_Clicked;
            PresetsList.ItemSelected += PresetsList_ItemSelected;
            
            var repo = DependencyService.Get<VideoRepository>();
            var videos = repo.GetVideos();
            TimelineView.Init(videos.ToList(), YtPlayerWebview);

            YtPlayerWebview.OnPlayerStateChange += YoutubeWebView_OnPlayerStateChange;
        }

        private void PreviousButton_Clicked(object sender, EventArgs e)
        {
            TimelineView.Previous();
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            TimelineView.Next();
        }

        private void YoutubeWebView_OnPlayerStateChange(object sender, PlayerState playerState)
        {
            _state = playerState;
        }

        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            var apiService = DependencyService.Get<IApiService>();
            _searchResponse = await apiService.SearchAsync("test", "test", 0, "test");
            var presetsVms = _searchResponse.Items
                .Select(item => new PresetViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    PreviewImageUrl = item.Thumbnails.Default.Url,
                    //PreviewImage = item.Thumbnails.Default.Url,
                })
                .ToList();
            PresetsList.ItemsSource = presetsVms;
        }

        private async void PresetsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var presetVm = e.SelectedItem as PresetViewModel;
            var searchResultItem = _searchResponse.Items.FirstOrDefault(item => item.Id == presetVm.Id);

            var apiService = DependencyService.Get<IApiService>();
            var preset = (await apiService.PresetsAsync(searchResultItem.PresetId)).Items.FirstOrDefault();

            if (preset == null)
            {
                await DisplayAlert("No presets found!", "No presets found!", "Cancel");
                return;
            }

            var repo = DependencyService.Get<VideoRepository>();
            var videos = repo.UpdateVideos(preset.Segments);
            TimelineView.Reinit(videos.ToList(), YtPlayerWebview);

            var video = videos.FirstOrDefault();
            if (video != null)
            {
                YtPlayerWebview.CueVideoById(new UI.Controls.Commands.LoadVideoByIdCmd
                {
                    VideoId = video.Id,
                    StartSeconds = (float)video.Start.TotalSeconds,
                    EndSeconds = (float)video.Stop.TotalSeconds,
                });
            }
        public void PlayPause(object sender, EventArgs e)
        {
            if (_state == PlayerState.PAUSED)
                YtPlayerWebview.PlayVideo();
            if (_state == PlayerState.PLAYING)
                YtPlayerWebview.PauseVideo();
        }
    }
}
