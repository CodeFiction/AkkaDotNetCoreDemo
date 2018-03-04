using System;
using System.Threading.Tasks;
using Actors.Messages;
using Akka.Actor;

namespace ClientConsoleNonCluster
{
    public class ConsoleLoggerActor : ReceiveActor
    {
        public static TaskCompletionSource<bool> CompletionSource;

        public ConsoleLoggerActor()
        {
            Receive<RecommendationResponse>(response =>
            {
                foreach (var responseResponseVideo in response.ResponseVideos)
                {
                    Console.WriteLine(responseResponseVideo);
                }

                CompletionSource.SetResult(true);
            });

            Receive<VideoStatus>(status =>
            {
                switch (status.Status)
                {
                    case "watching":
                        Console.WriteLine($"{status.UserId} has begun to watch {status.VideoId}.");
                        break;
                    case "stopped":
                        Console.WriteLine($"{status.UserId} has stop watch {status.VideoId}.");
                        break;
                }
            });
        }
    }
}
