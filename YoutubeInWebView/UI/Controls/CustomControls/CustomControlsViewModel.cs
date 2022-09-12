using System;
using System.Collections.Generic;
using System.Text;

using YoutubeInWebView.UI.Controls.Commands;
using YoutubeInWebView.UI.Controls.SegmentedTimeline.Models;
using YoutubeInWebView.UI.ViewModels;

namespace YoutubeInWebView.UI.Controls.CustomControls
{
    public class CustomControlsViewModel
    {
        private readonly YoutubeWebView _youtubeWebView;

        private PlayerState _state;

        public CustomControlsViewModel(YoutubeWebView youtubeWebView)
        {
            _youtubeWebView = youtubeWebView;

            _youtubeWebView.OnPlayerStateChange += YoutubeWebView_OnPlayerStateChange;
        }


        private void YoutubeWebView_OnPlayerStateChange(object sender, PlayerState playerState)
        {
            _state = playerState;
        }

        public void PlayPause()
        {
            if (_state == PlayerState.PAUSED)
                _youtubeWebView.PlayVideo();
            if (_state == PlayerState.PLAYING)
                _youtubeWebView.PauseVideo();
        }
    }
}
