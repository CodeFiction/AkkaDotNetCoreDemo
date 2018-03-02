using System;
using Actors;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClientWebNonCluster
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider AppApplicationServices { get; set; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSignalR(options => options.Hubs.EnableDetailedErrors = true);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR();

            Config hoconConfiguration = GetHoconConfiguration();
            ActorSystem actorSystem = ActorSystem.Create("moviedb", hoconConfiguration);

            IActorRef watchedVideo = actorSystem.ActorOf(Props.Create<WatchedVideoRepoActor>().WithRouter(FromConfig.Instance), "watchedVideo");
            IActorRef videoRepo = actorSystem.ActorOf(Props.Create<VideoRepoActor>().WithRouter(FromConfig.Instance), "videoRepo");
            ActorRefs.SignalRActor = actorSystem.ActorOf(Props.Create<SignalRActor>(), "signalRActor");

            ActorRefs.ApiActor = actorSystem.ActorOf(Props.Create<ApiActor>(watchedVideo, videoRepo), "api");

            AppApplicationServices = app.ApplicationServices;

            ActorRefs.ConnectionManager = AppApplicationServices.GetService<IConnectionManager>();
        }

        public static Config GetHoconConfiguration()
        {
            string config = @"					akka {
						actor {
										 deployment {
												/videoRepo {
													 router = round-robin-pool
													 nr-of-instances = 2
												}
												
												/watchedVideo {
													 router = round-robin-pool
													 nr-of-instances = 1
												}

												/api/recommandation {
													 router = round-robin-pool
													 nr-of-instances = 4
												}
									}
							 }";

            return ConfigurationFactory.ParseString(config);
        }
    }
}
