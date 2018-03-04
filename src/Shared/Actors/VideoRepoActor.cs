using System;
using System.Threading;
using Actors.ExternalApi;
using Actors.Messages;
using Actors.Models;
using Akka.Actor;

namespace Actors
{
    public class VideoRepoActor : ReceiveActor
    {
        private readonly MovieRepository _movieRepository;

        public VideoRepoActor()
        {
            _movieRepository = new MovieRepository();

            Receive<UserUnwatchedVideoRequest>(request =>
            {
                IActorRef actorRef = Sender;
                StartRecommendation startRecommendation = request.ResponseStartRecommendation;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{request.ResponseStartRecommendation.UserId} icin onceden izlemis oldugu video'ların bilgisi geldi, tavsiye edilecek video'lar getiriliyor");
                Console.ResetColor();

                Thread.Sleep(50);

                Video[] unseenVideos = _movieRepository.GetUnseenVideos(request.Videos ?? new int[0]);

                actorRef.Tell(new UserUnwatchedVideoResponse(startRecommendation, unseenVideos));
            });
        }
    }
}