using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gql.EmptyExample.Posts.Core.Domain
{
    public class PostQuery
    {

        [UsePaging]
        [UseOffsetPaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        [Authorize(Policy = "AdminOnly")]
        public IExecutable<Post> GetPosts([Service] DbContext db)
        { 
            
            return db.Post.AsExecutable();

        }
    }
}
