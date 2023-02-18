using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gql.EmptyExample.Posts.Core.Domain
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Content { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
    
        //public ObjectId PostId { get; set; }

        //public Post Post { get; set; }
    }
}
