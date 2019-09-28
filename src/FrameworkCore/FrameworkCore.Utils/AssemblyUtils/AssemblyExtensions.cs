using System.Reflection;

namespace FrameworkCore.Utils.AssemblyUtils
{
    public static class AssemblyExtensions
    {
        public static string GetDirectoryPath(this Assembly assembly)
        {
            return AssemblyHelper.GetDirectoryPath(assembly);
        }
    }
}