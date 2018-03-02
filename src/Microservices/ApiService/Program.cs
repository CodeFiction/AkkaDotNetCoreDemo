using System;
using System.IO;
using Actors;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;

namespace ApiService
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("akka-config.hocon"));
            ActorSystem actorSystem = ActorSystem.Create("moviedb", config);

            IActorRef watchedVideo = actorSystem.ActorOf(Props.Create<WatchedVideoRepoActor>().WithRouter(FromConfig.Instance), "watchedVideo");
            IActorRef videoRepo = actorSystem.ActorOf(Props.Create<VideoRepoActor>().WithRouter(FromConfig.Instance), "videoRepo");

            IActorRef apiActor = actorSystem.ActorOf(Props.Create<ApiActor>(watchedVideo, videoRepo), "api");

            Console.ReadLine();

            CoordinatedShutdown.Get(actorSystem).Run().Wait();
        }
    }
}
