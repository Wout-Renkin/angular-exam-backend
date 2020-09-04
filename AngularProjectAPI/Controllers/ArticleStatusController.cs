using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularProjectAPI.Data;
using AngularProjectAPI.Models;

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleStatusController : ControllerBase
    {
        private readonly NewsContext _context;

        public ArticleStatusController(NewsContext context)
        {
            _context = context;
        }

        // GET: api/ArticleStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleStatus>>> GetArticleStatuses()
        {
            return await _context.ArticleStatuses.ToListAsync();
        }

        // GET: api/ArticleStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleStatus>> GetArticleStatus(int id)
        {
            var articleStatus = await _context.ArticleStatuses.FindAsync(id);

            if (articleStatus == null)
            {
                return NotFound();
            }

            return articleStatus;
        }

        // PUT: api/ArticleStatus/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticleStatus(int id, ArticleStatus articleStatus)
        {
            if (id != articleStatus.ArticleStatusID)
            {
                return BadRequest();
            }

            _context.Entry(articleStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleStatusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ArticleStatus
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ArticleStatus>> PostArticleStatus(ArticleStatus articleStatus)
        {
            _context.ArticleStatuses.Add(articleStatus);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ArticleStatusExists(articleStatus.ArticleStatusID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetArticleStatus", new { id = articleStatus.ArticleStatusID }, articleStatus);
        }

        // DELETE: api/ArticleStatus/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ArticleStatus>> DeleteArticleStatus(int id)
        {
            var articleStatus = await _context.ArticleStatuses.FindAsync(id);
            if (articleStatus == null)
            {
                return NotFound();
            }

            _context.ArticleStatuses.Remove(articleStatus);
            await _context.SaveChangesAsync();

            return articleStatus;
        }

        private bool ArticleStatusExists(int id)
        {
            return _context.ArticleStatuses.Any(e => e.ArticleStatusID == id);
        }
    }
}
