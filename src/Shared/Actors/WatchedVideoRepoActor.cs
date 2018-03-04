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
            Receive<UserWatchedVideoRequest>(request =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{request.Recommendation.UserId} icin onceden izlemis oldugu video'lar bulundu, cevap dönülüyor.");
                Console.ResetColor();

                Thread.Sleep(50);

                int[] videoIds = Watched.Where(videos => videos.UserId == request.Recommendation.UserId)
                    .Select(videos => videos.VideoId)
                    .Distinct()
                    .ToArray();

                Sender.Tell(new UserWatchedVideoResponse(request.Recommendation, videoIds));
            });

            Receive<WatchVideoEvent>(message =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{message.UserId} {message.VideoId}'li video bulundu, izlenmeye başlıyor.");
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