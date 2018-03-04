using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Actors.Messages;
using Akka.Actor;

namespace Actors
{
    public class WatchedVideoRepoActor : ReceiveActor
    {
        private static readonly IList<WatchVideoEvent> Watched = new List<WatchVideoEvent>();

        public WatchedVideoRepoActor()
        {             
            Receive<UserWatchHistoryRequest>(request =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"The videos that {request.Recommendation.UserId} had previously watched has found.");
                Console.ResetColor();

                Thread.Sleep(50);

                int[] videoIds = Watched.Where(videos => videos.UserId == request.Recommendation.UserId)
                    .Select(videos => videos.VideoId)
                    .Distinct()
                    .ToArray();

                Sender.Tell(new UserWatchHistoryResponse(request.Recommendation, videoIds));
            });

            Receive<WatchVideoEvent>(message =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Playing the video {message.VideoId} for {message.UserId}");
                Console.ResetColor();

                IActorRef sender = message.Client;

                sender.Tell(new VideoStatus(message.UserId, message.VideoId, "watching"));

                Thread.Sleep(1000);

                sender.Tell(new VideoStatus(message.UserId, message.VideoId, "stopped"));

                Watched.Add(message);
            });
        }
    }
}