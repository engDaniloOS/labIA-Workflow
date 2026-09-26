using Microsoft.Extensions.AI;

namespace labIA_Workflow.Configs
{
    public class ChatSession
    {
        private const int historyWindowSize = 50;
        private readonly List<ChatMessage> _messages = [];

        public IReadOnlyList<ChatMessage> Messages => _messages;

        public void AddUserMessage(string text) => Add(ChatRole.User, text);

        public void AddAssistantMessage(string text) => Add(ChatRole.Assistant, text);

        private void Add(ChatRole role, string text)
        {
            _messages.Add(new ChatMessage(role, text));

            var excess = _messages.Count - historyWindowSize;
            if (excess > 0)
                _messages.RemoveRange(0, excess);
        }
    }
}
