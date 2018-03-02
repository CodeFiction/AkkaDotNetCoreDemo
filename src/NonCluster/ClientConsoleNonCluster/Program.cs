using System;
using System.IO;
using Actors;
using Actors.Messages;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using Client;

namespace ClientConsoleNonCluster
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = ConfigurationFactory.ParseString(File.ReadAllText("akka-config.hocon"));
			ActorSystem actorSystem = ActorSystem.Create("moviedb", config);

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

			CoordinatedShutdown.Get(actorSystem).Run().Wait();
		}
	}
}