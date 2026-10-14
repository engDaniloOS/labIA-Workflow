using labIA_Workflow.Agents;
using labIA_Workflow.Agents.Executors;
using labIA_Workflow.Configs;
using labIA_Workflow.Models.Dto;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var chat = ChatClientFactory.BuildClient();

        var session = new ChatSession();

        var sessaoExecutor = new SessaoExecutor(SessaoAgent.Create(chat), session);
        var respostaMemoriaExecutor = new RespostaMemoriaExecutor();
        var estoqueExecutor = new EstoqueExecutor(EstoqueAgent.Create(chat));
        var respostaEstoqueExecutor = new RespostaEstoqueExecutor();
        var webSearchExecutor = new WebSearchExecutor(WebSearchEstoqueAgent.Create(chat));
        var scrapingExecutor = new ScrapingExecutor(ScrapingAgent.Create(chat));

        Workflow BuildWorkflow() =>
            new WorkflowBuilder(sessaoExecutor)
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
                    webSearchExecutor,
                    condition: (DisponibilidadeResultado? resultado) => resultado?.Disponivel == false)
                .AddEdge(webSearchExecutor, scrapingExecutor)
                .WithOutputFrom([respostaMemoriaExecutor, respostaEstoqueExecutor, scrapingExecutor])
                .Build();

        Console.WriteLine("Gostaria de saber algo sobre algum item do estoque?");

        while (true)
        {
            string command = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(command))
                continue;

            if (string.Equals(command, "exit", StringComparison.OrdinalIgnoreCase)
                || string.Equals(command, "sair", StringComparison.OrdinalIgnoreCase))
                break;

            session.AddUserMessage(command);

            var userMessage = new ChatMessage(ChatRole.User, command);

            await using var run = 
                await InProcessExecution.RunStreamingAsync(
                    BuildWorkflow(), 
                    userMessage, 
                    cancellationToken: default);

            await run.TrySendMessageAsync(new TurnToken(emitEvents: true));

            var responseBuilder = new StringBuilder();

            await foreach (var evt in run.WatchStreamAsync(cancellationToken: default))
            {
                if (evt is AgentResponseUpdateEvent updateEvent)
                {
                    Console.Write(updateEvent.Update.Text);
                    responseBuilder.Append(updateEvent.Update.Text);
                }
                // Ramos sem streaming (ex: resposta vinda da memória da sessão) não emitem
                // AgentResponseUpdateEvent — usamos o WorkflowOutputEvent "puro" como fallback,
                // só quando nada foi streamado ainda, pra não duplicar o texto nos ramos que já streamam.
                else if (evt is WorkflowOutputEvent outputEvent && responseBuilder.Length == 0)
                {
                    var texto = outputEvent.As<string>();
                    Console.Write(texto);
                    responseBuilder.Append(texto);
                }
            }

            Console.WriteLine();

            var responseText = responseBuilder.Length > 0
                ? responseBuilder.ToString()
                : "Não encontrei resultados. Tente novamente.";

            session.AddAssistantMessage(responseText);
        }
    }
}