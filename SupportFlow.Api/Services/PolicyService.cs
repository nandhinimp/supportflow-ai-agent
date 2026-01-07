namespace SupportFlow.Api.Services
{
    public class PolicyService
    {
        private const string ReturnPolicy = """
        Our return policy allows customers to return products within 30 days of delivery.
        Items must be unused, in original packaging, and include the receipt.
        Refunds are processed within 5 business days.
        """;

        public string GetReturnPolicy()
        {
            return ReturnPolicy;
        }
    }
}
