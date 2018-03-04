namespace Actors.Messages
{
    public class UserWatchedVideoRequest
    {
        private readonly StartRecommendation _recommendation;

        public UserWatchedVideoRequest(StartRecommendation recommendation)
        {
            _recommendation = recommendation;
        }

        public StartRecommendation Recommendation
        {
            get { return _recommendation; }
        }
    }
}