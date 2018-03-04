namespace Actors.Messages
{
    public class UserWatchHistoryRequest
    {
        private readonly StartRecommendation _recommendation;

        public UserWatchHistoryRequest(StartRecommendation recommendation)
        {
            _recommendation = recommendation;
        }

        public StartRecommendation Recommendation
        {
            get { return _recommendation; }
        }
    }
}