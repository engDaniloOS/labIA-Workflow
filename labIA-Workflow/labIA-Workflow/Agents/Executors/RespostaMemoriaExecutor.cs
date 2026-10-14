using labIA_Workflow.Models.Dto;
using Microsoft.Agents.AI.Workflows;

namespace labIA_Workflow.Agents.Executors
{
    public class RespostaMemoriaExecutor() : Executor("RespostaMemoriaExecutor")
    {
        public ValueTask<string> HandleAsync(MemoriaResultado resultado, IWorkflowContext context, CancellationToken cancellationToken = default)
            => ValueTask.FromResult(resultado.Resposta);

        protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
        {
            protocolBuilder.ConfigureRoutes(
                builder => builder.AddHandler<MemoriaResultado, string>(HandleAsync));

            return protocolBuilder;
        }
    }
}
