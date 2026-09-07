using API.Data;
using API.DTOs;
using API.Entities;
using API.Extentions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
    {
        /// <summary>
        /// Register a user account
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="displayName">User's desired display name</param>
        /// <param name="password">User's password</param>
        /// <returns></returns>
        /// <remarks>EXAMPLE: POST localhost:5001/api/account/register</remarks>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto p_registerDto)
        {
            if (await EmailExists(p_registerDto.Email))
            {
                return BadRequest($"Email address '{p_registerDto.Email}' is already registered to an existing user.");
            }

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                DisplayName = p_registerDto.DisplayName,
                Email = p_registerDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(p_registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user.ToDto(tokenService);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email address or password.");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (var i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid email address or password.");
                }
            }

            return user.ToDto(tokenService);
        }

        private async Task<bool> EmailExists(string p_email)
        {
            return await context.Users.AnyAsync(u => u.Email.ToLower() == p_email.ToLower()); 
        }

    }
}
