using System.Data.Common;
using System.Runtime.CompilerServices;
using Gql.EmptyExample.Posts.Core.Domain;
using Gql.EmptyExample.Requests;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace Gql.EmptyExample.Gql
{
    public class PostSubscription
    {
        [Subscribe(With = nameof(OnPublishedStream))]
        //[Subscribe]
        //[Topic(nameof(PostMutation.PublishPost))]
        public Post OnPublishedPost(
            [EventMessage] Post publishedPost)
        {
            return publishedPost;
        }

        public async IAsyncEnumerable<Post> OnPublishedStream(
            [Service] ITopicEventReceiver eventReceiver, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var sourceStream =
                await eventReceiver.SubscribeAsync<string, Post>(nameof(PostMutation.PublishPost), cancellationToken);

            yield return new Post()
            {
                Title = "subscription Title",
                Abstraction = "this is an introduction post for graphql",
                Content = "some random content for post 1",
                Author = "subscription Author",
                PublishedAt = DateTime.Now.AddDays(-2),
                Link = "http://link-to-post-1.html",
                Comments = new List<Comment>
                {
                    new() { CreatedAt = DateTime.Now, Content = "test  comment 03 for post 1", Name = "kindUser02" }
                },
            };


            await Task.Delay(5000, cancellationToken);

            await foreach (Post post in sourceStream.ReadEventsAsync())
            {
                yield return post;
            }
        }


    }
}
