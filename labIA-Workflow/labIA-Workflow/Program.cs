using labIA_Workflow.Agents;
using labIA_Workflow.Configs;

var chat = ChatClientFactory.BuildClient();
var agent = GeneralAgent.Create(chat);
var session = new ChatSession();

while (true)
{
    Console.WriteLine("Gostaria de saber algo sobre algum item do estoque?");
    string command = Console.ReadLine() ?? "";

    if (string.Equals(command, "exit", StringComparison.OrdinalIgnoreCase))
        break;

    session.AddUserMessage(command);
    var response = await agent.RunAsync(session.Messages);

    session.AddAssistantMessage(response.Text);

    Console.WriteLine(response.Text);
}