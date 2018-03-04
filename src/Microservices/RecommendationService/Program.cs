using System;
using System.IO;
using Akka.Actor;
using Akka.Configuration;

namespace RecommendationService
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("akka-config.hocon"));

            ActorSystem actorSystem = ActorSystem.Create("moviedb", config);

            Console.ReadLine();

            CoordinatedShutdown.Get(actorSystem).Run().Wait();
        }
    }
}
