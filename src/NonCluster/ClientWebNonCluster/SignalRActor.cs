using System.Collections.Generic;
using System.Linq;
using Actors.Messages;
using Actors.Models;
using Akka.Actor;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ClientWebNonCluster
{
    public class SignalRActor : ReceiveActor
    {
        private IHubContext<ActorBridgeHub> _hubContext;

        public SignalRActor()
        {
            Receive<RecommendationResponse>(response =>
            {
                string responseResponseVideosJsonPaylod = response.ResponseVideosJsonPaylod;
                List<Video> responseVideos = JsonConvert.DeserializeObject<List<Video>>(responseResponseVideosJsonPaylod);

                _hubContext.Clients.Client(response.UserId).InvokeAsync("videoResponse", responseVideos.OrderByDescending(video => video.Id).ToList());
            });

            Receive<VideoStatus>(response =>
            {
                _hubContext.Clients.Client(response.UserId).InvokeAsync("videoStatus", response);
            });
        }

        protected override void PreStart()
        {
            _hubContext = ActorReferences.ActorBridgeHubContext;
        }
    }
}
