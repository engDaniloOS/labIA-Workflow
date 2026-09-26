using labIA_Workflow.Tools.Mock;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents
{
    public static class GeneralAgent
    {
        private const string Instructions = """
        Você é um analista de estoque. Use as ferramentas disponíveis para consultar
        quantidade e preço de produtos, e responda ao usuário com base nos resultados obtidos.
        """;

        public static AIAgent Create(IChatClient chatClient)
        {
            var estoqueService = new EstoqueTool();

            return chatClient.AsAIAgent(
                instructions: Instructions,
                name: "AnalistaEstoque",
                tools:
                [
                    AIFunctionFactory.Create(estoqueService.ConsultarEstoque),
                    AIFunctionFactory.Create(estoqueService.ConsultarPreco)
                ]);
        }
    }
}
