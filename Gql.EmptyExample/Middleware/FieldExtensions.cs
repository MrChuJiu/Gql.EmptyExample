namespace Gql.EmptyExample.Middleware
{
    public static class FieldExtensions
    {
        public static IObjectFieldDescriptor UseToUpper(this IObjectFieldDescriptor descriptor)
            => descriptor.Use(next => async context =>
            {
                await next(context);

                if (context.Result is string s)
                {
                    context.Result = s.ToUpperInvariant();
                }
            });
    }
}
