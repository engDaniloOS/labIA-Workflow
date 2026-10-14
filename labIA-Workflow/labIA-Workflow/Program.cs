using labIA_Workflow;
using labIA_Workflow.Agents;
using labIA_Workflow.Configs;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var chat = ChatClientFactory.BuildClient();
        var session = new ChatSession();
        var workflowFactory = new WorkflowFactory(chat, session);

        Console.WriteLine("Gostaria de saber algo sobre algum item do estoque?");

        while (true)
        {
            string command = Console.ReadLine() ?? "";

            if (IsCommandEmpty(command))
                continue;

            if (IsCommandToQuit(command))
                break;

            session.AddUserMessage(command);

            var userMessage = new ChatMessage(ChatRole.User, command);

            await using var run = 
                await InProcessExecution.RunStreamingAsync(workflowFactory.BuildWorkflow(), userMessage, cancellationToken: default);

            await run.TrySendMessageAsync(new TurnToken(emitEvents: true));

            var responseBuilder = new StringBuilder();

            await foreach (var evt in run.WatchStreamAsync(cancellationToken: default))
            {
                if (evt is RequestInfoEvent requestEvent)
                    await Flows.RequestInfoFlow(run, requestEvent);

                else if (evt is AgentResponseUpdateEvent updateEvent)
                    Flows.UpdateEventFlow(responseBuilder, updateEvent);

                else if (evt is WorkflowOutputEvent outputEvent
                    && outputEvent.ExecutorId is not ("RespostaEstoqueExecutor" or "ScrapingExecutor"))
                    Flows.RawAnswerFlow(responseBuilder, outputEvent);
            }

            Console.WriteLine();

            var responseText = responseBuilder.Length > 0
                ? responseBuilder.ToString()
                : "Não encontrei resultados. Tente novamente.";

            session.AddAssistantMessage(responseText);
        }
    }

    private static bool IsCommandEmpty(string command) => string.IsNullOrWhiteSpace(command);

    private static bool IsCommandToQuit(string command)
    {
        return string.Equals(command, "exit", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(command, "sair", StringComparison.OrdinalIgnoreCase);
    }
}