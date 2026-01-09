export async function sendMessage(message, threadId) {
  const response = await fetch("http://localhost:5084/api/chat", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      message,
      threadId
    })
  });

  return response.json();
}
