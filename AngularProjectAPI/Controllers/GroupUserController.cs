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
    public class GroupUserController : ControllerBase
    {
        private readonly NewsContext _context;

        public GroupUserController(NewsContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("{groupId}/{companyId}")]
        public async Task<ActionResult<IEnumerable<GroupUser>>> GetUsersByGroup(int groupId, int companyId, [FromQuery] int pageSize = 10, int currentPage = 1, string filter = null, Boolean requestedModerator = false, Boolean notInGroup = false, Boolean moderator = false, Boolean groupRequest = false)
        {
            IQueryable<GroupUser> users;

            if (notInGroup)
            {
                IQueryable<User> companyUsers;
                IQueryable<User> groupUsers;

                companyUsers = _context.Users.Where(c => c.CompanyId == companyId);
                groupUsers = _context.GroupUsers.Where(c => c.GroupId == groupId).Select(u => u.User);
                companyUsers = companyUsers.Except(groupUsers);


                if (filter != null && filter != "null")
                {

                    companyUsers = companyUsers.Where(u => u.Email.Contains(filter) || u.FirstName.Contains(filter) || u.LastName.Contains(filter));

                }
               
                var noGroupCount = companyUsers.Count();

                return Ok(new { users = await companyUsers.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync(), totalUsers = noGroupCount });
            }
            else
            {
                if (requestedModerator)
                {
                    users = _context.GroupUsers
                        .Where(g => g.GroupId == groupId && g.RequestedModerator == true)
                        .Where(c => c.CompanyId == companyId);
                } else
                {

                    users = _context.GroupUsers
                       .Where(g => g.GroupId == groupId)
                       .Where(c => c.CompanyId == companyId).Where(m => m.Moderator == moderator).Where(g => g.GroupRequest == groupRequest);
                }
            }

            users = users.Include(u => u.User);

            if (filter != null && filter != "null")
            {
                users = users.Where(u => u.User.Email.Contains(filter) || u.User.FirstName.Contains(filter) || u.User.LastName.Contains(filter));
            }

            var count = await users.CountAsync();

          

            return Ok(new { users = await users.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync(), totalUsers = count });
             }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Post>> PostGroupUser(GroupUser groupUser)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault()?.Value);
            var user = await _context.Users.Where(x => x.UserID == userId).FirstOrDefaultAsync();
            var moderator = await _context.GroupUsers.Where(u => u.GroupId == groupUser.GroupId).Where(u => u.UserId == user.UserID).Where(u => u.Moderator == true).FirstOrDefaultAsync();

            if (user.RoleId == 4 || user.RoleId == 3 || moderator != null || groupUser.GroupRequest == true)
            {

                _context.GroupUsers.Add(groupUser);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGroupUser", new { id = groupUser.GroupUserId }, groupUser);
            } else
            {
                return Forbid();
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupUser>> GetGroupUser(int id)
        {
            var groupUser = await _context.GroupUsers.Include(x => x.Group).FirstOrDefaultAsync(x => x.GroupUserId == id);

            if (groupUser == null)
            {
                return NotFound();
            }

            return groupUser;
        }

        [Authorize]
        [HttpGet("group/user/{userId}/{groupId}")]
        public async Task<ActionResult<GroupUser>> GetSpecificUserGroup(int userId, int groupId)
        {
            var groupUser = await _context.GroupUsers.Where(x => x.UserId == userId).Where(x => x.GroupId == groupId).FirstOrDefaultAsync();
          

            return groupUser;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserGroup(int id, GroupUser groupUser)
        {
            if (id != groupUser.GroupUserId)
            {
                return BadRequest();
            }

            _context.Entry(groupUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupUserExists(id))
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

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<GroupUser>> DeleteGroupUser(int id)
        {
            var groupUser = await _context.GroupUsers.FindAsync(id);
            if (groupUser == null)
            {
                return NotFound();
            }

            var userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault()?.Value);
            var user = await _context.Users.Where(x => x.UserID == userId).FirstOrDefaultAsync();
            var moderator = await _context.GroupUsers.Where(u => u.GroupId == groupUser.GroupId).Where(u => u.UserId == user.UserID).Where(u => u.Moderator == true).FirstOrDefaultAsync();

            if (user.RoleId == 4 || user.RoleId == 3 || moderator != null)
            {
                _context.GroupUsers.Remove(groupUser);
                await _context.SaveChangesAsync();

                return groupUser;
            } else
            {
                return Forbid();
            }
         
        }

        private bool GroupUserExists(int id)
        {
            return _context.GroupUsers.Any(e => e.GroupUserId == id);
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<GroupUser>>> GetGroupsByUser(int userId)
        {
            return await _context.GroupUsers.Where(u => u.UserId == userId).Include(g => g.Group).Include(u => u.User).ToListAsync();
        }




    }
}
