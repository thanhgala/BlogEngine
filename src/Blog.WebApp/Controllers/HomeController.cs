using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using FrameworkCore.Web.AzureIdentity.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITokenAcquisition = FrameworkCore.Web.AzureIdentity.Client.ITokenAcquisition;

namespace Blog.WebApp.Controllers
{
    public class HomeController : BlogAppMvcBaseController
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        public HomeController(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
        }

        [MsalUiRequiredExceptionFilter(Scopes = new[] { "User.Read" })]
        public async Task<IActionResult> Index()
        {
            var identity = (ClaimsIdentity)User.Identity;

            var scopes = new[] {"api://3f9e839e-97c4-493e-8e2e-47169fc1892e/access-api"};

            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            var accessToken = await _tokenAcquisition.GetAccessTokenOnBehalfOfUserAsync(scopes);

            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim Type: {claim.Type} - Claim Value: {claim.Value}");
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task SignOut()
        {
            await Task.WhenAll(HttpContext.SignOutAsync(AzureADDefaults.AuthenticationScheme),
                HttpContext.SignOutAsync(AzureADDefaults.OpenIdScheme));
        }
    }
}