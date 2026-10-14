using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents
{
    public static class WebSearchEstoqueAgent
    {
        private const float Temperature = 0.2f;

        private const string Instructions = """
        Quando o usuário perguntar sobre um produto, pesquise na web especificamente no site
        mercadolivre.com.br (inclua "site:mercadolivre.com.br" na sua busca) e encontre o anúncio
        mais relevante para esse produto.
        Responda APENAS com a URL completa do anúncio encontrado, sem nenhum texto adicional.
        Se não encontrar nenhum resultado no Mercado Livre, responda apenas com a palavra
        NAO_ENCONTRADO, sem mais nada.
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
