using GeminiDotnet.V1Beta;
using GeminiDotnet.V1Beta.CachedContents;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents.Agents
{
    public static class ScrapingAgent
    {
        private const float Temperature = 0.1f;

        private const string Instructions = """
        Você é um extrator de dados de produtos. Você recebe a URL de um anúncio do Mercado Livre
        e deve acessar essa página para extrair as informações reais do produto.
        Responda sempre no formato:
        Descrição: <resumo curto>
        Preços: <valor mais barato presente na página> até <valor mais caro presente na página>
        Marcas: <lista de marcas. Exiba apenas três das marcas presentes na página>
        Link: <link da página>
        Se não conseguir acessar a página ou não encontrar alguma dessas informações, informe isso
        claramente no campo correspondente, em vez de inventar um valor.
        """;

        public static AIAgent Create(IChatClient chatClient)
        {
            var options = new ChatClientAgentOptions
            {
                Name = "ExtratorMercadoLivre",
                ChatOptions = new ChatOptions
                {
                    Instructions = Instructions,
                    Temperature = Temperature,
                    // Model e Contents precisam ficar null! (não "" / []) — o mapper do GeminiDotnet
                    // usa '??' para preencher esses campos com os valores reais da conversa, então
                    // qualquer valor não-nulo aqui (mesmo vazio) sobrescreveria a mensagem real.
                    RawRepresentationFactory = _ => new GenerateContentRequest
                    {
                        Model = null!,
                        Contents = null!,
                        Tools = [new Tool { UrlContext = new UrlContext() }]
                    }
                }
            };

            return chatClient.AsAIAgent(options);
        }
    }
}
