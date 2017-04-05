namespace Actors.Messages
{
    public class UserWatchedVideoRequest
    {
        private readonly StartRecommendation _attemptRecommendation;

        public UserWatchedVideoRequest(StartRecommendation attemptRecommendation)
        {
            _attemptRecommendation = attemptRecommendation;
        }

        public StartRecommendation AttemptRecommendation
        {
            get { return _attemptRecommendation; }
        }
    }
}