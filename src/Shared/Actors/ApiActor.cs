using System;
using System.Collections.Generic;
using System.Text;
using Actors.Messages;
using Akka.Actor;
using Akka.Routing;

namespace Actors
{
    public class ApiActor : ReceiveActor
    {
        private readonly IActorRef _watchedVideoActor;
        private readonly IActorRef _videoActor;
        private readonly IActorRef _recommandationActor;

        public ApiActor(IActorRef watchedVideoActor, IActorRef videoActor)
        {
            _watchedVideoActor = watchedVideoActor;
            _videoActor = videoActor;
            _recommandationActor = Context.ActorOf(Props.Create<RecommandationActor>(watchedVideoActor, videoActor).WithRouter(FromConfig.Instance), "recommandation");

            Receive<LoginMessage>(message =>
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{message.UserId} icin login talebi geldi");
                Console.ResetColor();

                _recommandationActor.Tell(new StartRecommendation(message.UserId, Sender));
            });

            Receive<WatchedVideoEvent>(videos =>
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"{videos.UserId} {videos.VideoId} video'yu izlemeye başladı");
                Console.ResetColor();

                _watchedVideoActor.Tell(videos, Sender);
            });
        }
    }
}
