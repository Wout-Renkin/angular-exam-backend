using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly NewsContext _context;

        public PostController(NewsContext context)
        {
            _context = context;
        }

        //Get a specific post
        //GET: api/post/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.Where(p => p.PostId == id).Include(u => u.User).Include(u => u.Likes).ThenInclude(u => u.User).Include(u => u.Comments).ThenInclude(u => u.User).FirstOrDefaultAsync();

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        //Get all posts from group in company
        //GET: api/post/company/{companyId}/group/{groupId}
        [Authorize]
        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByGroup(int groupId, [FromQuery] int pageSize = 10, int currentPage = 1)
        {
           
            return await _context.Posts
                .Include(u => u.User)
                .Include(u => u.Likes)
                .ThenInclude(u => u.User)
                .Include(u => u.Comments)
                .ThenInclude(u => u.User)
                .Where(g => g.Group.GroupId == groupId)
                .OrderByDescending(x => x.PostId)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        //Create a post
        //POST: api/post
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost([FromForm]Post post)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault()?.Value);
            var user = await _context.Users.Where(x => x.UserID == userId).FirstOrDefaultAsync();
            var groupUser = await _context.GroupUsers.Where(u => u.GroupId == post.GroupId).Where(u => u.UserId == user.UserID).FirstOrDefaultAsync();

            if (user.RoleId == 4 || user.RoleId == 3 || groupUser != null)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        post.ImagePath = "https://localhost:44348/" + dbPath;

                    }
                }

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPost", new { id = post.PostId }, post);

            } else
            {
                return Forbid();
            }



     
        }

        //Delete a post
        //DELETE: api/post/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault()?.Value);
            var user = await _context.Users.Where(x => x.UserID == userId).FirstOrDefaultAsync();
            var moderator = await _context.GroupUsers.Where(u => u.GroupId == post.GroupId).Where(u => u.UserId == user.UserID).Where(u => u.Moderator == true).FirstOrDefaultAsync();

            if (user.RoleId == 4 || user.RoleId == 3 || moderator != null || post.UserId == user.UserID)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return post;
            } else
            {
                return Forbid();
            }
         

            
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, [FromForm] Post post)
        {
            if (id != post.PostId)
            {
                return BadRequest();
            }

            var userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault()?.Value);
            var user = await _context.Users.Where(x => x.UserID == userId).FirstOrDefaultAsync();
            var moderator = await _context.GroupUsers.Where(u => u.GroupId == post.GroupId).Where(u => u.UserId == user.UserID).Where(u => u.Moderator == true).FirstOrDefaultAsync();

            if (user.RoleId == 4 || user.RoleId == 3 || moderator != null || post.UserId == user.UserID)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        post.ImagePath = "https://localhost:44348/" + dbPath;

                    }
                }

                _context.Entry(post).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(post);
            } else
            {
                return Forbid();
            }

              
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }



    }
}
