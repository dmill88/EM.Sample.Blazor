using System.Threading.Tasks;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EM.Sample.DomainLogic;
using EM.Sample.DomainModels.Models;

namespace EM.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogQueries _blogQueries = null;
        private readonly IBlogCommands _blogCommands = null;
        private readonly ILogger<BlogPostsController> _logger;

        public BlogPostsController(IBlogQueries blogQueries, IBlogCommands blogCommands, ILogger<BlogPostsController> logger)
        {
            _blogQueries = blogQueries;
            _blogCommands = blogCommands;
            _logger = logger;
        }

        [HttpGet("[action]/{blogId}")]
        public IActionResult AddBlogPost(int blogId)
        {
            BlogDto blog = _blogQueries.GetBlog(blogId);
            BlogPostDto blogPost = new BlogPostDto();
            blogPost.BlogId = blogId;
            blogPost.BlogDisplayName = blog.DisplayName;
            blogPost.PostStatusId = (int)EM.Sample.DomainModels.Enums.PostStatuses.Draft;
            blogPost.CommentStatusId = (int)EM.Sample.DomainModels.Enums.CommentStatuses.MemberOnly;
            return Ok(blogPost);
        }

        [HttpPost("[action]")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBlogPost([FromBody] BlogPostDto post)
        {
            if (post == null)
            {
                return BadRequest("Blog object is null.");
            }
            if (post.Id != 0)
            {
                return BadRequest("Post Id is not zero.");
            }
            if (post.BlogId == 0)
            {
                return BadRequest("BlogId is zero.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                await _blogCommands.AddBlogPostAsync(post);
            }

            return CreatedAtAction(nameof(AddBlogPost), post);
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePost([FromBody] PostDto post)
        {
            if (post == null)
            {
                return BadRequest("Blog object is null.");
            }
            if (post.Id == 0)
            {
                return BadRequest("Post Id is zero.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                await _blogCommands.UpdatePostAsync(post);
            }

            return Ok(post);
        }

        [HttpDelete("[action]/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            if (id == 0)
            {
                return BadRequest("Blog Id is zero.");
            }
            await _blogCommands.DeletePostAsync(id);
            return Ok();
        }


    }
}