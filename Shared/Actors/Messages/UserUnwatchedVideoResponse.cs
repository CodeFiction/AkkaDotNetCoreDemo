using Actors.Models;

namespace Actors.Messages
{
    public class UserUnwatchedVideoResponse
    {
        private readonly StartRecommendation _startRecommendation;
        private readonly Video[] _videos;

        public UserUnwatchedVideoResponse(StartRecommendation startRecommendation, Video[] videos)
        {
            _startRecommendation = startRecommendation;
            _videos = videos;
        }

        public StartRecommendation Recommendation
        {
            get { return _startRecommendation; }
        }

        public Video[] Videos
        {
            get { return _videos; }
        }
    }
}