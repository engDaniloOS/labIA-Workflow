using labIA_Workflow.Models.Dto;
using Microsoft.Agents.AI.Workflows;
using System.Text;

namespace labIA_Workflow
{
    public class Flows
    {
        public static void RawAnswerFlow(StringBuilder responseBuilder, WorkflowOutputEvent outputEvent)
        {
            var texto = outputEvent.As<string>();

            Console.Write(texto);
            responseBuilder.Append(texto);
        }

        public static void UpdateEventFlow(StringBuilder responseBuilder, AgentResponseUpdateEvent updateEvent)
        {
            var texto = updateEvent.Update.Text;

            Console.Write(texto);
            responseBuilder.Append(texto);
        }

        public static async Task RequestInfoFlow(StreamingRun run, RequestInfoEvent requestEvent)
        {
            requestEvent.Request.TryGetDataAs<DisponibilidadeResultado>(out var contexto);

            Console.Write("Deseja que eu busque esse produto no Mercado Livre? (s/n): ");

            var resposta = Console.ReadLine() ?? "";

            var confirmado =
                string.Equals(resposta, "s", StringComparison.OrdinalIgnoreCase)
                || string.Equals(resposta, "sim", StringComparison.OrdinalIgnoreCase);

            var respostaHumano = requestEvent.Request.CreateResponse(
                new ConfirmacaoBusca(confirmado, contexto?.RespostaEstoque ?? ""));

            await run.SendResponseAsync(respostaHumano);
        }
    }
}
