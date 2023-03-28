using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using System.Text;

namespace AudioGhatGPT.Controllers
{
    public class OpenAIController
    {
        private readonly OpenAIService _openAIService;
        private readonly StringBuilder _stringBuilder;
        public OpenAIController()
        {
            _stringBuilder = new StringBuilder();   
            _openAIService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = ConfigurationManager.AppSettings["OpenAI:ApiKey"]
            });
        }

        public async Task<string> GetResponce(string promt)
        {
            var completionResult = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Model = Models.ChatGpt3_5Turbo,
                Temperature = 0.7F,
                MaxTokens = 100,
                Messages = new List<ChatMessage>
                {
                    new ChatMessage("user",promt)
                },
            });

            if (completionResult.Successful)
            {
                _stringBuilder.Clear();
                completionResult.Choices.ForEach(x => _stringBuilder.Append(x.Delta.Content));
                return _stringBuilder.ToString();
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }
            }
            return "";

        }
    }
}
