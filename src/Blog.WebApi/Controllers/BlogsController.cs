using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Application.Seedwork.RequestDto.Categories;
using Blog.Application.Seedwork.ServiceContracts;
using Blog.Domain.Core.Events;
using Blog.Domain.Core.Notification;
using FrameworkCore.Web.ApiResponseWrapper.Extensions.Wrappers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Blog.WebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ApiBaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IBlogCategoryService _blogCategoryService;

        public BlogsController(IHttpContextAccessor httpContextAccessor, IBlogCategoryService blogCategoryService,
            INotificationHandler<DomainNotification> notifications,
            IEventDispatcher eventDispatcher) 
            : base(notifications, eventDispatcher)
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
            var dataFromService = await _blogCategoryService.GetAll();
            return ResponseApi(new ApiResponse(dataFromService));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryRequestDto request)
        {
            await _blogCategoryService.Create(request);
            return ResponseApi();
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
