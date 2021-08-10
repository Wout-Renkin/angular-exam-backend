using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AngularProjectAPI.Data;
using AngularProjectAPI.Models;
using AngularProjectAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private readonly NewsContext _context;

        public UserController(IUserService userService, NewsContext context)
        {
            _userService = userService;
            _context = context;
        }

        //[Authorize(Roles = "SuperAdmin")]
        [Authorize]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] int pageSize = 10, int currentPage = 1, int companyId = 0, string filter = null, int? roleId = null)
        {
            var userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault()?.Value);
            var user = await _context.Users.Where(x => x.UserID == userId).FirstOrDefaultAsync();
            if(user.RoleId == 4)
            {
                IQueryable<User> users;


                if (filter == null || filter == "null")
                {
                    if (companyId == 0)
                    {
                        users = _context.Users.Where(r => r.RoleId == roleId);

                    }
                    else
                    {
                        Console.WriteLine(companyId);
                        users = _context.Users.Where(c => c.CompanyId == companyId).Where(r => r.RoleId == roleId);
                        Console.WriteLine(users);
                    }

                }
                else
                {
                    users = _context.Users.Where(u => u.Email.Contains(filter) || u.FirstName.Contains(filter) || u.LastName.Contains(filter));
                    if (companyId == 0)
                    {
                        users = users.Where(r => r.RoleId == roleId);

                    }
                    else
                    {
                        users = users.Where(c => c.CompanyId == companyId).Where(r => r.RoleId == roleId);
                    }

                }



                var count = await users.CountAsync();

                return Ok(new { users = await users.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync(), totalUsers = count });
            } else
            {
                return Forbid();
            }


            

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersRoleCompany()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            //user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var userEmail = _context.Users.Where(x => x.Email == user.Email).FirstOrDefault(); 
            if (userEmail != null)
            {
                return BadRequest(new { message = "Email already exists!" });
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();



            return Ok(user);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Email, userParam.Password);


            if (user == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            _context.Entry(user).Property(x => x.Password).IsModified = false;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }

    }
}