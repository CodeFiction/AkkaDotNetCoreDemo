namespace Actors.Messages
{
    public class UserWatchedVideoResponse
    {
        private readonly StartRecommendation _startRecommendation;
        private readonly int[] _videoIds;

        public UserWatchedVideoResponse(StartRecommendation startRecommendation, int[] videoIds)
        {
            _startRecommendation = startRecommendation;
            _videoIds = videoIds;
        }

        public StartRecommendation StartRecommendation
        {
            get { return _startRecommendation; }
        }

        public int[] VideoIds
        {
            get { return _videoIds; }
        }
    }
}