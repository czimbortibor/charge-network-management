using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.WebSockets;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string hostname = configuration["GremlinEndpointHost"];
            int port = int.Parse(configuration["GremlinEndpointPort"]);
            string containerLink = configuration["CosmosDbContainerLink"];
            string password = configuration["CosmosDbPrimaryKey"];

            var gremlinServer = new GremlinServer(hostname, port, enableSsl: true, containerLink, password);

            var connectionPoolSettings = new ConnectionPoolSettings()
            {
                MaxInProcessPerConnection = 10,
                PoolSize = 30,
                ReconnectionAttempts = 3,
                ReconnectionBaseDelay = TimeSpan.FromMilliseconds(500)
            };

            var webSocketConfiguration = new Action<ClientWebSocketOptions>(options =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(10);
            });

            var gremlinClient = new GremlinClient(
                                    gremlinServer,
                                    new GraphSON2Reader(),
                                    new GraphSON2Writer(),
                                    GremlinClient.GraphSON2MimeType,
                                    connectionPoolSettings,
                                    webSocketConfiguration);

            services.AddSingleton(gremlinClient);

            return services;
        }
    }
}
