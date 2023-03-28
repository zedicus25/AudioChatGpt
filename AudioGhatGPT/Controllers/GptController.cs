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
        public async Task<ActionResult<string>> GetRepsponceFromAudio(IFormFile file)
        {
            //string transcribeText = await _transcribeHelper.TranscribeMediaFile(file);
            //string openAIRes = await _openAIController.GetResponce("how are you?");
            return "Responce from audio";
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.UserRoles.UserPremium},{UserRoles.UserRoles.UserFreePlus},{UserRoles.UserRoles.UserPlus}")]
        [Route("getReponseFromImage")]
        public async Task<ActionResult<string>> GetRepsponceFromImage(IFormFile file)
        {
            //string transcribeImage = await _transcribeHelper.GetLinesFromImage(file);
            //string openAIRes = await _openAIController.GetResponce(transcribeImage);
            return "Responce from image";
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.UserRoles.UserPremium}," +
            $"{UserRoles.UserRoles.UserFreePlus}," +
            $"{UserRoles.UserRoles.UserPlus}," +
            $"{UserRoles.UserRoles.UserFree}")]
        [Route("getReponseFromText")]
        public async Task<ActionResult<string>> GetRepsponceFromText([FromQuery(Name ="requestText")]string requestText)
        {
            string openAIRes = await _openAIController.GetResponce(requestText);
            return openAIRes;
        }
    }
}
