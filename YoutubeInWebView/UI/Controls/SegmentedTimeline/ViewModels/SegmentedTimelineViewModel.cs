using System;
using System.Collections.Generic;
using System.Linq;

using TouchTracking;
using Xamarin.Essentials;
using YoutubeInWebView.Dtos;
using YoutubeInWebView.UI.Controls.Commands;
using YoutubeInWebView.UI.Controls.SegmentedTimeline.Models;
using YoutubeInWebView.UI.ViewModels;

namespace YoutubeInWebView.UI.Controls.SegmentedTimeline.ViewModels
{
    public class SegmentedTimelineViewModel : BaseViewModel
    {
        private readonly IReadOnlyList<VideoDto> _videos;
        private readonly YoutubeWebView _youtubeWebView;

        private int _currentIndex = 0;
        private PlayerState _playerState;
        private float _currentTime;

        public SegmentedTimelineViewModel(IReadOnlyList<VideoDto> videos, YoutubeWebView youtubeWebView)
        {
            VideosDurations = new TimeSpan[videos.Count];
            for (var i = 0; i < videos.Count; i++)
                VideosDurations[i] = videos[i].Duration;

            _videos = videos;
            _youtubeWebView = youtubeWebView;

            _youtubeWebView.OnPlayerReady += YoutubeWebView_OnPlayerReady;
            _youtubeWebView.OnPlayerStateChange += YoutubeWebView_OnPlayerStateChange;
            _youtubeWebView.OnCurrentTimeUpdate += YoutubeWebView_OnCurrentTimeUpdate;

            _youtubeWebView.YoutubeVideoId = _videos.FirstOrDefault()?.Id;
        }

        public event EventHandler<float> PositionChanged;

        public int CurrentIndex => _currentIndex;
        public TimeSpan[] VideosDurations { get; }
        public float FullDurationS => VideosDurations.Sum(d => (float)d.TotalSeconds);

        private void YoutubeWebView_OnPlayerReady(object sender, EventArgs e)
        {
            var video = _videos[_currentIndex];
            MainThread.BeginInvokeOnMainThread(() =>
                LoadVideo(video, (float)video.Start.TotalSeconds));
        }

        private void YoutubeWebView_OnPlayerStateChange(object sender, PlayerState playerState)
        {
            var prevPlayerState = _playerState;
            _playerState = playerState;

            if (playerState == PlayerState.ENDED)
            {
                var currentVideo = _videos[_currentIndex];
                if (_currentTime > currentVideo.Start.TotalSeconds && prevPlayerState == PlayerState.PLAYING)
                    PlayNext();
            }
        }

        private void YoutubeWebView_OnCurrentTimeUpdate(object sender, float currentTime)
        {
            _currentTime = currentTime;
            var currentVideo = _videos[_currentIndex];
            var currentTs = TimeSpan.FromSeconds(currentTime);
            if (currentTs < currentVideo.Start)
                return;

            var previousVideosDurationS = _videos.Where(v => v.Index < _currentIndex).Sum(v => v.Duration.TotalSeconds);
            VideoPlayer_PositionChanged(TimeSpan.FromSeconds(previousVideosDurationS) + currentTs - currentVideo.Start);
        }

        private void VideoPlayer_PositionChanged(TimeSpan position)
        {
            var value = FullDurationS < 0.00001F
                ? 0
                : (float)(position.TotalSeconds / FullDurationS);
            PositionChanged?.Invoke(this, value);
        }

        public void SeekTo(double relativePosition, TouchActionType touchAction, TimelineFragment timelineFragment)
        {
            var position = TimeSpan.FromSeconds(relativePosition * timelineFragment.Duration.TotalSeconds);
            var video = _videos[timelineFragment.Index];
            var seekToTs = video.Start + position;
            switch (touchAction)
            {
                case TouchActionType.Pressed:
                case TouchActionType.Moved:
                    if (_currentIndex != timelineFragment.Index)
                        _currentIndex = timelineFragment.Index;

                    break;
                case TouchActionType.Released:
                    if (_playerState == PlayerState.PAUSED)
                        CueVideo(video, (float)seekToTs.TotalSeconds);
                    else
                        LoadVideo(video, (float)seekToTs.TotalSeconds);

                    break;
            }
        }

        private void CueVideo(VideoDto video, float startSeconds)
        {
            _youtubeWebView.CueVideoById(new LoadVideoByIdCmd
            {
                VideoId = video.Id,
                StartSeconds = startSeconds,
                EndSeconds = (float)video.Stop.TotalSeconds,
            });
        }

        private void LoadVideo(VideoDto video, float startSeconds)
        {
            _youtubeWebView.LoadVideoById(new LoadVideoByIdCmd
            {
                VideoId = video.Id,
                StartSeconds = startSeconds,
                EndSeconds = (float)video.Stop.TotalSeconds,
            });
        }

        private void PlayNext()
        {
            if (_currentIndex < _videos.Count - 1)
            {
                var currentVideo = _videos[++_currentIndex];
                _currentTime = (float)currentVideo.Start.TotalSeconds;
                MainThread.BeginInvokeOnMainThread(() =>
                    LoadVideo(currentVideo, _currentTime));
            }
        }
        
        private void PlayPrevious()
        {
            if (_currentIndex > 0)
            {
                var currentVideo = _videos[--_currentIndex];
                _currentTime = (float)currentVideo.Start.TotalSeconds;
                MainThread.BeginInvokeOnMainThread(() =>
                    LoadVideo(currentVideo, _currentTime));
            }
        }

        //TODO: Ееще одни методы
        public void Next() => PlayNext();

        public void Previous() => PlayPrevious();
    }
}
