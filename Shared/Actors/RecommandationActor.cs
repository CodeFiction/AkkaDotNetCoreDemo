using System;
using System.Linq;
using System.Threading.Tasks;
using Actors.Messages;
using Actors.Models;
using Akka.Actor;
using Akka.Routing;

namespace Actors
{
    public class RecommandationActor : ReceiveActor
    {
        private readonly IActorRef _watchedVideoActor;
        private readonly IActorRef _videoRepoActor;

        private ICancelable _startAttempts;

        public RecommandationActor(IActorRef watchedVideoActor, IActorRef videoRepoActor)
        {
            _watchedVideoActor = watchedVideoActor;
            _videoRepoActor = videoRepoActor;    
            
            Attempt();      
        }

        public void Attempt()
        {
            Receive<StartRecommendation>(recommendation =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{recommendation.UserId} video tavsiye isteğinde bulundu");
                Console.ResetColor();

                _startAttempts = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.Zero, TimeSpan.FromMilliseconds(200), Self, new BeginAttempt(recommendation), ActorRefs.NoSender);

                Become(StartRecommendation);
            });
        }

        public void StartRecommendation()
        {
            Receive<BeginAttempt>(attempt =>
            {
                Task<Routees> watchedVideoRoutees = _watchedVideoActor.Ask<Routees>(new GetRoutees());
                Task<Routees> videoRoutees = _videoRepoActor.Ask<Routees>(new GetRoutees());

                Task.WhenAll(watchedVideoRoutees, videoRoutees)
                    .ContinueWith(allRoutess => new JobAttemptMessage(attempt.Recommendation, allRoutess.Result.All(routees => routees.Members.Any())))
                    .PipeTo(Self);
            });

            Receive<JobAttemptMessage>(message =>
            {
                if (!message.CanStart)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Film tavsiyesinde bulunulamiyor. Movie Repository ve Watched Video ayakta degil");
                    Console.ResetColor();

                    return;
                }

                _startAttempts.Cancel();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{message.Job.UserId} icin önceden izlemis oldugu video'lar soruluyor");
                Console.ResetColor();

                _watchedVideoActor.Tell(new UserWatchedVideoRequest(message.Job));
            });

            Receive<UserWatchedVideoResponse>(response =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{response.StartRecommendation.UserId} icin izlemis oldugu video'ların cevabı geldi, tavsiye edilecek video'lar soruluyor");
                Console.ResetColor();

                int[] watchedVideos = response.VideoIds;

                _videoRepoActor.Tell(new UserUnwatchedVideoRequest(response.StartRecommendation, watchedVideos));
            });

            Receive<UserUnwatchedVideoResponse>(response =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{response.Recommendation.UserId} icin tavsiye edilecek video'ların cevabı geldi.");
                Console.ResetColor();

                Video[] responseVideos = response.Videos;

                response.Recommendation.Client.Tell(new RecommendationResponse(response.Recommendation.UserId, responseVideos));

                // Self.Tell(PoisonPill.Instance); // Özel tipte bir mesaj, actor'ün kendini yok etmesini sağlıyor. Bu adımdan itibaren actor'le işimiz kalmıyor.
            });
        }
    }
}