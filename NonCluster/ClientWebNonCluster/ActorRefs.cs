using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.SignalR.Infrastructure;

namespace ClientWebNonCluster
{
    public static class ActorRefs
    {
        public static IActorRef ApiActor { get; set; }

        public static IActorRef SignalRActor { get; set; }

        public static IConnectionManager ConnectionManager { get; set; }
    }
}
