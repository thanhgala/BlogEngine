using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.WebApp.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Blog.WebApp.Controllers
{
    public class BlogAppMvcBaseController : Controller
    {

        public BlogAppMvcBaseController()
        {
        }

        public static string GetMsalAccountId(ClaimsPrincipal claimsPrincipal)
        {
            var userObjectId = claimsPrincipal.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
            if (string.IsNullOrEmpty(userObjectId))
            {
                userObjectId = claimsPrincipal.FindFirstValue("oid");
            }
            var tenantId = claimsPrincipal.FindFirstValue("http://schemas.microsoft.com/identity/claims/tenantid");
            if (string.IsNullOrEmpty(tenantId))
            {
                tenantId = claimsPrincipal.FindFirstValue("tid");
            }

            if (string.IsNullOrWhiteSpace(userObjectId))
                throw new ArgumentOutOfRangeException("Missing claim 'http://schemas.microsoft.com/identity/claims/objectidentifier' or 'oid' ");


            if (string.IsNullOrWhiteSpace(tenantId))
                throw new ArgumentOutOfRangeException("Missing claim 'http://schemas.microsoft.com/identity/claims/tenantid' or 'tid' ");

            var accountId = userObjectId + "." + tenantId;
            return accountId;
        }

        public void Test()
        {

        }
    }
}