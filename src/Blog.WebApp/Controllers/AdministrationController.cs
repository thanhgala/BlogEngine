using System.Threading.Tasks;
using Blog.WebApp.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Blog.WebApp.Controllers
{
    [Authorize]
    public class AdministrationController : BlogAppMvcBaseController
    {
        public AdministrationController()
        {
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}