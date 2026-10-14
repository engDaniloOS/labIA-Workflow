using Microsoft.Extensions.AI;

namespace labIA_Workflow.Models.Dto
{
    public record MemoriaResultado(bool Encontrado, string Resposta, ChatMessage Pergunta);
}
