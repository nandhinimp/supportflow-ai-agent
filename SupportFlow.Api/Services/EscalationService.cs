namespace SupportFlow.Api.Services
{
    public class EscalationService
    {
        public string CreateSummary(string userMessage, string reason)
        {
            return $"""
            Escalation Reason: {reason}

            User Message:
            {userMessage}

            Action Needed:
            Please review and assist the customer as soon as possible.
            """;
        }
    }
}
