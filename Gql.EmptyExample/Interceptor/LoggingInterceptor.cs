using System.Security.Claims;
using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using HotChocolate.Resolvers;

namespace Gql.EmptyExample.Interceptor
{
    public class LoggingInterceptor : DefaultHttpRequestInterceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;
        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }
        public override ValueTask OnCreateAsync(
            HttpContext context,
            IRequestExecutor requestExecutor,
            IQueryRequestBuilder requestBuilder,
            CancellationToken cancellationToken)
        {

            var request = requestBuilder.Create();
            _logger.LogInformation("Executing GraphQL request {OperationName}: {Query}", request.OperationName, request.Query);

            requestBuilder.TryAddProperty("currentUserId", 69);

            return base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
        }
    }
}
