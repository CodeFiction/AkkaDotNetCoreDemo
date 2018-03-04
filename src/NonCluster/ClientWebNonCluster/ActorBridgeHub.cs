using System.Threading.Tasks;
using Actors.Messages;
using Actors.Models;
using Microsoft.AspNetCore.SignalR;

using Akka.Actor;

namespace ClientWebNonCluster
{
    public class ActorBridgeHub : Hub
    {
        public ActorBridgeHub()
        {           
        }

        public void Login()
        {
            ActorReferences.ApiActor.Tell(new LoginMessage(Context.ConnectionId, ActorReferences.SignalRActor));
        }

        public void Watch(int movieId)
        {
            ActorReferences.ApiActor.Tell(new WatchVideoEvent(Context.ConnectionId, movieId, ActorReferences.SignalRActor));
        }

        public void VideoResponse(Video[] videos)
        {           
        }

        public override Task OnConnectedAsync()
        {
            // Set connection id for just connected client only
            return Clients.Client(Context.ConnectionId).InvokeAsync("SetConnectionId", Context.ConnectionId);
        }
    }
}
