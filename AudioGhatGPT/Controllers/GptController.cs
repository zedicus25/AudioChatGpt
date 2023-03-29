using Amazon;
using Amazon.Util.Internal;
using Domain.Interfaces.IUnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AudioGhatGPT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GptController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly TranscribeHelper _transcribeHelper;
        private readonly OpenAIController _openAIController;

        public GptController(IConfiguration configuration, IUnitOfWorks unitOfWorks)
        {
            _unitOfWorks = unitOfWorks;
            _configuration = configuration;
            _transcribeHelper = new TranscribeHelper();
            _openAIController = new OpenAIController();
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.UserRoles.UserPremium}")]
        [Route("getReponseFromAudio")]
        public async Task<ActionResult<string>> GetRepsponceFromAudio()
        {
            var file = Request.Form.Files[0];
            int userId = Convert.ToInt32(Request.Form["userId"].ToString());
            var user = _unitOfWorks.UsersRepo.GetAll().Result.FirstOrDefault(x => x.Id == userId);
            if (user.IsBanned)
                return "You banned!";

            if (user == null)
                return NotFound();

            var subscription = _unitOfWorks.SubscriptionRepo.GetSubscriptionById(user.SubscriptionId);
            if (subscription == null)
                return NotFound();
          


            if (subscription.MaxImageRequests < 0)
            {
                user.LastImageRequest = DateTime.Now;
                //string transcribeText = await _transcribeHelper.TranscribeMediaFile(file);
                //string openAIRes = await _openAIController.GetResponce("transcribeText");
                if (_unitOfWorks.Commit() > 0)
                    return "Image responce";
                return StatusCode(StatusCodes.Status500InternalServerError, "Error!");
            }
            else
                return "Day limit!";
           

        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.UserRoles.UserPremium},{UserRoles.UserRoles.UserFreePlus},{UserRoles.UserRoles.UserPlus}")]
        [Route("getReponseFromImage")]
        public async Task<ActionResult<string>> GetRepsponceFromImage()
        {
            var file = Request.Form.Files[0];
            int userId = Convert.ToInt32( Request.Form["userId"].ToString());

            var user = _unitOfWorks.UsersRepo.GetAll().Result.FirstOrDefault(x => x.Id == userId);
            if (user.IsBanned)
                return "You banned!";

            if (user == null)
                return NotFound();

            var subscription = _unitOfWorks.SubscriptionRepo.GetSubscriptionById(user.SubscriptionId);
            if (subscription == null)
                return NotFound();
            if (user.LastImageRequest != null)
            {
                if (user.CountImageRequests >= subscription.MaxImageRequests &&
                    user.LastImageRequest.Value.Day < DateTime.Now.Day)
                {
                    user.LastImageRequest = null;
                    user.CountTextRequests = 0;
                }
            }


            if (subscription.MaxImageRequests < 0 || user.CountImageRequests < subscription.MaxImageRequests)
            {
                if (subscription.MaxTextRequests > 0)
                    user.CountImageRequests += 1;
                user.LastImageRequest = DateTime.Now;
                //string transcribeImage = await _transcribeHelper.GetLinesFromImage(file);
                //string openAIRes = await _openAIController.GetResponce(transcribeImage);
                if (_unitOfWorks.Commit() > 0)
                    return "Image responce";
                return StatusCode(StatusCodes.Status500InternalServerError, "Error!");
            }
            else
                return "Day limit!";
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.UserRoles.UserPremium}," +
            $"{UserRoles.UserRoles.UserFreePlus}," +
            $"{UserRoles.UserRoles.UserPlus}," +
            $"{UserRoles.UserRoles.UserFree}")]
        [Route("getReponseFromText")]
        public async Task<ActionResult<string>> GetRepsponceFromText([FromQuery(Name ="requestText")]string requestText,
            [FromQuery(Name = "userId")]int userId)
        {
            var user = _unitOfWorks.UsersRepo.GetAll().Result.FirstOrDefault(x => x.Id== userId);
            if (user.IsBanned)
                return "You banned!";

            if (user == null)
                return NotFound();

            var subscription = _unitOfWorks.SubscriptionRepo.GetSubscriptionById(user.SubscriptionId);
            if (subscription == null) 
                return NotFound();
            if(user.LastTextRequest != null)
            {
                if (user.CountTextRequests >= subscription.MaxTextRequests &&
                    user.LastTextRequest.Value.Day < DateTime.Now.Day)
                {
                    user.LastTextRequest = null;
                    user.CountTextRequests = 0;
                }
            }
                

            if(subscription.MaxTextRequests < 0 || user.CountTextRequests < subscription.MaxTextRequests)
            {
                //string openAIRes = await _openAIController.GetResponce(requestText);
                if(subscription.MaxTextRequests > 0)
                    user.CountTextRequests += 1;
                user.LastTextRequest = DateTime.Now;
                if(_unitOfWorks.Commit() > 0)
                    return "Text responce";
                return StatusCode(StatusCodes.Status500InternalServerError, "Error!");
            }
            else
                return "Day limit!";
        }
    }
}
