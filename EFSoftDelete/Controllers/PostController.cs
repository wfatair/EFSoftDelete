using EFSoftDelete.Contracts.Post;
using EFSoftDelete.Data;
using EFSoftDelete.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFSoftDelete.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly BlogDbContext _context;

        //Should use Dependency Injection to implement Db transactions
        public PostController(BlogDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> Get()
        {
            return Ok(await _context.Posts.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> Get(int id)
        {
            return Ok(await _context.Posts.FindAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreatePostRequest postRequest)
        {
            var newCreatedPost = new Post { Title = postRequest.Title };
            var result = await _context.Posts.AddAsync(newCreatedPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = newCreatedPost.Id }, newCreatedPost);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return Ok("Post Deleted");
            }
            else
                return NotFound();
        }
    }
}
