using Microsoft.AspNetCore.Authentication.AzureAD.UI;

namespace Blog.WebApp.Configs
{
    public class AzureConfiguration : AzureADOptions
    {
        public static AzureConfiguration Current;

        public AzureConfiguration() => Current = this;
    }
}
