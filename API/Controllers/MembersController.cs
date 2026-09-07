using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class MembersController(AppDbContext context) : BaseApiController
    {
        /// <summary>
        /// Gets all members
        /// </summary>
        /// <returns></returns>
        /// <remarks>EXAMPLE: GET localhost:5001/api/members</remarks>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await context.Users.ToListAsync();

            return members;
        }

        /// <summary>
        /// Gets a single member based off of their id
        /// </summary>
        /// <param name="id">The user's identifier</param>
        /// <returns></returns>
        /// <remarks>EXAMPLE: GET localhost:5001/api/members</remarks>
        
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            var member = await context.Users.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }
    }
}
