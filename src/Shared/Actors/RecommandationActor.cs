﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Actors.Messages;
using Actors.Models;
using Akka.Actor;
using Akka.Routing;
using Newtonsoft.Json;

namespace Actors
{
    public class RecommandationActor : ReceiveActor, IWithUnboundedStash
    {
        private readonly IActorRef _watchedVideoActor;
        private readonly IActorRef _videoRepoActor;

        private ICancelable _startAttempts;

        public IStash Stash { get; set; }

        public RecommandationActor(IActorRef watchedVideoActor, IActorRef videoRepoActor)
        {
            _watchedVideoActor = watchedVideoActor;
            _videoRepoActor = videoRepoActor;

            StartTakeRecommandationRequest();
        }

        private void StopTakeRecommandationRequest()
        {
            HandleCheckRecommendationSystemAvailable();

            Receive<StartRecommendation>(recommendation =>
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Video recommendation is not available, please try again later.");
                Console.ResetColor();

                Stash.Stash();
            });
        }

        private void StartTakeRecommandationRequest()
        {
            Stash?.UnstashAll();

            HandleCheckRecommendationSystemAvailable();

            Receive<StartRecommendation>(recommendation =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Requesting for previously watched videos for {recommendation.UserId}");
                Console.ResetColor();

                Thread.Sleep(50);

                _watchedVideoActor.Tell(new UserWatchHistoryRequest(recommendation));
            });

            Receive<UserWatchHistoryResponse>(response =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"The request of videos for {response.StartRecommendation.UserId} that watched before has come, asking for recommended videos.");
                Console.ResetColor();

                int[] watchedVideos = response.VideoIds;

                Thread.Sleep(50);

                _videoRepoActor.Tell(new UserUnwatchedVideoRequest(response.StartRecommendation, watchedVideos));
            });

            Receive<UserUnwatchedVideoResponse>(response =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"The request of recommended videos for {response.Recommendation.UserId} has come.");
                Console.ResetColor();

                // Issue about interoperability between .NET Full and .NET Core versions
                // https://github.com/akkadotnet/akka.net/issues/3226
                Video[] responseVideos = response.Videos;
                var responseVideoJsonPaylod = JsonConvert.SerializeObject(responseVideos);

                Thread.Sleep(50);

                IActorRef sender = response.Recommendation.Client;

                sender.Tell(new RecommendationResponse(response.Recommendation.UserId, responseVideoJsonPaylod));

                // Self.Tell(PoisonPill.Instance); // Özel tipte bir mesaj, actor'ün kendini yok etmesini sağlıyor. Bu adımdan itibaren actor'le işimiz kalmıyor.
            });
        }

        private void HandleCheckRecommendationSystemAvailable()
        {
            ReceiveAsync<CheckRecommendationSystemAvailable>(async attempt =>
            {
                Task<Routees> watchedVideoRoutees = _watchedVideoActor.Ask<Routees>(new GetRoutees());
                Task<Routees> videoRoutees = _videoRepoActor.Ask<Routees>(new GetRoutees());

                Routees[] routeeses = await Task.WhenAll(watchedVideoRoutees, videoRoutees);

                bool systemHealty = routeeses.All(routees => routees.Members.Any());

                if (systemHealty)
                {
                    Become(StartTakeRecommandationRequest);
                }
                else
                {
                    Become(StopTakeRecommandationRequest);
                }
            });
        }

        protected override void PreStart()
        {
            _startAttempts = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.Zero, TimeSpan.FromSeconds(5), Self, new CheckRecommendationSystemAvailable(), ActorRefs.NoSender);
        }

        protected override void PostStop()
        {
            _startAttempts.Cancel();
        }
    }
}