using CookieBasedAuth.Data;
using CookieBasedAuth.Models;
using CookieBasedAuth.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CookieBasedAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly PasswordHasher<UserDto> _passwordHasher;

        public AuthenticationController(AppDbContext _dbContext)
        {
            this.dbContext = _dbContext;
            _passwordHasher = new PasswordHasher<UserDto>();
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup(UserDto user)
        {
            var existsUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existsUser != null)
            {
                return BadRequest("User already exists with this email");
            }
            var hashedPassword = _passwordHasher.HashPassword(user, user.Password);
            var newUser = new User()
            {
                Email = user.Email,
                FullName = user.FullName,
                Password = hashedPassword
            };
            await dbContext.Users.AddAsync(newUser);
            await dbContext.SaveChangesAsync();
            return Created();
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Signin(UserDto user)
        {
            {
                var _user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (_user == null)
                {
                    return BadRequest("Invalid creadentials");
                }

                var result = _passwordHasher.VerifyHashedPassword(user, _user.Password, user.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, _user.Email),
                        new Claim("UserId", _user.Id.ToString()),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                        });

                    return Ok("Signed in successfully");
                }
                else
                {
                    return BadRequest("Invalid creadentials");
                }
            }
        }

        [HttpPost("signout")]
        [Authorize]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Signed out successfully");
        }
    }
}
