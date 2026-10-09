using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents.Executors
{
    public class EstoqueExecutor(AIAgent _agent) : Executor("EstoqueExecutor")
    {
        public async ValueTask<string> HandleAsync(ChatMessage message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var response = await _agent.RunAsync(message, cancellationToken: cancellationToken);

            return response.Text;
        }

        protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
        {
            protocolBuilder.ConfigureRoutes(
                builder => builder.AddHandler<ChatMessage, string>(HandleAsync));

            return protocolBuilder;
        }
    }
}
