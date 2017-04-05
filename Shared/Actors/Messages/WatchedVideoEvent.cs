namespace Actors.Messages
{
    public class WatchedVideoEvent
    {
        private readonly string _userId;
        private readonly int _videoId;

        public WatchedVideoEvent(string userId, int videoId)
        {
            _userId = userId;
            _videoId = videoId;
        }

        public string UserId
        {
            get { return _userId; }
        }

        public int VideoId
        {
            get { return _videoId; }
        }
    }
}