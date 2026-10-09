using labIA_Workflow.Agents;
using labIA_Workflow.Agents.Executors;
using labIA_Workflow.Configs;
using labIA_Workflow.Models.Dto;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var chat = ChatClientFactory.BuildClient();

        var estoqueExecutor = new EstoqueExecutor(EstoqueAgent.Create(chat));
        var disponibilidadeExecutor = new DisponibilidadeExecutor(AnaliseDisponibilidadeAgent.Create(chat));
        var respostaEstoqueExecutor = new RespostaEstoqueExecutor();
        var webSearchExecutor = new WebSearchExecutor(WebSearchEstoqueAgent.Create(chat));

        Workflow BuildWorkflow() =>
            new WorkflowBuilder(estoqueExecutor)
                .AddEdge(
                    estoqueExecutor,
                    disponibilidadeExecutor)
                .AddEdge(
                    disponibilidadeExecutor,
                    respostaEstoqueExecutor,
                    condition: (DisponibilidadeResultado? resultado) => resultado?.Disponivel == true)
                .AddEdge(
                    disponibilidadeExecutor,
                    webSearchExecutor,
                    condition: (DisponibilidadeResultado? resultado) => resultado?.Disponivel == false)
                .WithOutputFrom([respostaEstoqueExecutor, webSearchExecutor])
                .Build();

        var session = new ChatSession();

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
            var run = await InProcessExecution.RunAsync(BuildWorkflow(), userMessage, cancellationToken: default);

            var responseText = run.NewEvents
                .OfType<WorkflowOutputEvent>()
                .Select(e => e.As<string>())
                .FirstOrDefault() ?? "Não encontrei resultados. Tente novamente.";

            session.AddAssistantMessage(responseText);

            Console.WriteLine(responseText);
        }
    }
}