using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly NewsContext _context;

        public GroupController(NewsContext context)
        {
            _context = context;
        }

        //Get all groups from company
        //GET: api/company/{companyId}
        [Authorize]
        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups(int companyId)
        {
            return await _context.Groups.Where(c => c.Company.CompanyId == companyId).Include(x => x.Posts).ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return group;
        }

        // PUT: api/group/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id,[FromForm]Group group)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault()?.Value);
            var user = await _context.Users.Where(x => x.UserID == userId).FirstOrDefaultAsync();
            var moderator = await _context.GroupUsers.Where(u => u.GroupId == group.GroupId).Where(u => u.UserId == user.UserID).Where(u => u.Moderator == true).FirstOrDefaultAsync();
            if (user.RoleId == 4 || user.RoleId == 3 || moderator != null)

                if (id != group.GroupId)
            {
                return BadRequest();
            }

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
                    group.ImagePath = "https://localhost:44348/" + dbPath;

                }
            }



            _context.Entry(group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(group);
            /*  if (id != group.GroupId)
              {
                  return BadRequest();
              }

              _context.Entry(group).State = EntityState.Modified;

              try
              {
                  await _context.SaveChangesAsync();
              }
              catch (DbUpdateConcurrencyException)
              {
                  if (!GroupExists(id))
                  {
                      return NotFound();
                  }
                  else
                  {
                      throw;
                  }
              }

              return NoContent();*/
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(g => g.GroupId == id);
        }

        //Delete a group
        //DELETE: api/group/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Group>> DeleteGroup(int id)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault()?.Value);
            var user = await _context.Users.Where(x => x.UserID == userId).FirstOrDefaultAsync();
            if (user.RoleId == 4 || user.RoleId == 3)
            {
                var group = await _context.Groups.FindAsync(id);
                if (group == null)
                {
                    return NotFound();
                }

                _context.Posts.RemoveRange(_context.Posts.Where(x => x.GroupId == group.GroupId));
                _context.Groups.Remove(group);

                await _context.SaveChangesAsync();

                return group;
            } else
            {
                return Forbid();
            }
            
          
        }

        //Create a group
        //POST: api/group
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup([FromForm]Group group)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault()?.Value);
            var user = await _context.Users.Where(x => x.UserID == userId).FirstOrDefaultAsync();
            if(user.RoleId == 4 || user.RoleId == 3)
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
                    group.ImagePath = "https://localhost:44348/" + dbPath;
                    _context.Groups.Add(group);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetGroup", new { id = group.GroupId }, group);

                }
                else
                {
                    return BadRequest();
                }
            } else
            {
                return Forbid();
            }

    


            /*            _context.Groups.Add(group);
                        await _context.SaveChangesAsync();

                        return CreatedAtAction("GetGroup", new { id = group.GroupId }, group);*/
        }




    }
}
