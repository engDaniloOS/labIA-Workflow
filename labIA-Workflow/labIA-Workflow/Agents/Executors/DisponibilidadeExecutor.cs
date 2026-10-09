using labIA_Workflow.Models.Dto;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents.Executors
{
    public class DisponibilidadeExecutor(AIAgent _agent) : Executor("DisponibilidadeExecutor")
    {
        public async ValueTask<DisponibilidadeResultado> HandleAsync(string respostaEstoque, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var response = await _agent.RunAsync<bool>(
                new ChatMessage(ChatRole.User, respostaEstoque),
                cancellationToken: cancellationToken);

            return new DisponibilidadeResultado(response.Result, respostaEstoque);
        }

        protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
        {
            protocolBuilder.ConfigureRoutes(
                builder => builder.AddHandler<string, DisponibilidadeResultado>(HandleAsync));

            return protocolBuilder;
        }
    }
}
