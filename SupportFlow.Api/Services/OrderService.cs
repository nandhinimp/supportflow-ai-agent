namespace SupportFlow.Api.Services
{
    public class OrderService
    {
        private static readonly Dictionary<int, string> Orders = new()
        {
            { 9987, "Out for Delivery" },
            { 1001, "Delivered" },
            { 1002, "Processing" }
        };

        public string GetOrderStatus(int orderId)
        {
            if (Orders.TryGetValue(orderId, out var status))
            {
                return status;
            }

            return "Order not found";
        }
    }
}
