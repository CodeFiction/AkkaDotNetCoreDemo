using Akka.Actor;

namespace Actors.Messages
{
    public class LoginMessage
    {
        private readonly string _userId;
        private readonly IActorRef _client;

        public LoginMessage(string userId, IActorRef client)
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