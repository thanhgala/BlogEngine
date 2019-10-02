using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebApp.Controllers
{
    [Authorize]
    public class AdministrationController : BlogAppMvcBaseController
    {
        public AdministrationController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}