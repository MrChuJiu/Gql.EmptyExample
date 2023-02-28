using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Gql.EmptyExample.Posts.Core.Domain
{
    [Authorize(Policy = "HasCountry")]
    public class PostQuery
    {

        [UsePaging]
        [UseOffsetPaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        [Authorize]
        public IExecutable<Post> GetPosts([Service] DbContext db)
        { 
            
            return db.Post.AsExecutable();

        }
    }
}
