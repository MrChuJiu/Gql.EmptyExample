using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Gql.EmptyExample.Posts.Core.Domain
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Abstraction { get; set; } 
        public string Content { get; set; }
        public string Link { get; set; }
        public DateTime PublishedAt { get; set; }
        public IReadOnlyList<Comment> Comments { get; init; }
        public async Task<bool> Wait(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);
            return true;
        }


    }
}
