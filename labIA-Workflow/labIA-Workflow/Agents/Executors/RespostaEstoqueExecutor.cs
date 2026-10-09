using labIA_Workflow.Models.Dto;
using Microsoft.Agents.AI.Workflows;

namespace labIA_Workflow.Agents.Executors
{
    public class RespostaEstoqueExecutor() : Executor("RespostaEstoqueExecutor")
    {
        public ValueTask<string> HandleAsync(DisponibilidadeResultado resultado, IWorkflowContext context, CancellationToken cancellationToken = default)
            => ValueTask.FromResult(resultado.RespostaEstoque);

        protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
        {
            protocolBuilder.ConfigureRoutes(
                builder => builder.AddHandler<DisponibilidadeResultado, string>(HandleAsync));

            return protocolBuilder;
        }
    }
}
