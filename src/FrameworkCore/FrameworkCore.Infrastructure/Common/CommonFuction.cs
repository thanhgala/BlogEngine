using System.Linq;
using System.Reflection;

namespace FrameworkCore.Infrastructure.Common
{
    public static class CommonFuction
    {
        public static bool TryGetAttribute<T>(MemberInfo memberInfo, out T customAttribute) where T : System.Attribute
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            if (attributes == null)
            {
                customAttribute = null;
                return false;
            }
            customAttribute = (T)attributes;
            return true;
        }
    }
}
