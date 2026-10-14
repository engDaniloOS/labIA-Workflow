using labIA_Workflow.Configs;
using labIA_Workflow.Models.Dto;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents.Executors
{
    public class SessaoExecutor(AIAgent _agent, ChatSession _session) : Executor("SessaoExecutor")
    {
        public async ValueTask<MemoriaResultado> HandleAsync(ChatMessage message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var response = await _agent.RunAsync(_session.Messages, cancellationToken: cancellationToken);

            var resposta = response.Text;
            var encontrado = !string.IsNullOrWhiteSpace(resposta)
                && !resposta.Contains("NAO_ENCONTRADO", StringComparison.OrdinalIgnoreCase);

            return new MemoriaResultado(encontrado, resposta, message);
        }

        protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
        {
            protocolBuilder.ConfigureRoutes(
                builder => builder.AddHandler<ChatMessage, MemoriaResultado>(HandleAsync));

            return protocolBuilder;
        }
    }
}
