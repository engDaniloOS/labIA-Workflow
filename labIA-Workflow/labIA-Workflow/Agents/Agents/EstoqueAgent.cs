using labIA_Workflow.Tools.Mock;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents.Agents
{
    public static class EstoqueAgent
    {
        private const string Instructions = """
        Você é um analista de estoque. Use as ferramentas disponíveis para consultar
        quantidade e preço de produtos, e responda ao usuário com base nos resultados obtidos.
        Se o produto não for encontrado, informe ao usuário que o produto não está disponível, e diga que o buscará no mercado livre.
        Não invente informações sobre produtos que não existem.
        Se o usuário pedir a lista com todos os produtos, diga que tem varios, e de apenas 2 como exemplo, com os seus detalhes.
        Se o produto estiver no estoque, finalize a resposta questionando o usuário se ele deseja consultar outro produto, ou mais informações sobre o produto já consultado.
        """;

        private const float Temperature = 0.3f;

        public static AIAgent Create(IChatClient chatClient)
        {
            var estoqueService = new EstoqueTool();

            var options = new ChatClientAgentOptions
            {
                Name = "AnalistaEstoque",
                ChatOptions = new ChatOptions
                {
                    Instructions = Instructions,
                    Temperature = Temperature,
                    Tools =
                    [
                        AIFunctionFactory.Create(estoqueService.ConsultarEstoque),
                        AIFunctionFactory.Create(estoqueService.ConsultarPreco),
                        AIFunctionFactory.Create(estoqueService.ConsultarProdutosPorLimiteDePreco),
                        AIFunctionFactory.Create(estoqueService.ListarProdutos)
                    ]
                }
            };

            return chatClient.AsAIAgent(options);
        }
    }
}
