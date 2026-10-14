using labIA_Workflow.Agents.Agents;
using labIA_Workflow.Agents.Executors;
using labIA_Workflow.Configs;
using labIA_Workflow.Models.Dto;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

namespace labIA_Workflow.Agents
{
    public class WorkflowFactory
    {
        private readonly SessaoExecutor sessaoExecutor;
        private readonly RespostaMemoriaExecutor respostaMemoriaExecutor;
        private readonly EstoqueExecutor estoqueExecutor;
        private readonly RespostaEstoqueExecutor respostaEstoqueExecutor;
        private readonly RequestPort<DisponibilidadeResultado, ConfirmacaoBusca> confirmacaoBuscaPort;
        private readonly RespostaNaoEncontradoExecutor respostaNaoEncontradoExecutor;
        private readonly WebSearchExecutor webSearchExecutor;
        private readonly ScrapingExecutor scrapingExecutor;

        public WorkflowFactory(IChatClient chat, ChatSession session)
        {
            sessaoExecutor = new SessaoExecutor(SessaoAgent.Create(chat), session);
            respostaMemoriaExecutor = new RespostaMemoriaExecutor();
            estoqueExecutor = new EstoqueExecutor(EstoqueAgent.Create(chat));
            respostaEstoqueExecutor = new RespostaEstoqueExecutor();
            confirmacaoBuscaPort = RequestPort.Create<DisponibilidadeResultado, ConfirmacaoBusca>("ConfirmarBuscaMercadoLivre");
            respostaNaoEncontradoExecutor = new RespostaNaoEncontradoExecutor();
            webSearchExecutor = new WebSearchExecutor(WebSearchEstoqueAgent.Create(chat));
            scrapingExecutor = new ScrapingExecutor(ScrapingAgent.Create(chat));
        }

        public Workflow BuildWorkflow()
        {
            return new WorkflowBuilder(sessaoExecutor)
                .AddEdge(
                    sessaoExecutor,
                    respostaMemoriaExecutor,
                    condition: (MemoriaResultado? resultado) => resultado?.Encontrado == true)
                .AddEdge(
                    sessaoExecutor,
                    estoqueExecutor,
                    condition: (MemoriaResultado? resultado) => resultado?.Encontrado == false)
                .AddEdge(
                    estoqueExecutor,
                    respostaEstoqueExecutor,
                    condition: (DisponibilidadeResultado? resultado) => resultado?.Disponivel == true)
                .AddEdge(
                    estoqueExecutor,
                    confirmacaoBuscaPort,
                    condition: (DisponibilidadeResultado? resultado) => resultado?.Disponivel == false)
                .AddEdge(
                    confirmacaoBuscaPort,
                    webSearchExecutor,
                    condition: (ConfirmacaoBusca? resultado) => resultado?.Confirmado == true)
                .AddEdge(
                    confirmacaoBuscaPort,
                    respostaNaoEncontradoExecutor,
                    condition: (ConfirmacaoBusca? resultado) => resultado?.Confirmado == false)
                .AddEdge(
                    webSearchExecutor,
                    scrapingExecutor)
                .WithOutputFrom(
                [
                    respostaMemoriaExecutor,
                    respostaEstoqueExecutor,
                    respostaNaoEncontradoExecutor,
                    scrapingExecutor
                ])
                .Build();
        }
    }
}
