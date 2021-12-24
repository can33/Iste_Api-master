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
    public class AcedemicianController : ControllerBase
    {
        readonly IsteDbContext _context;
        readonly IConfiguration _configuration;
        public AcedemicianController(IsteDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public async Task<Iste_Api.Models.Token> Login([FromForm] UserLogin userLogin)
        {
            Academician academician = await _context.Academicians.FirstOrDefaultAsync(x => x.Email == userLogin.Email && x.Password == userLogin.Password);
            if (academician != null)
            {
                TokenHandler tokenHandler = new TokenHandler(_configuration);
                Iste_Api.Models.Token token = tokenHandler.CreateAccessToken(academician);

                academician.RefreshToken = token.RefreshToken;
                academician.RefreshTokenEndDate = token.Expiration.AddMinutes(3);
                await _context.SaveChangesAsync();

                return token;
            }
            return null;
        }
        [HttpGet("[action]/{email}")]
        public async Task<ActionResult<Academician>> UserGetById(string email)
        {
            var getirUlan = await _context.Academicians.Where(x=>x.Email == email).SingleOrDefaultAsync();

            return getirUlan;
        }
        [HttpGet("api/{search}")]
        public async Task<IActionResult> Search(string search ="")
        {
            var query = await _context.Academicians.Where(x=>
                 x.Name.ToLower().Trim().Contains(search.ToLower().Trim())
                || x.Surname.ToLower().Trim().Contains(search.ToLower().Trim())
    
            ).ToListAsync();
            return Ok(query);
        }
    }
}