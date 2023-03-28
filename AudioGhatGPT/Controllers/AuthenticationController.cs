namespace AudioGhatGPT.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Domain.Models;
    using Microsoft.AspNetCore.Identity;
    using Domain.Interfaces.IUnitOfWorks;

    namespace AudioGhatGPT.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AuthenticationController : ControllerBase
        {
            private readonly IConfiguration _configuration;
            private readonly IUnitOfWorks _unitOfWorks;

            public AuthenticationController(IConfiguration configuration, IUnitOfWorks unitOfWorks)
            {
                _unitOfWorks = unitOfWorks;
                _configuration = configuration;
            }

            [HttpPost]
            [Route("login")]
            public async Task<IActionResult> Login([FromBody] Login model)
            {
                var user = await _unitOfWorks.UsersRepo.FindByLoginAsync(model.UserName);
                if (user != null && _unitOfWorks.UsersRepo.CheckPassword(user, model.Password))
                {
                    var authClaims = new List<Claim> {
                        new Claim(ClaimTypes.Name, user.Login),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var token = GetToken(authClaims);

                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                    });
                }
                return Unauthorized();
            }

            [HttpPost]
            [Route("regUser")]
            public async Task<IActionResult> RegUser([FromBody] Register model)
            {
                var userEx =  await _unitOfWorks.UsersRepo.FindByLoginAsync(model.UserName);
                if (userEx != null) 
                    return StatusCode(StatusCodes.Status500InternalServerError, "User in db already");

                User user = new()
                {
                    Login = model.UserName,
                    Password = model.Password,
                    SubscriptionId = (int)Subscriptions.Subscriptions.Free,
                    CountRequests = 0,
                    LastRequest = null 
                };

                _unitOfWorks.UsersRepo.Add(user);
                if(_unitOfWorks.Commit() > 0)
                    return Ok("User added!");
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong!");
            }

            private JwtSecurityToken GetToken(List<Claim> claimsList)
            {
                var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(6),
                        claims: claimsList,
                        signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256)
                    );

                return token;
            }
        }
    }
}
