using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YoutubeInWebView.Dtos;
using YoutubeInWebView.UI.Controls.SegmentedTimeline.Models;
using YoutubeInWebView.UI.Controls.SegmentedTimeline.ViewModels;

namespace YoutubeInWebView.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SegmentedTimelineView : ContentView, IDisposable
    {
        private static readonly TimeSpan TouchAheadInterval = TimeSpan.FromMilliseconds(400);
        private static readonly float MarginToHeightRatio = 0.15f;

        private float _relativePosition = 0;
        private int _width;

        private List<TimelineFragment> _fragments;
        private bool _isDisposed;
        private bool _forceUpdateFragments = false;

        private DateTime _lastTouchDateTime;

        public SegmentedTimelineView()
        {
            InitializeComponent();
        }
        
        public static event Action TouchAction;

        public void Init(IReadOnlyList<VideoDto> videos, YoutubeWebView youtubeWebView)
        {
            ViewModel = new SegmentedTimelineViewModel(videos, youtubeWebView);
            ViewModel.PositionChanged += OnPositionChanged;
            BindingContext = ViewModel;

            Canvas.InvalidateSurface();
        }

        public void Reinit(IReadOnlyList<VideoDto> videos, YoutubeWebView youtubeWebView)
        {
            if (ViewModel != null)
                ViewModel.PositionChanged -= OnPositionChanged;

            _forceUpdateFragments = true;

            Init(videos, youtubeWebView);
        }

        private SegmentedTimelineViewModel ViewModel { get; set; }

        private SKPaint BackgroundPaint => TimelineCanvasConfig.BackgroundPaint;
        private SKPaint RightPaint => TimelineCanvasConfig.RightPaint;
        private SKPaint SliderPaint => TimelineCanvasConfig.SliderPaint;
        private SKPaint BlackPaint => TimelineCanvasConfig.BlackPaint;

        public void Dispose()
        {
            if (_isDisposed)
                return;

            try
            {
                _isDisposed = true;
                ViewModel.PositionChanged -= OnPositionChanged;
                Canvas.PaintSurface -= OnPaintSurface;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        //TODO: Новые методы
        public void Previous() => ViewModel.Previous();

        public void Next() => ViewModel.Next();


        private void OnPositionChanged(object sender, float relativePosition)
        {
            if (ViewModel.CurrentIndex != 0 && relativePosition < 0.001)
                return;

            _relativePosition = relativePosition;
            Canvas.InvalidateSurface();
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (_isDisposed)
                return;

            DrawTimeline(e.Info.Width, e.Info.Height, e.Surface.Canvas);
        }

        private void OnTouchAction(object sender, TouchActionEventArgs args)
        {
            if (_isDisposed)
                return;

            SeekTo(args.Type, args.Location);
        }

        private void DrawTimeline(int width, int height, SKCanvas canvas)
        {
            if (_isDisposed)
                return;

            var margin = MarginToHeightRatio * height;
            canvas.Clear();

            UpdateFragments();

            if (_fragments == null || !_fragments.Any())
                return;

            // the whole gradient timeline
            canvas.DrawRect(0, margin, width, height - 2 * margin, BackgroundPaint);
            canvas.DrawRect(0, margin, _relativePosition * width, height - 2 * margin, GetLeftPaint());
            canvas.DrawRect(_relativePosition * width, margin, width, height - 2 * margin, RightPaint);

            // black areas around all except current fragments
            var currentFragment = _fragments.FirstOrDefault(tf => tf.Index == ViewModel.CurrentIndex);
            if (currentFragment != null)
            {
                canvas.DrawRect(0, 0, currentFragment.StartX, 2 * margin, BlackPaint);
                canvas.DrawRect(currentFragment.EndX, 0, width, 2 * margin, BlackPaint);
                canvas.DrawRect(0, height - 2 * margin, currentFragment.StartX, height, BlackPaint);
                canvas.DrawRect(currentFragment.EndX, height - 2 * margin, width, height, BlackPaint);

                canvas.DrawRoundRect(
                    new SKRoundRect(
                        new SKRect(_relativePosition * width, 0, _relativePosition * width + 10, height),
                        10),
                    SliderPaint);
            }

            // separators
            foreach (var timelineFragment in _fragments.Where(tf => tf.IsSeparator))
                canvas.DrawRect(timelineFragment.StartX, margin, timelineFragment.Width, height, BlackPaint);

            SKPaint GetLeftPaint() => TimelineCanvasConfig.GetLeftPaint(width, height);

            void UpdateFragments()
            {
                if (!_forceUpdateFragments
                    && ((_fragments != null && _width == width)
                        || ViewModel.FullDurationS == 0))
                    return;

                _fragments = GetFragments(width);
                _width = width;
            }
        }

        private void SeekTo(TouchActionType touchAction, TouchTrackingPoint touchPoint)
        {
            if (_lastTouchDateTime > DateTime.UtcNow && touchAction == TouchActionType.Pressed)
                return;

            _lastTouchDateTime = DateTime.UtcNow + TouchAheadInterval;

            var point = new SKPoint(
                (float) (Canvas.CanvasSize.Width * touchPoint.X / Canvas.Width),
                (float) (Canvas.CanvasSize.Height * touchPoint.Y / Canvas.Height));
            if (point.X < 0)
                point.X = 0;

            if (point.X > Canvas.CanvasSize.Width)
                point.X = Canvas.CanvasSize.Width;

            var fragment = GetFragments(Canvas.CanvasSize.Width)
                .FirstOrDefault(x => x.Contains(point.X));
            if (fragment == null || fragment.IsSeparator)
                return;

            var relativePosition = (point.X - fragment.StartX) / fragment.Width;
            if (touchAction == TouchActionType.Moved)
            {
                _relativePosition = point.X / Canvas.CanvasSize.Width;
                Canvas.InvalidateSurface();
            }

            ViewModel?.SeekTo(relativePosition, touchAction, fragment);
            TouchAction?.Invoke();
        }
        
        private List<TimelineFragment> GetFragments(float canvasWidth) =>
            new FragmentedTimelineFactory(canvasWidth, TimelineCanvasConfig.SeparatorWidth)
                .Create(ViewModel.FullDurationS, ViewModel.VideosDurations);
    }
}
