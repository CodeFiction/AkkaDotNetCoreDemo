namespace Actors.Messages
{
    public class BeginAttempt
    {
        private readonly StartRecommendation _startRecommendation;

        public BeginAttempt(StartRecommendation startRecommendation)
        {
            _startRecommendation = startRecommendation;
        }

        public StartRecommendation Recommendation
        {
            get { return _startRecommendation; }
        }
    }
}