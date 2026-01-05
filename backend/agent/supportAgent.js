const getOrderStatus = require("../tools/getOrderStatus");

// This function acts like the "AI brain"
const supportAgent = async (userMessage) => {
  // 1️⃣ Check if message contains an order number
  const orderIdMatch = userMessage.match(/\d+/);

  // 2️⃣ If order number exists → call tool
  if (orderIdMatch) {
    const orderId = orderIdMatch[0];

    const orderResult = await getOrderStatus(orderId);

    return {
      type: "order",
      data: orderResult
    };
  }

  // 3️⃣ Otherwise → normal message
  return {
    type: "general",
    message: "This looks like a general question. I will answer using documents."
  };
};

module.exports = supportAgent;
