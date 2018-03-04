using System.Collections.Generic;
using Actors.Models;

namespace Actors.Messages
{
    public class RecommendationResponse
    {
        private readonly string _userId;
        private readonly string _responseVideosJsonPaylod;

        public RecommendationResponse(string userId, string responseVideosJsonPaylod)
        {
            _userId = userId;
            _responseVideosJsonPaylod = responseVideosJsonPaylod;
        }

        public string ResponseVideosJsonPaylod
        {
            get { return _responseVideosJsonPaylod; }
        }

        public string UserId
        {
            get { return _userId; }
        }
    }
}