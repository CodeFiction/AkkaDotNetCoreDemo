using Akka.Actor;

namespace Actors.Messages
{
    public class WatchVideoEvent
    {
        private readonly string _userId;
        private readonly int _videoId;
        private readonly IActorRef _client;

        public WatchVideoEvent(string userId, int videoId, IActorRef client)
        {
            _userId = userId;
            _videoId = videoId;
            _client = client;
        }

        public string UserId
        {
            get { return _userId; }
        }

        public int VideoId
        {
            get { return _videoId; }
        }

        public IActorRef Client
        {
            get { return _client; }
        }
    }
}