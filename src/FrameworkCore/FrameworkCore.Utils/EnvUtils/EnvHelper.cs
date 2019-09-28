using System;

namespace FrameworkCore.Utils.EnvUtils
{
    public static class EnvHelper
    {
        public const string AspNetCoreEnvironmentVariable = "ASPNETCORE_ENVIRONMENT";

        public static string CurrentEnvironment => Environment.GetEnvironmentVariable(AspNetCoreEnvironmentVariable);

        public static readonly string MachineName = Environment.MachineName;

        public const string EnvDevelopmentName = "Development";
        public const string EnvStagingName = "Staging";
        public const string EnvProductionName = "Production";

        public static bool IsDevelopment()
        {
            return Is(EnvDevelopmentName);
        }

        public static bool IsStaging()
        {
            return Is(EnvStagingName);
        }

        public static bool IsProduction()
        {
            return Is(EnvProductionName);
        }

        public static bool Is(string environment)
        {
            return string.Equals(CurrentEnvironment, environment, StringComparison.OrdinalIgnoreCase);
        }
    }
}