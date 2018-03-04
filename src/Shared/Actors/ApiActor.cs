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
                Console.WriteLine($"{message.UserId}, login talebi gönderdi.");
                Console.ResetColor();

                recommandationActor.Tell(new StartRecommendation(message.UserId, message.Client));
            });

            Receive<WatchVideoEvent>(watchVideoEvent =>
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{watchVideoEvent.UserId}, {watchVideoEvent.VideoId} id'li video için izleme talebi gönderdi.");
                Console.ResetColor();

                watchedVideoActor.Tell(watchVideoEvent, watchVideoEvent.Client);
            });
        }
    }
}
