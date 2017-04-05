using System.Threading.Tasks;
using Actors.Messages;
using Actors.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;

using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;

namespace ClientWebNonCluster
{
    [HubName("actorBridgeHub")]
    public class ActorBridgeHub : Hub
    {
        public ActorBridgeHub()
        {
            
        }

        public void Login()
        {
            ActorRefs.ApiActor.Tell(new LoginMessage(Context.ConnectionId), ActorRefs.SignalRActor);
        }

        public void Watch(int movieId)
        {
            ActorRefs.ApiActor.Tell(new WatchedVideoEvent(Context.ConnectionId, movieId), ActorRefs.SignalRActor);
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
