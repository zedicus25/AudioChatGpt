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
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly UserManager<IdentityUser> _userManager;
            private readonly IConfiguration _configuration;
            private readonly IUnitOfWorks _unitOfWorks;

            public AuthenticationController(IConfiguration configuration, IUnitOfWorks unitOfWorks, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                _unitOfWorks = unitOfWorks;
                _configuration = configuration;
                _roleManager = roleManager;
                _userManager = userManager;
                
            }

            [HttpPost]
            [Route("login")]
            public async Task<IActionResult> Login([FromBody] Login model)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRole = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                    foreach (var role in userRole)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var token = GetToken(authClaims);

                    var myUser = _unitOfWorks.UsersRepo.GetAll().Result.FirstOrDefault(x => x.IdentityId == user.Id);

                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Roles = userRole,
                        UserId = myUser.Id
                    });
                }
                return Unauthorized();
            }

            [HttpPost]
            [Route("regUser")]
            public async Task<IActionResult> RegUser([FromBody] Register model)
            {
                var userEx = await _userManager.FindByNameAsync(model.UserName);
                if (userEx != null) 
                    return StatusCode(StatusCodes.Status500InternalServerError, "User in db already");

                IdentityUser identityUser = new()
                {
                    UserName = model.UserName,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var res = await _userManager.CreateAsync(identityUser, model.Password);
                if (!res.Succeeded) 
                    return StatusCode(StatusCodes.Status500InternalServerError, "Creation failed!");

                User user = new User
                {
                    IdentityId = identityUser.Id,
                    IsBanned = false,
                    LastTextRequest = null,
                    LastImageRequest = null,
                    SubscriptionId = (int)Subscriptions.Subscriptions.Free,
                    UnbanTime = null,
                    CountTextRequests = 0,
                    CountImageRequests = 0
                };
                _unitOfWorks.UsersRepo.Add(user);
                if(_unitOfWorks.Commit() <= 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Creation failed!");


                await CreateRoles();

                if (await _roleManager.RoleExistsAsync(UserRoles.UserRoles.UserFree))
                    await _userManager.AddToRoleAsync(identityUser, UserRoles.UserRoles.UserFree);

                return Ok("User added!");
            }
            [HttpPost]
            [Route("regAdmin")]
            public async Task<IActionResult> RegAdmin([FromBody] Register model)
            {
                var userEx = await _userManager.FindByNameAsync(model.UserName);
                if (userEx != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "User in db already");

                IdentityUser identityUser = new()
                {
                    UserName = model.UserName,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var res = await _userManager.CreateAsync(identityUser, model.Password);
                if (!res.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Creation failed!");

                User user = new User
                {
                    IdentityId = identityUser.Id,
                    IsBanned = false,
                    LastTextRequest = null,
                    LastImageRequest = null,
                    SubscriptionId = (int)Subscriptions.Subscriptions.Premium,
                    UnbanTime = null,
                    CountTextRequests = 0,
                    CountImageRequests = 0
                };
                _unitOfWorks.UsersRepo.Add(user);
                if (_unitOfWorks.Commit() <= 0)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Creation failed!");

                await CreateRoles();

                if (await _roleManager.RoleExistsAsync(UserRoles.UserRoles.UserFree))
                    await _userManager.AddToRoleAsync(identityUser, UserRoles.UserRoles.UserFree);
                if (await _roleManager.RoleExistsAsync(UserRoles.UserRoles.UserFreePlus))
                    await _userManager.AddToRoleAsync(identityUser, UserRoles.UserRoles.UserFreePlus);
                if (await _roleManager.RoleExistsAsync(UserRoles.UserRoles.UserPlus))
                    await _userManager.AddToRoleAsync(identityUser, UserRoles.UserRoles.UserPlus);
                if (await _roleManager.RoleExistsAsync(UserRoles.UserRoles.UserPremium))
                    await _userManager.AddToRoleAsync(identityUser, UserRoles.UserRoles.UserPremium);
                if (await _roleManager.RoleExistsAsync(UserRoles.UserRoles.Admin))
                    await _userManager.AddToRoleAsync(identityUser, UserRoles.UserRoles.Admin);

                return Ok("User added!");
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

            private async Task CreateRoles()
            {
                if (!await _roleManager.RoleExistsAsync(UserRoles.UserRoles.UserFree))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.UserRoles.UserFree));
                if (!await _roleManager.RoleExistsAsync(UserRoles.UserRoles.UserFreePlus))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.UserRoles.UserFreePlus));
                if (!await _roleManager.RoleExistsAsync(UserRoles.UserRoles.UserPlus))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.UserRoles.UserPlus));
                if (!await _roleManager.RoleExistsAsync(UserRoles.UserRoles.UserPremium))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.UserRoles.UserPremium));
                if (!await _roleManager.RoleExistsAsync(UserRoles.UserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.UserRoles.Admin));
            }
        }
    }
}
