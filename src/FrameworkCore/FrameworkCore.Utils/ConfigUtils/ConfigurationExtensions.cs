using Microsoft.Extensions.Configuration;

namespace FrameworkCore.Utils.ConfigUtils
{
    public static class ConfigurationExtensions
    {
        public static T GetSection<T>(this IConfiguration configuration, string key = null) where T : new()
        {
            return ConfigurationHelper.GetSection<T>(configuration, key);
        }

        /// <summary>
        ///     Get Value follow Priority: Key:[Machine Name] &gt; Key:[Environment] &gt; Key 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <param name="key">          </param>
        /// <returns></returns>
        public static T GetValueByEnv<T>(this IConfiguration configuration, string key)
        {
            return ConfigurationHelper.GetValueByEnv<T>(configuration, key);
        }
    }
}
