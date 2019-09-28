using Microsoft.AspNetCore.Mvc;

namespace Blog.WebApp.Controllers
{
    public class ErrorController : Controller
    { 
        public IActionResult AccessDenied()
        {            
            return View();
        }
    }
}