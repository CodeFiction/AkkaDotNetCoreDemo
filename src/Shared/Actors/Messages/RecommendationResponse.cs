using System.Collections.Generic;
using Actors.Models;

namespace Actors.Messages
{
    public class RecommendationResponse
    {
        private readonly string _userId;
        private readonly List<Video> _responseVideos;

        public RecommendationResponse(string userId, List<Video> responseVideos)
        {
            _userId = userId;
            _responseVideos = responseVideos;
        }

        public List<Video> ResponseVideos
        {
            get { return _responseVideos; }
        }

        public string UserId
        {
            get { return _userId; }
        }
    }
}