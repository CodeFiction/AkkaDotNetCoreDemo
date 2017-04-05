namespace Actors.Messages
{
    public class LoginMessage
    {
        private readonly string _userId;

        public LoginMessage(string userId)
        {
            _userId = userId;
        }

        public string UserId
        {
            get { return _userId; }
        }
    }
}