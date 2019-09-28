using FrameworkCore.Utils.CheckUtils;
using FrameworkCore.Utils.EnvUtils;
using Microsoft.Extensions.Configuration;

namespace FrameworkCore.Utils.ConfigUtils
{
    public class ConfigurationHelper
    {
        public static T GetSection<T>(IConfiguration configuration, string key = null) where T : new()
        {
            var value = new T();

            key = string.IsNullOrWhiteSpace(key) ? typeof(T).Name : key;

            configuration.GetSection(key).Bind(value);

            return value;
        }

        public static T GetValueByEnv<T>(IConfiguration configuration, string key)
        {
            CheckHelper.CheckNullOrWhiteSpace(key, nameof(key));

            var value = configuration.GetValue<T>($"{key}:{EnvHelper.MachineName}");

            if (value != null)
            {
                return value;
            }

            var environmentName = !string.IsNullOrWhiteSpace(EnvHelper.CurrentEnvironment) ? EnvHelper.CurrentEnvironment : EnvHelper.EnvDevelopmentName;

            value = configuration.GetValue<T>($"{key}:{environmentName}");

            if (value != null)
            {
                return value;
            }

            value = configuration.GetValue<T>($"{key}");

            return value;
        }
    }
}
