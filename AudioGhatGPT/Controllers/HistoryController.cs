using Domain.Interfaces.IUnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioGhatGPT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly IUnitOfWorks _unitOfWorks;

        public HistoryController(IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.UserRoles.UserPlus)]
        [Route("getHistory")]
        public async Task<IActionResult> GetHistory([FromQuery(Name = "userId")] int userId)
        {
            var userList = await _unitOfWorks.UsersRepo.GetAll();
            var user = userList.FirstOrDefault(x => x.Id == userId);

            if (user == null)
                return NotFound();

            var histroy = await _unitOfWorks.HistoryRepo.GetAll();
            var userHistory = histroy.Where(x => x.UserId == user.Id);
            var request = await _unitOfWorks.RequestRepo.GetAll();
            var userRequest = request.Where(x => userHistory.Any(y => y.RequestId == x.Id));
            var responces = await _unitOfWorks.ResponceRepo.GetAll();
            var userResponce = responces.Where(x => userHistory.Any(y => y.ResponceId == x.Id));

            List<string> stringReq = new List<string>();
            foreach (var item in userRequest)
            {
                stringReq.Add(item.RequestText);
            }
            List<string> stringRes = new List<string>();
            foreach (var item in userResponce)
            {
                stringRes.Add(item.ResponceText);
            }
            return Ok(new
            {
                Request = stringReq,
                Response = stringRes
            });
        }
    }
}
