using AudioGhatGPT.UserRoles;
using Domain.Interfaces.IUnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AudioGhatGPT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWorks _unitOfWorks;

        public SubscriptionController(IConfiguration configuration, IUnitOfWorks unitOfWorks, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWorks = unitOfWorks;
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("updateSubscription")]
        public async Task<IActionResult> UpdateSubcsription([FromQuery(Name = "userId")] int userId, [FromQuery(Name = "subscriptionId")] int subscriptionId)
        {
            var user = _unitOfWorks.UsersRepo.GetAll().Result.FirstOrDefault(x => x.Id == userId);

            if (user == null)
                return NotFound();

            var subscription = _unitOfWorks.SubscriptionRepo.GetSubscriptionById(subscriptionId);
            if (subscription == null)
                return NotFound();
            
            var identityUser = await _userManager.FindByIdAsync(user.IdentityId);
            var roles = await _userManager.GetRolesAsync(identityUser);
            var newRoles = GetRoles(subscriptionId);
            var deleteRes = await _userManager.RemoveFromRolesAsync(identityUser, roles);
            if (!deleteRes.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed!");

            var res = await _userManager.AddToRolesAsync(identityUser, newRoles);
            if (!res.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed!");

            user.SubscriptionId = subscriptionId;
            if(_unitOfWorks.Commit() <= 0)
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed!");

            return Ok();
        }

        private IEnumerable<string> GetRoles(int subscriptionId)
        {
            List<string> res = new List<string>();
            switch (subscriptionId)
            {
                case 1:
                    res.Add(UserRoles.UserRoles.UserFree);
                    break;
                case 2:
                    res.Add(UserRoles.UserRoles.UserFree);
                    res.Add(UserRoles.UserRoles.UserFreePlus);
                    break;
                case 3:
                    res.Add(UserRoles.UserRoles.UserFree);
                    res.Add(UserRoles.UserRoles.UserFreePlus);
                    res.Add(UserRoles.UserRoles.UserPlus);
                    break;
                case 4:
                    res.Add(UserRoles.UserRoles.UserFree);
                    res.Add(UserRoles.UserRoles.UserFreePlus);
                    res.Add(UserRoles.UserRoles.UserPlus);
                    res.Add(UserRoles.UserRoles.UserPremium);
                    break;
            }
            return res;
        }
    }
}
