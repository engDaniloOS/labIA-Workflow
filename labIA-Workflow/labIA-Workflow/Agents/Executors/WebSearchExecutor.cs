using labIA_Workflow.Models.Dto;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents.Executors
{
    public class WebSearchExecutor(AIAgent _agent) : Executor("WebSearchExecutor")
    {
        public async ValueTask<string> HandleAsync(ConfirmacaoBusca resultado, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var response = await _agent.RunAsync(
                new ChatMessage(ChatRole.User, resultado.RespostaEstoque),
                cancellationToken: cancellationToken);

            return response.Text;
        }

        protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
        {
            protocolBuilder.ConfigureRoutes(
                builder => builder.AddHandler<ConfirmacaoBusca, string>(HandleAsync));

            return protocolBuilder;
        }
    }
}
