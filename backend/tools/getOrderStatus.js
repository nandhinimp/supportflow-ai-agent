
// This tool fetches live order status from the database
// It is called by the AI agent (not directly by users)

const getOrderStatus = async (orderId) => {
  // 1️⃣ Validate input
  if (!orderId) {
    throw new Error("Order ID is required");
  }

  // 2️⃣ TEMP mock data (we will connect DB later)
  const mockOrders = [
    { orderId: 9987, status: "Out for Delivery" },
    { orderId: 9988, status: "Delivered" },
    { orderId: 9989, status: "Processing" }
  ];

  // 3️⃣ Find order
  const order = mockOrders.find(
    (o) => o.orderId === Number(orderId)
  );

  // 4️⃣ If order not found
  if (!order) {
    return {
      success: false,
      message: "Order not found"
    };
  }

  // 5️⃣ Return clean result
  return {
    success: true,
    orderId: order.orderId,
    status: order.status
  };
};

module.exports = getOrderStatus;
