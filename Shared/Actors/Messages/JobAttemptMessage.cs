namespace Actors.Messages
{
    public class JobAttemptMessage
    {
        private readonly StartRecommendation _job;
        private readonly bool _canStart;

        public JobAttemptMessage(StartRecommendation job, bool canStart)
        {
            _job = job;
            _canStart = canStart;
        }

        public bool CanStart
        {
            get { return _canStart; }
        }

        public StartRecommendation Job
        {
            get { return _job; }
        }
    }
}