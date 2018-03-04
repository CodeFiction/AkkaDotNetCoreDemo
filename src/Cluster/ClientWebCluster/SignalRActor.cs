using System.Collections.Generic;
using System.Linq;
using Actors.Messages;
using Actors.Models;
using Akka.Actor;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace ClientWebCluster
{
    public class SignalRActor : ReceiveActor
    {
        private IHubContext _hubContext;

        public SignalRActor()
        {
            Receive<RecommendationResponse>(response =>
            {
                // Issue about interoperability between .NET Full and .NET Core versions
                // https://github.com/akkadotnet/akka.net/issues/3226
                var videos = JsonConvert.DeserializeObject<List<Video>>(response.ResponseVideosJsonPaylod);

                _hubContext.Clients.Client(response.UserId).videoResponse(videos.OrderByDescending(video => video.Id).ToArray());
            });

            Receive<VideoStatus>(response =>
            {
                _hubContext.Clients.Client(response.UserId).videoStatus(response);
            });
        }

        protected override void PreStart()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<ActorBridgeHub>();
        }
    }
}
