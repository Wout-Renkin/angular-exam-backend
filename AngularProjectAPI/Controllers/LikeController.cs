using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly NewsContext _context;

        public LikeController(NewsContext context)
        {
            _context = context;
        }

        //Get a specific like
        //GET: api/like/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Like>> GetLike(int id)
        {
            var like = await _context.Likes.FindAsync(id);

            if (like == null)
            {
                return NotFound();
            }

            return like;
        }

        //Create a like
        //POST: api/like
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Like>> PostLike(Like like)
        {
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLike", new { id = like.LikeId }, like);
        }

        //Delete a like
        //DELETE: api/like/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Like>> DeleteLike(int id)
        {
            var comment = await _context.Likes.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Likes.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        //Get all likes from a post
        //GET: api/like/post/{postId}
        [Authorize]
        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<Like>>> GetLikesByPost(int postId)
        {
            return await _context.Likes.Where(l => l.post.PostId == postId).ToListAsync();
        }



    }
}
