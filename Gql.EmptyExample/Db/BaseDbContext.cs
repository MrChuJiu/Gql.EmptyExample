using Gql.EmptyExample.Posts.Core.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Gql.EmptyExample
{
    public class BaseDbContext
    {

        public BaseDbContext(IOptions<DatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoConnectionUrl = new MongoUrl(bookStoreDatabaseSettings.Value.ConnectionString);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
            mongoClientSettings.ClusterConfigurator = cb => {
                cb.Subscribe<CommandStartedEvent>(e => {
                    Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
                });
            };


            Client = new MongoClient(
                mongoClientSettings);

            Database = Client.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);
        }

        /// <summary>
        /// MongoClient
        /// </summary>
        public IMongoClient Client { get; private set; } = default!;
        /// <summary>
        /// 获取链接字符串或者HoyoMongoSettings中配置的特定名称数据库或默认数据库hoyo
        /// </summary>
        public IMongoDatabase Database { get; private set; } = default!;
    }
}
