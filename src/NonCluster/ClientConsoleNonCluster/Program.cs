using System;
using System.IO;
using System.Threading.Tasks;
using Actors;
using Actors.Messages;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;

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

            Console.Write("Login user : ");
            string userName = Console.ReadLine();

            ConsoleLoggerActor.CompletionSource = new TaskCompletionSource<bool>();

            apiActor.Tell(new LoginMessage(userName, consoleLogger));

            ConsoleLoggerActor.CompletionSource.Task.Wait();

            bool watch = true;
            while (watch)
            {
                Console.WriteLine("Enter the id of the video you want to watch.");
                string strId = Console.ReadLine();

                if (!int.TryParse(strId, out int id))
                {
                    Console.WriteLine("Invalid id has entered.");
                    continue;
                }

                ConsoleLoggerActor.CompletionSource = new TaskCompletionSource<bool>();
                apiActor.Tell(new WatchVideoEvent(userName, id, consoleLogger));
                apiActor.Tell(new LoginMessage(userName, consoleLogger));
                ConsoleLoggerActor.CompletionSource.Task.Wait();

                Console.WriteLine("Do you want to continue watching video? y/n");
                string answer = Console.ReadLine();

                if (answer != null && !answer.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                {
                    watch = false;
                }
            }                                 

            CoordinatedShutdown.Get(actorSystem).Run().Wait();
        }
    }
}