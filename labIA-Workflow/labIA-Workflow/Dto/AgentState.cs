using Microsoft.Extensions.AI;

namespace labIA_Workflow.Dto
{
    public class AgentState
    {
        public string? Pergunta { get; set; }
        public List<ChatMessage>? Historico { get; set; }
        public string? AcaoPendente { get; set; }
        public string? RespostaFinal { get; set; }
    }
}