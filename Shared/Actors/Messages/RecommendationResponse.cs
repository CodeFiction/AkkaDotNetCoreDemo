using Actors.Models;

namespace Actors.Messages
{
    public class RecommendationResponse
    {
        private readonly string _userId;
        private readonly Video[] _responseVideos;

        public RecommendationResponse(string userId, Video[] responseVideos)
        {
            _userId = userId;
            _responseVideos = responseVideos;
        }

        public Video[] ResponseVideos
        {
            get { return _responseVideos; }
        }

        public string UserId
        {
            get { return _userId; }
        }
    }
}