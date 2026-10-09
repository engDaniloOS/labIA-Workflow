using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents
{
    public static class AnaliseDisponibilidadeAgent
    {
        private const string Instructions = """
            Você é um analista de estoque com uma função muito simples.
            Ao receber os dados de um produto, retorne o booleano false se o produto não estiver disponível no estoque, 
            ou o booleano true se o produto estiver disponível.
            Se produto estiver listado no estoque, mas a quantidade for zero, retorne false.
            """;

        private const float Temperature = 0.0f;

        public static AIAgent Create(IChatClient chatClient)
        {
            var options = new ChatClientAgentOptions
            {
                Name = "AnalistaDisponibilidade",
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
