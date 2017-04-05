namespace Actors.Messages
{
    public class VideoStatus
    {
        private readonly string _userId;
        private readonly int _videoId;
        private readonly string _status;

        public VideoStatus(string userId, int videoId, string status)
        {
            _userId = userId;
            _videoId = videoId;
            _status = status;
        }

        public string UserId
        {
            get { return _userId; }
        }

        public int VideoId
        {
            get { return _videoId; }
        }

        public string Status
        {
            get { return _status; }
        }
    }
}