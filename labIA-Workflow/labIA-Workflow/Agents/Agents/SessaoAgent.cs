using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents.Agents
{
    public static class SessaoAgent
    {
        private const float Temperature = 0.0f;

        private const string Instructions = """
        Você recebe o histórico de uma conversa sobre consulta de produtos em estoque, terminando
        com a pergunta mais recente do usuário.
        Verifique se essa pergunta já foi respondida anteriormente, na própria conversa, com a
        mesma informação que está sendo pedida agora. Se sim, repita essa resposta já dada.
        Se a pergunta for nova, ou pedir uma informação que ainda não apareceu na conversa,
        responda apenas com a palavra NAO_ENCONTRADO, sem mais nada.
        """;

        public static AIAgent Create(IChatClient chatClient)
        {
            var options = new ChatClientAgentOptions
            {
                Name = "AnalistaMemoria",
                ChatOptions = new ChatOptions
                {
                    Instructions = Instructions,
                    Temperature = Temperature
                }
            };

            return chatClient.AsAIAgent(options);
        }
    }
}
