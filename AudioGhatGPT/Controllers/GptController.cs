using Amazon;
using Amazon.Util.Internal;
using Domain.Interfaces.IUnitOfWorks;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static OpenAI.GPT3.ObjectModels.SharedModels.IOpenAiModels;

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


            user.LastImageRequest = DateTime.Now;
            
            string transcribeText = await _transcribeHelper.TranscribeMediaFile(file);
            string openAIRes = await _openAIController.GetResponce("transcribeText");
            if (SaveHistory(transcribeText, openAIRes, user.Id))
                return openAIRes;
            return StatusCode(StatusCodes.Status500InternalServerError, "Error!");
        }
            
        [HttpPost]
        [Authorize(Roles = $"{UserRoles.UserRoles.UserPremium},{UserRoles.UserRoles.UserFreePlus},{UserRoles.UserRoles.UserPlus}")]
        [Route("getReponseFromImage")]
        public async Task<ActionResult<string>> GetRepsponceFromImage()
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
                string transcribeImage = await _transcribeHelper.GetLinesFromImage(file);
                string openAIRes = await _openAIController.GetResponce(transcribeImage);
                if (SaveHistory(transcribeImage, openAIRes, user.Id))
                    return openAIRes;
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
        public async Task<ActionResult<string>> GetRepsponceFromText([FromQuery(Name = "requestText")] string requestText,
            [FromQuery(Name = "userId")] int userId)
        {
            var user = _unitOfWorks.UsersRepo.GetAll().Result.FirstOrDefault(x => x.Id == userId);
            if (user.IsBanned)
                return "You banned!";

            if (user == null)
                return NotFound();

            var subscription = _unitOfWorks.SubscriptionRepo.GetSubscriptionById(user.SubscriptionId);
            if (subscription == null)
                return NotFound();
            if (user.LastTextRequest != null)
            {
                if (user.CountTextRequests >= subscription.MaxTextRequests &&
                    user.LastTextRequest.Value.Day < DateTime.Now.Day)
                {
                    user.LastTextRequest = null;
                    user.CountTextRequests = 0;
                }
            }


            if (subscription.MaxTextRequests < 0 || user.CountTextRequests < subscription.MaxTextRequests)
            {
                string openAIRes = await _openAIController.GetResponce(requestText);
                if (subscription.MaxTextRequests > 0)
                    user.CountTextRequests += 1;
                user.LastTextRequest = DateTime.Now;
                if (SaveHistory(requestText, openAIRes, user.Id))
                    return openAIRes;
                return StatusCode(StatusCodes.Status500InternalServerError, "Error!");
            }
            else
                return "Day limit!";
        }

        private bool SaveHistory(string request, string responce, int userId)
        {
            Request req = new Request()
            {
                RequestText = request,
                RequestTime = DateTime.Now,
            };
            Responce res = new Responce()
            {
                ResponceText = responce
            };
            _unitOfWorks.ResponceRepo.Add(res);
            _unitOfWorks.RequestRepo.Add(req);
            if (_unitOfWorks.Commit() > 0)
            {
                int reqId = _unitOfWorks.RequestRepo.GetAll().Result.LastOrDefault().Id;
                int resId = _unitOfWorks.ResponceRepo.GetAll().Result.LastOrDefault().Id;
                History his = new History { RequestId = reqId, ResponceId = resId, UserId = userId };
                _unitOfWorks.HistoryRepo.Add(his);
                return _unitOfWorks.Commit() > 0;
            }
            return false;
        }
    }
}
