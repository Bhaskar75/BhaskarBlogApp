using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BhaskarBlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("This is the GET Images API call");
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            //call a repository
            return Ok();
        }
    }
}
