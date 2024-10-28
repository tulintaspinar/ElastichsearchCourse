

using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;

namespace Elastichsearch.API.Extensions
{
    public static class ElasticsearchExt
    {
        private static string userName = string.Empty;
        private static string password = string.Empty;
        public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
        {
            userName = configuration.GetSection("Elastic")["Username"]!;
            password = configuration.GetSection("Elastic")["Password"]!;
            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!)).Authentication(new BasicAuthentication(userName!,password!));            
            var client = new ElasticsearchClient(settings);
            services.AddSingleton(client);
        }
    }
}
