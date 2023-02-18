using System.Reflection;
using HotChocolate.Types.Descriptors;

namespace Gql.EmptyExample.Middleware
{
    public sealed class UseToUpperAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.UseToUpper();
        }
    }
}
