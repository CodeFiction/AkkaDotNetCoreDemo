using System;
using Actors.Messages;
using Akka.Actor;
using Akka.Routing;

namespace Actors
{
    public class ApiActor : ReceiveActor
    {
        public ApiActor(IActorRef watchedVideoActor, IActorRef videoActor)
        {
            var recommandationActor = Context.ActorOf(Props.Create<RecommandationActor>(watchedVideoActor, videoActor).WithRouter(FromConfig.Instance), "recommandation");

            Receive<LoginMessage>(message =>
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{message.UserId} has sended login request.");
                Console.ResetColor();

                recommandationActor.Tell(new StartRecommendation(message.UserId, message.Client));
            });

            Receive<WatchVideoEvent>(watchVideoEvent =>
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{watchVideoEvent.UserId} has sent watch requests for {watchVideoEvent.VideoId}");
                Console.ResetColor();

                watchedVideoActor.Tell(watchVideoEvent, watchVideoEvent.Client);
            });
        }
    }
}
