using System.Collections.Concurrent;

namespace SupportFlow.Api.Services
{
    public class ConversationMemory
    {
        private readonly ConcurrentDictionary<string, List<string>> _memory = new();

        public void AddMessage(string threadId, string message)
        {
            _memory.AddOrUpdate(
                threadId,
                new List<string> { message },
                (_, existing) =>
                {
                    existing.Add(message);
                    return existing;
                }
            );
        }

        public string GetHistory(string threadId)
        {
            if (_memory.TryGetValue(threadId, out var messages))
            {
                return string.Join("\n", messages);
            }

            return "";
        }
    }
}
