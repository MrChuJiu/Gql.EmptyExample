using Gql.EmptyExample.Requests;
using HotChocolate.Subscriptions;
using MongoDB.Driver;

namespace Gql.EmptyExample.Posts.Core.Domain
{
    public class PostMutation
    {
        public async Task<AddPostPayload> CreatePostAsync(
            [Service] DbContext db,
            AddPostInput input)
        {
            var entity = new Post()
            {
                Title = input.Title,
                Abstraction = "this is an introduction post for graphql",
                Content = "some random content for post 1",
                Author = input.Author,
                PublishedAt = DateTime.Now.AddDays(-2),
                Link = "http://link-to-post-1.html",
                Comments = new List<Comment>
                {
                    new() { CreatedAt = DateTime.Now, Content = "test  comment 03 for post 1", Name = "kindUser02" }
                },
            };

            await db.Post.InsertOneAsync(entity);

            return new AddPostPayload(entity);
        }

        public async Task<UpdatePostPayload> UpdatePostAsync(
            [Service] DbContext db,
            UpdatePostInput input)
        {

            var _cbf = Builders<Post>.Filter
                .Eq(restaurant => restaurant.Id, input.PostId);


            var _cbu = Builders<Post>.Update
                .Set(restaurant => restaurant.Title, input.Title)
                .Set(restaurant => restaurant.Author, input.Author);

            _ = await db.Post.UpdateOneAsync(_cbf, _cbu);

            return new UpdatePostPayload(await db.Post.Find(x => x.Id == input.PostId).FirstOrDefaultAsync());


        }


        public async Task<AddPostPayload> PublishPost(
            [Service] DbContext db,
            [Service]ITopicEventSender sender,
            AddPostInput input,
            CancellationToken cancellationToken)
        {
            var entity = new Post()
            {
                Title = input.Title,
                Abstraction = "this is an introduction post for graphql",
                Content = "some random content for post 1",
                Author = input.Author,
                PublishedAt = DateTime.Now.AddDays(-2),
                Link = "http://link-to-post-1.html",
                Comments = new List<Comment>
                {
                    new() { CreatedAt = DateTime.Now, Content = "test  comment 03 for post 1", Name = "kindUser02" }
                },
            };

            // await db.Post.InsertOneAsync(entity, cancellationToken: cancellationToken);

            await sender.SendAsync(nameof(PublishPost), entity, cancellationToken);

            return new AddPostPayload(entity);
        }


    }
}
