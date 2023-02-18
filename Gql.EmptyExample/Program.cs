using Gql.EmptyExample;
using Gql.EmptyExample.Gql;
using Gql.EmptyExample.Interceptor;
using Gql.EmptyExample.Middleware;
using Gql.EmptyExample.Posts.Core.Domain;
using GraphQL.Server.Ui.Voyager;
using HotChocolate.Execution;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<DbContext>();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<PostQuery>()
    .AddMutationType<PostMutation>()
    .AddSubscriptionType<PostSubscription>()
    // 接口响应时间不能大于1秒
    .ModifyRequestOptions(t => t.ExecutionTimeout = TimeSpan.FromSeconds(1))
    .UseExceptions()
    .UseRequest<TimeoutMiddleware>()
    //.UseTimeout()
    .UseDocumentCache()
    .UseDocumentParser()
    .UseDocumentValidation()
    .UseOperationCache()
    .UseOperationComplexityAnalyzer()
    .UseOperationResolver()
    .UseOperationVariableCoercion()
    .UseOperationExecution()
    .AddHttpRequestInterceptor<LoggingInterceptor>()
    .AddInMemorySubscriptions()
    .AddMongoDbFiltering()
    .AddMongoDbSorting()
    .AddMongoDbProjections()
    .AddMongoDbPagingProviders()
    .SetPagingOptions(new PagingOptions
    {
        MaxPageSize = 50,
        IncludeTotalCount = true
    });


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:7145"; // IdentityServer地址
        options.Audience = "graphql_api"; // 与IdentityServer中客户端的ClientId一致
    });





// Add services to the container.

var app = builder.Build();

app.UseHttpsRedirection();

app.UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions { GraphQLEndPoint = "/graphql" });

app.UseAuthentication();

app.UseAuthorization();

app.UseWebSockets();

app.UseRouting().UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});


app.Run();

