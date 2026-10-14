using System.Text;
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

            var textoResposta = new StringBuilder();

            await foreach (var update in _agent.RunStreamingAsync(new ChatMessage(ChatRole.User, url), cancellationToken: cancellationToken))
            {
                await context.AddEventAsync(new AgentResponseUpdateEvent(Id, update), cancellationToken);
                textoResposta.Append(update.Text);
            }

            return textoResposta.ToString();
        }

        protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
        {
            protocolBuilder.ConfigureRoutes(
                builder => builder.AddHandler<string, string>(HandleAsync));

            return protocolBuilder;
        }
    }
}
