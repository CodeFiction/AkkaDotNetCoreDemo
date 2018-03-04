// Copyright 2014-2015 Aaron Stannard, Petabridge LLC
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

using System.IO;
using System.Linq;
using Akka.Actor;
using Akka.Configuration;
using ConfigurationException = Akka.Configuration.ConfigurationException;

namespace Lighthouse
{
    /// <summary>
    /// Launcher for the Lighthouse <see cref="ActorSystem"/>
    /// </summary>
    public static class LighthouseHostFactory
    {
        public static ActorSystem LaunchLighthouse(string ipAddress = null, int? specifiedPort = null)
        {
            var systemName = "lighthouse";
            var clusterConfig = ConfigurationFactory.ParseString(File.ReadAllText("akka-config.hocon"));

            systemName = clusterConfig.GetString("lighthouse.actorsystem", systemName);

            var remoteConfig = clusterConfig.GetConfig("akka.remote");
            ipAddress = ipAddress ??
                        remoteConfig.GetString("dot-netty.tcp.public-hostname") ??
                        "127.0.0.1"; //localhost as a final default
            int port = specifiedPort ?? remoteConfig.GetInt("dot-netty.tcp.port");

            if (port == 0)
            {
                throw new ConfigurationException("Need to specify an explicit port for Lighthouse. Found an undefined port or a port value of 0 in App.config.");
            }

            var selfAddress = $"akka.tcp://{systemName}@{ipAddress}:{port}";
            var seeds = clusterConfig.GetStringList("akka.cluster.seed-nodes");
            if (!seeds.Contains(selfAddress))
            {
                seeds.Add(selfAddress);
            }

            var injectedClusterConfigString = seeds.Aggregate("akka.cluster.seed-nodes = [", (current, seed) => current + (@"""" + seed + @""", "));
            injectedClusterConfigString += "]";


            var finalConfig = ConfigurationFactory.ParseString($"akka.remote.dot-netty.tcp.public-hostname = {ipAddress}\r\nakka.remote.dot-netty.tcp.port = {port}")
                .WithFallback(ConfigurationFactory.ParseString(injectedClusterConfigString))
                .WithFallback(clusterConfig);

            return ActorSystem.Create(systemName, finalConfig);
        }
    }
}
