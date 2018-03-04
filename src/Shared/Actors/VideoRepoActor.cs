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
                Console.WriteLine($"History of videos for {request.ResponseStartRecommendation.UserId} that watched before has come, requesting for recommended videos.");
                Console.ResetColor();

                Thread.Sleep(50);

                Video[] unseenVideos = _movieRepository.GetUnseenVideos(request.Videos ?? new int[0]);

                actorRef.Tell(new UserUnwatchedVideoResponse(startRecommendation, unseenVideos));
            });
        }
    }
}