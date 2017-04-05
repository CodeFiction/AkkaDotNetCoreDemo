namespace Actors.Messages
{
    public class UserUnwatchedVideoRequest
    {
        private readonly StartRecommendation _responseStartRecommendation;
        private readonly int[] _watchedVideos;

        public UserUnwatchedVideoRequest(StartRecommendation responseStartRecommendation, int[] watchedVideos)
        {
            _responseStartRecommendation = responseStartRecommendation;
            _watchedVideos = watchedVideos;
        }

        public StartRecommendation ResponseStartRecommendation
        {
            get { return _responseStartRecommendation; }
        }

        public int[] Videos
        {
            get { return _watchedVideos; }
        }
    }
}