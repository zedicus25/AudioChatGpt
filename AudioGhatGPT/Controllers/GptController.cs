using Amazon;
using Amazon.Util.Internal;
using Domain.Interfaces.IUnitOfWorks;
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
        [Route("getReponseFromAudio")]
        public async Task<ActionResult<string>> GetRepsponceFromAudio(IFormFile file)
        {
            string transcribeText = await _transcribeHelper.TranscribeMediaFile(file);
            string openAIRes = await _openAIController.GetResponce(transcribeText);
            return openAIRes;
        }
    }
}
