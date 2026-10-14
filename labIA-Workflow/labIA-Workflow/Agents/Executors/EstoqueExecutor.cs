using labIA_Workflow.Models.Dto;
using labIA_Workflow.Tools.Mock;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents.Executors
{
    public class EstoqueExecutor(AIAgent _agent) : Executor("EstoqueExecutor")
    {
        public async ValueTask<DisponibilidadeResultado> HandleAsync(ChatMessage message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var response = await _agent.RunAsync(message, cancellationToken: cancellationToken);

            return new DisponibilidadeResultado(ResolveDisponibilidade(response), response.Text);
        }

        private static bool ResolveDisponibilidade(AgentResponse response)
        {
            var chamadaConsultaEstoque = response.Messages
                .SelectMany(m => m.Contents)
                .OfType<FunctionCallContent>()
                .FirstOrDefault(c => c.Name == nameof(EstoqueTool.ConsultarEstoque));

            // Se o EstoqueAgent não chamou ConsultarEstoque para um produto específico
            // (ex: perguntas gerais, lista de produtos), não há disponibilidade a verificar.
            if (chamadaConsultaEstoque is null)
                return true;

            var produtoNome = chamadaConsultaEstoque.Arguments?.TryGetValue("produto", out var valor) == true
                ? valor?.ToString()
                : null;

            if (string.IsNullOrWhiteSpace(produtoNome))
                return true;

            var produto = Estoque.GetProdutoByName(produtoNome);

            return produto is not null && produto.Quantidade > 0;
        }

        protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
        {
            protocolBuilder.ConfigureRoutes(
                builder => builder.AddHandler<ChatMessage, DisponibilidadeResultado>(HandleAsync));

            return protocolBuilder;
        }
    }
}
