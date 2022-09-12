using System;
using YoutubeInWebView.Dtos.Api.Presets;

namespace YoutubeInWebView.Dtos
{
    public class VideoDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Index { get; set; } = -1;

        public TimeSpan Start { get; set; }
        public TimeSpan Stop { get; set; }

        public TimeSpan Duration => Stop - Start;

        public static VideoDto FromSegmentDto(SegmentDto segmentDto, int index = -1) =>
            new VideoDto()
            {
                Id = segmentDto.VideoSource.Id,
                Title = segmentDto.Title,
                Index = index,
                Start = TimeSpan.FromSeconds(segmentDto.VideoSource.StartTs),
                Stop = TimeSpan.FromSeconds(segmentDto.VideoSource.StopTs),
            };
    }
}
