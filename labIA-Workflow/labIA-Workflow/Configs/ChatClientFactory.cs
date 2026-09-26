using GeminiDotnet;
using GeminiDotnet.Extensions.AI;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Configs
{
    public static class ChatClientFactory
    {
        public static IChatClient BuildClient()
        {
            var options = new GeminiClientOptions
            {
                ApiKey = GetApiKey(),
                ModelId = "gemini-3.5-flash-lite"
            };

            return new GeminiChatClient(options);
        }

        private static string GetApiKey()
        {
            var apiKey = Environment.GetEnvironmentVariable("AI_API_KEY");

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException(
                    "A variável de ambiente AI_API_KEY não está definida.");

            return apiKey;
        }
    }
}
