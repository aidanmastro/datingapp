using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
        /// <summary>
        /// Gets all members
        /// </summary>
        /// <returns></returns>
        /// <remarks>EXAMPLE: GET localhost:5001/api/members</remarks>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        {
            return Ok(await memberRepository.GetMembersAsync());  
        }

        /// <summary>
        /// Gets a particular Member 
        /// </summary>
        /// <param name="id">The user's identifier</param>
        /// <returns></returns>
        /// <remarks>EXAMPLE: GET localhost:5001/api/members</remarks>
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var member = await memberRepository.GetMemberByIdAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        /// <summary>
        /// Gets all Photo(s) for particular member
        /// </summary>
        /// <returns></returns>
        /// <remarks>EXAMPLE: GET localhost:5001/api/members</remarks>
        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok(await memberRepository.GetPhotosForMemberAsync(id));
        }
    }
}
