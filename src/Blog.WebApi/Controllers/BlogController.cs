using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Application.Seedwork.ServiceContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.WebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IBlogCategoryService _blogCategoryService;

        private IMediator _mediator;

        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

        public BlogController(IHttpContextAccessor httpContextAccessor, IBlogCategoryService blogCategoryService)
        {
            _httpContextAccessor = httpContextAccessor;
            _blogCategoryService = blogCategoryService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetToken()
        {
            var accessToken = _httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
            return new[] { accessToken };
        }

        [HttpGet]
        [Route("GetBlog")]
        public async Task<IActionResult> GetBlog()
        {
            //var data = await Mediator.Send(new GetAllBlogsQuery());
            var dataFromService = await _blogCategoryService.GetAll();
            return Ok(dataFromService);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803
        }
    }
}
