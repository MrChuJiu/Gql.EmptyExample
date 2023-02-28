using System.Security.Claims;
using Gql.EmptyExample;
using Gql.EmptyExample.Gql;
using Gql.EmptyExample.Interceptor;
using Gql.EmptyExample.Middleware;
using Gql.EmptyExample.Posts.Core.Domain;
using GraphQL.Server.Ui.Voyager;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Execution;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Bson;
using Okta.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<DbContext>();

//builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
}).AddOktaWebApi(new OktaWebApiOptions()
{
    OktaDomain = "https://dev-63355328.okta.com",
    AuthorizationServerId = "default",
    Audience = "api://default",
});

builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("AtLeast21", policy =>
    //    policy.Requirements.Add(new MinimumAgeRequirement(21)));

    options.AddPolicy("HasCountry", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "http://schemas.microsoft.com/identity/claims/scope")));
});

builder.Services
    .AddGraphQLServer()
    .AddQueryType<PostQuery>()
    .AddMutationType<PostMutation>()
    .AddSubscriptionType<PostSubscription>()
    .AddAuthorization()
    // 接口响应时间不能大于1秒
    //.ModifyRequestOptions(t => t.ExecutionTimeout = TimeSpan.FromSeconds(1))
    //.UseExceptions()
    //.UseRequest<TimeoutMiddleware>()
    ////.UseTimeout()
    //.UseDocumentCache()
    //.UseDocumentParser()
    //.UseDocumentValidation()
    //.UseOperationCache()
    //.UseOperationComplexityAnalyzer()
    //.UseOperationResolver()
    //.UseOperationVariableCoercion()
    //.UseOperationExecution()
    //.AddHttpRequestInterceptor<LoggingInterceptor>()
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



// Add services to the container.

var app = builder.Build();

app.UseRouting();

app.UseHttpsRedirection();

app.UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions { GraphQLEndPoint = "/graphql" });

app.UseAuthentication();

app.UseAuthorization();

app.UseWebSockets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();

