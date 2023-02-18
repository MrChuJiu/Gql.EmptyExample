using Gql.EmptyExample.Posts.Core.Domain;
using Gql.EmptyExample.Requests;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Gql.EmptyExample
{
    public class DbContext : BaseDbContext
    {
        /// <summary>
        ///
        /// </summary>
        public IMongoCollection<Post> Post => Database.GetCollection<Post>("post");

        public IMongoCollection<Comment> Comments => Database.GetCollection<Comment>("Comment");

        public DbContext(IOptions<DatabaseSettings> bookStoreDatabaseSettings) : base(bookStoreDatabaseSettings)
        {
          
        }
    }
}
