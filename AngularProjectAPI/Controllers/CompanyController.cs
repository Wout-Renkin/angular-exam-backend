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
    public class CompanyController : ControllerBase
    {
        private readonly NewsContext _context;

        public CompanyController(NewsContext context)
        {
            _context = context;
        }

        //Get a specific company
        //GET: api/company/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        //Delete a company
        //DELETE: api/company/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return company;
        }

        //Create a company
        //POST: api/company
        /* [HttpPost]
         public async Task<ActionResult<Company>> PostCompany(Company company)
         {

             _context.Companies.Add(company);
             await _context.SaveChangesAsync();

             return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
         }
 */
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany([FromForm] Company company)
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
                company.ImagePath = "https://localhost:44348/" + dbPath;
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);

            }
            else
            {
                return BadRequest();
            }


            

        }

        //Update a company
        //PUT: api/company/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, [FromForm] Company company)
        {

            var userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault()?.Value);
            var user = await _context.Users.Where(x => x.UserID == userId).FirstOrDefaultAsync();
            if (user.RoleId == 4)
            {
                if (id != company.CompanyId)
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
                        company.ImagePath = "https://localhost:44348/" + dbPath;

                    }
                }



                _context.Entry(company).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            } else
            {
                return Forbid();
            }
 
        }

        //Check if company exists
        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(c => c.CompanyId == id);
        }




    }
}
