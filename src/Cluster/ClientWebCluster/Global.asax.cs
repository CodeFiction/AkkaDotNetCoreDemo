using System.Web.Mvc;
using System.Web.Routing;
using Akka.Actor;
using Akka.Routing;

namespace ClientWebCluster
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ActorSystem actorSystem = ActorSystem.Create("moviedb");

            ActorRefs.ApiActor = actorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "api");
            ActorRefs.SignalRActor = actorSystem.ActorOf(Props.Create<SignalRActor>(), "signalRActor");
        }
    }
}
