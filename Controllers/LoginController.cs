using Iste_Api.Context;
using Iste_Api.Entities;
using Iste_Api.TokenConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Iste_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        readonly IsteDbContext _context;
        readonly IConfiguration _configuration;
        public LoginController(IsteDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var listUser = _context.Users.ToList();
            return Ok(listUser);
        }
        [HttpPost("[action]")]
        public async Task<bool> Create([FromBody] User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        [HttpPost("[action]")]
        public async Task<Iste_Api.Models.Token> Login([FromForm] UserLogin userLogin)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userLogin.Email && x.Password == userLogin.Password);
            if (user != null)
            {
                TokenHandler tokenHandler = new TokenHandler(_configuration);
                Iste_Api.Models.Token token = tokenHandler.CreateAccessToken(user);

                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);
                await _context.SaveChangesAsync();

                return token;
            }
            return null;
        }
        [HttpGet("[action]/{email}")]
        public async Task<ActionResult<User>> UserGetById(string email)
        {
            var getirUlan = await _context.Users.Where(x=>x.Email == email).SingleOrDefaultAsync();

            return getirUlan;
        }
        [HttpGet("api/{search}")]
        public async Task<IActionResult> Search(string search ="")
        {
            var query = await _context.Users.Where(x=>
                 x.Name.ToLower().Contains(search.ToLower())
                ||x.Surname.ToLower().Contains(search.ToLower())
    
            ).ToListAsync();
            return Ok(query);
        }
}
}
