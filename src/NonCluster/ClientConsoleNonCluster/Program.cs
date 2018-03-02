using System;
using System.Threading;
using Actors;
using Actors.Messages;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Config hoconConfiguration = GetHoconConfiguration();
            ActorSystem actorSystem = ActorSystem.Create("moviedb", hoconConfiguration);

            IActorRef watchedVideo = actorSystem.ActorOf(Props.Create<WatchedVideoRepoActor>().WithRouter(FromConfig.Instance), "watchedVideo");
            IActorRef videoRepo = actorSystem.ActorOf(Props.Create<VideoRepoActor>().WithRouter(FromConfig.Instance), "videoRepo");
            IActorRef consoleLogger = actorSystem.ActorOf(Props.Create<ConsoleLoggerActor>(), "logger");

            IActorRef apiActor = actorSystem.ActorOf(Props.Create<ApiActor>(watchedVideo, videoRepo), "api");

            apiActor.Tell(new LoginMessage("deniz"), consoleLogger);

            apiActor.Tell(new WatchedVideoEvent("deniz", 0));
            apiActor.Tell(new WatchedVideoEvent("deniz", 4));
            apiActor.Tell(new WatchedVideoEvent("deniz", 7));
            apiActor.Tell(new WatchedVideoEvent("deniz", 10));
            apiActor.Tell(new WatchedVideoEvent("deniz", 2));

            apiActor.Tell(new LoginMessage("deniz"), consoleLogger);

            Console.Read();

            actorSystem.Terminate();
        }

        public static Config GetHoconConfiguration()
        {
            string config = @"					akka {
						actor {
										 deployment {
												/videoRepo {
													 router = round-robin-pool
													 nr-of-instances = 2
												}
												
												/watchedVideo {
													 router = round-robin-pool
													 nr-of-instances = 1
												}

												/api/recommandation {
													 router = round-robin-pool
													 nr-of-instances = 4
												}
									}
							 }";

            return ConfigurationFactory.ParseString(config);
        }
    }
}