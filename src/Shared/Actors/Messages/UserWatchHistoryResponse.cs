namespace Actors.Messages
{
    public class UserWatchHistoryResponse
    {
        private readonly StartRecommendation _startRecommendation;
        private readonly int[] _videoIds;

        public UserWatchHistoryResponse(StartRecommendation startRecommendation, int[] videoIds)
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