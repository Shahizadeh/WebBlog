using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Models.Blog;
using WebBlog.Services.Blog;

namespace WebBlog.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ICommentService CommentService { get; }
        public CommentController(ICommentService commentService) 
        {
            CommentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCommentModel request)
        {
            var result = await CommentService.Add(request);
            if(result.Success)
            {
                var comments = await CommentService.GetByPostId(request.PostId);
                return new JsonResult( new
                {
                    Success= true,
                    Comments= comments  
                });
            }
            return new JsonResult(result);
        }
    }
}
