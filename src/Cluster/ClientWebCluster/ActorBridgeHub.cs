using System.Threading.Tasks;
using Actors.Messages;
using Actors.Models;
using Akka.Actor;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ActorRefs = ClientWebCluster.ActorRefs;

namespace ClientWebCluster
{
    [HubName("actorBridgeHub")]
    public class ActorBridgeHub : Hub
    {
        public ActorBridgeHub()
        {
            
        }

        public void Login()
        {
            ActorRefs.ApiActor.Tell(new LoginMessage(Context.ConnectionId, ActorRefs.SignalRActor));
        }

        public void Watch(int movieId)
        {
            ActorRefs.ApiActor.Tell(new WatchVideoEvent(Context.ConnectionId, movieId, ActorRefs.SignalRActor));
        }

        public void VideoResponse(Video[] videos)
        {
            
        }

        public override Task OnConnected()
        {
            // Set connection id for just connected client only
            return Clients.Client(Context.ConnectionId).SetConnectionId(Context.ConnectionId);
        }
    }
}
