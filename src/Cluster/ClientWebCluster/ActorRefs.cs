using Akka.Actor;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace ClientWebCluster
{
    public static class ActorRefs
    {
        public static IActorRef ApiActor { get; set; }

        public static IActorRef SignalRActor { get; set; }

        public static IConnectionManager ConnectionManager { get; set; }
    }
}
