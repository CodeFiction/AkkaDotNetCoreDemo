using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Actors.Messages;
using Actors.Models;
using Akka.Actor;
using Newtonsoft.Json;

namespace ClientConsoleNonCluster
{
    public class ConsoleLoggerActor : ReceiveActor
    {
        public static TaskCompletionSource<bool> CompletionSource;

        public ConsoleLoggerActor()
        {
            Receive<RecommendationResponse>(response =>
            {
                // Issue about interoperability between .NET Full and .NET Core versions
                // https://github.com/akkadotnet/akka.net/issues/3226
                var videos = JsonConvert.DeserializeObject<IList<Video>>(response.ResponseVideosJsonPaylod);
                
                foreach (var responseResponseVideo in videos)
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
