using Akka.Actor;
using Microsoft.AspNetCore.SignalR;

namespace ClientWebNonCluster
{
    public static class ActorReferences
    {
        public static IActorRef ApiActor { get; set; }

        public static IActorRef SignalRActor { get; set; }

        public static IHubContext<ActorBridgeHub> ActorBridgeHubContext { get; set; }
    }
}
