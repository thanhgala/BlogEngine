using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;

namespace Blog.WebApp.Configs
{
    public class AzureConfiguration : AzureADOptions
    {
        public static AzureConfiguration Current;

        public AzureConfiguration() => Current = this;
    }
}
