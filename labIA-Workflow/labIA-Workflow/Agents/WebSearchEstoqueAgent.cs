using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents
{
    public static class WebSearchEstoqueAgent
    {
        private const float Temperature = 0.2f;

        private const string Instructions = """
        Você é um assistente de pesquisa de preços. Quando o usuário perguntar sobre um produto,
        pesquise na web especificamente no site mercadolivre.com.br (inclua "site:mercadolivre.com.br"
        na sua busca) e responda com o produto que apresente o menor preço encontrado, junto com o nome
        do anúncio correspondente.
        Se não encontrar nenhum resultado no Mercado Livre, informe isso ao usuário claramente, em vez
        de responder com preços de outras fontes.
        """;

        public static AIAgent Create(IChatClient chatClient)
        {
            var options = new ChatClientAgentOptions
            {
                Name = "PesquisadorMercadoLivre",
                ChatOptions = new ChatOptions
                {
                    Instructions = Instructions,
                    Temperature = Temperature,
                    Tools = [new HostedWebSearchTool()]
                }
            };

            return chatClient.AsAIAgent(options);
        }
    }
}
