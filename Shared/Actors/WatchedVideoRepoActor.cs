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
        private static readonly IList<WatchedVideoEvent> Watched = new List<WatchedVideoEvent>();

        public WatchedVideoRepoActor()
        {             
            Receive<UserWatchedVideoRequest>(request =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{request.AttemptRecommendation.UserId} icin onceden izlemis oldugu video'lar bulundu, cevap dönülüyor.");
                Console.ResetColor();

                int[] videoIds = Watched.Where(videos => videos.UserId == request.AttemptRecommendation.UserId)
                    .Select(videos => videos.VideoId)
                    .Distinct()
                    .ToArray();

                Sender.Tell(new UserWatchedVideoResponse(request.AttemptRecommendation, videoIds));
            });

            Receive<WatchedVideoEvent>(message =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{message.UserId} {message.VideoId}'li video'yu izlemeye basladi");
                Console.ResetColor();

                Sender.Tell(new VideoStatus(message.UserId, message.VideoId, "watching"));

                Thread.Sleep(2000);

                Sender.Tell(new VideoStatus(message.UserId, message.VideoId, "stopped"));

                Watched.Add(message);
            });
        }
    }
}