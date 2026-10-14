using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents.Executors
{
    public class ScrapingExecutor(AIAgent _agent) : Executor("ScrapingExecutor")
    {
        public async ValueTask<string> HandleAsync(string url, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                return "Não encontrei nenhum anúncio correspondente no Mercado Livre.";

            var response = await _agent.RunAsync(
                new ChatMessage(ChatRole.User, url),
                cancellationToken: cancellationToken);

            return response.Text;
        }

        protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
        {
            protocolBuilder.ConfigureRoutes(
                builder => builder.AddHandler<string, string>(HandleAsync));

            return protocolBuilder;
        }
    }
}
