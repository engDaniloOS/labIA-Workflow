using labIA_Workflow.Models.Dto;
using Microsoft.Agents.AI.Workflows;

namespace labIA_Workflow.Agents.Executors
{
    public class RespostaNaoEncontradoExecutor() : Executor("RespostaNaoEncontradoExecutor")
    {
        public ValueTask<string> HandleAsync(ConfirmacaoBusca resultado, IWorkflowContext context, CancellationToken cancellationToken = default)
            => ValueTask.FromResult("Ok, não vou buscar no Mercado Livre.");

        protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
        {
            protocolBuilder.ConfigureRoutes(
                builder => builder.AddHandler<ConfirmacaoBusca, string>(HandleAsync));

            return protocolBuilder;
        }
    }
}
