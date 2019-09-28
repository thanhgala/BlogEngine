using Refit;
using System;

namespace FrameworkCore.Infrastructure.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface)]
    public class WebApiAuthorizeAttribute : HeadersAttribute
    {
        public WebApiAuthorizeAttribute() : base("Authorization: Bearer")
        {

        }
    }
}
