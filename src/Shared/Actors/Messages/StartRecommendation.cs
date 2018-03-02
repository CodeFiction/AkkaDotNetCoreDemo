using System;
using Akka.Actor;

namespace Actors.Messages
{
    public class StartRecommendation
    {
        private readonly string _userId;
        private readonly IActorRef _client;

        public StartRecommendation(string userId, IActorRef client)
        {
            _userId = userId;
            _client = client;
        }

        public string UserId
        {
            get { return _userId; }
        }

        public IActorRef Client
        {
            get { return _client; }
        }
    }
}