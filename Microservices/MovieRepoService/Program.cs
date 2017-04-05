using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace MovieRepoService
{
    class Program
    {
        static void Main(string[] args)
        {
            ActorSystem actorSystem = ActorSystem.Create("moviedb");

            Console.ReadLine();

            actorSystem.WhenTerminated.Wait();
        }
    }
}
