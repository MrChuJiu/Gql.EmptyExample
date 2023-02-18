using HotChocolate.Execution;
using HotChocolate.Execution.Options;
using HotChocolate.Execution.Processing;
using RequestDelegate = Microsoft.AspNetCore.Http.RequestDelegate;


namespace Gql.EmptyExample.Middleware
{
    public class TimeoutMiddleware
    {   
        
        private readonly HotChocolate.Execution.RequestDelegate _next;
        private readonly TimeSpan _standardTimeout;

        private static readonly TimeSpan _extendedTimeout = TimeSpan.FromSeconds(30);
        private readonly HashSet<string> _specialQueries = new() { "9n/8UkfQWTolxOBgUPZ2oA1==" };

        public TimeoutMiddleware(
            HotChocolate.Execution.RequestDelegate next,
            IRequestExecutorOptionsAccessor options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            _next = next ?? throw new ArgumentNullException("next");
            _standardTimeout = options.ExecutionTimeout;
        }

        public async ValueTask InvokeAsync(IRequestContext context)
        {
            //if (Debugger.IsAttached)
            //{
            //    await _next(context).ConfigureAwait(continueOnCapturedContext: false);
            //    return;
            //}

            Console.WriteLine(context.Request.QueryHash);

            using CancellationTokenSource timeout = new CancellationTokenSource(context.Request.QueryHash is not null &&
                _specialQueries.Contains(context.Request.QueryHash)
                    ? _extendedTimeout
                    : _standardTimeout);

            CancellationTokenSource combined = CancellationTokenSource.CreateLinkedTokenSource(context.RequestAborted, timeout.Token);

            try
            {
                context.RequestAborted = combined.Token;
                await _next(context).ConfigureAwait(continueOnCapturedContext: false);
                if (timeout.IsCancellationRequested)
                {
                    context.Result = QueryResultBuilder.CreateError(
                        new Error(string.Format("Timed out {0}.", _standardTimeout), "HC0045"));
                }
            }
            catch (OperationCanceledException)
            {
                if (!timeout.IsCancellationRequested)
                {
                    throw;
                }
                context.Result = QueryResultBuilder.CreateError(
                    new Error(string.Format("Timed out {0}.", _standardTimeout), "HC0045"));
            }
            finally
            {
                IResponseStream responseStream = context.Result as IResponseStream;
                if (responseStream != null)
                { 
                    responseStream.RegisterDisposable(combined);
                }
                else
                {
                    combined.Dispose();
                }
            }


        }
    }
}
