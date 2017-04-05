using System.Linq;
using Actors.Messages;
using Akka.Actor;
using Microsoft.AspNet.SignalR;

namespace ClientWebCluster
{
    public class SignalRActor : ReceiveActor
    {
        private IHubContext _hubContext;

        public SignalRActor()
        {
            Receive<RecommendationResponse>(response =>
            {
                _hubContext.Clients.Client(response.UserId).videoResponse(response.ResponseVideos.OrderByDescending(video => video.Id).ToArray());
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
