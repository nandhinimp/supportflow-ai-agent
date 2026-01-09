import { useState, useRef, useEffect } from "react";
import { sendMessage } from "../services/api";

export default function ChatBox() {
  const [messages, setMessages] = useState([]);
  const [input, setInput] = useState("");
  const [loading, setLoading] = useState(false);
  const [threadId] = useState(() => crypto.randomUUID());
  const chatEndRef = useRef(null);

  const scrollToBottom = () => {
    chatEndRef.current?.scrollIntoView({ behavior: "smooth" });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  const handleSend = async () => {
    if (!input.trim() || loading) return;

    const userMsg = { role: "user", text: input };
    setMessages(prev => [...prev, userMsg]);
    setInput("");
    setLoading(true);

    try {
      const response = await sendMessage(input, threadId);

      const botMsg = {
        role: "bot",
        text: response.reply,
        escalation: response.escalationSummary
      };

      setMessages(prev => [...prev, botMsg]);
    } catch (error) {
      const errorMsg = {
        role: "bot",
        text: "Sorry, I'm having trouble connecting. Please try again.",
        isError: true
      };
      setMessages(prev => [...prev, errorMsg]);
    } finally {
      setLoading(false);
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      handleSend();
    }
  };

  return (
    <div style={styles.container}>
      <div style={styles.header}>
        <h1 style={styles.title}>SupportFlow</h1>
      </div>

      <div style={styles.chat}>
        {messages.length === 0 && (
          <div style={styles.emptyState}>
            <div style={styles.emptyContent}>
              <div style={styles.logoCircle}>💬</div>
              <h2 style={styles.welcomeTitle}>Welcome to SupportFlow</h2>
              <p style={styles.welcomeText}>Your AI-powered customer support assistant</p>
              <p style={styles.welcomeSubtext}>Ask me anything about orders, returns, or products!</p>
            </div>
          </div>
        )}

        {messages.map((m, i) => (
          <div
            key={i}
            style={{
              ...styles.messageWrapper,
              justifyContent: m.role === "user" ? "flex-end" : "flex-start"
            }}
          >
            {m.role === "bot" && (
              <div style={styles.avatar}>🤖</div>
            )}
            <div
              style={{
                ...styles.message,
                background: m.role === "user" 
                  ? "linear-gradient(135deg, #667eea 0%, #764ba2 100%)" 
                  : m.isError 
                    ? "#fee" 
                    : "#fff",
                color: m.role === "user" ? "#fff" : "#333",
                boxShadow: "0 2px 10px rgba(0,0,0,0.08)",
                border: m.role === "bot" ? "1px solid #e8e8e8" : "none"
              }}
            >
              <p style={styles.messageText}>{m.text}</p>
              {m.escalation && (
                <div style={styles.escalation}>
                  <strong style={styles.escalationTitle}>⚠️ Escalation Summary:</strong>
                  <pre style={styles.escalationText}>
                    {m.escalation}
                  </pre>
                </div>
              )}
            </div>
            {m.role === "user" && (
              <div style={styles.avatar}>👤</div>
            )}
          </div>
        ))}

        {loading && (
          <div style={styles.messageWrapper}>
            <div style={styles.avatar}>🤖</div>
            <div style={styles.loadingMessage}>
              <div style={styles.typingIndicator}>
                <span style={styles.dot}></span>
                <span style={styles.dot}></span>
                <span style={styles.dot}></span>
              </div>
            </div>
          </div>
        )}
        <div ref={chatEndRef} />
      </div>

      <div style={styles.inputArea}>
        <input
          value={input}
          onChange={e => setInput(e.target.value)}
          onKeyPress={handleKeyPress}
          placeholder="Type your message..."
          style={styles.input}
          disabled={loading}
        />
        <button 
          onClick={handleSend} 
          style={{
            ...styles.button,
            opacity: loading || !input.trim() ? 0.5 : 1,
            cursor: loading || !input.trim() ? "not-allowed" : "pointer"
          }}
          disabled={loading || !input.trim()}
        >
          {loading ? "..." : "Send"}
        </button>
      </div>
    </div>
  );
}

const styles = {
  container: { 
    width: "100%", 
    height: "100vh",
    margin: 0,
    fontFamily: "'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif",
    display: "flex",
    flexDirection: "column",
    overflow: "hidden",
    background: "linear-gradient(to bottom, #f9fafb 0%, #ffffff 100%)"
  },
  header: {
    background: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
    padding: "20px",
    color: "#fff",
    textAlign: "center",
    flexShrink: 0,
    boxShadow: "0 2px 10px rgba(0,0,0,0.1)"
  },
  title: {
    margin: 0,
    fontSize: 28,
    fontWeight: 700,
    letterSpacing: "0.5px"
  },
  chat: {
    flex: 1,
    padding: "24px",
    display: "flex",
    flexDirection: "column",
    overflowY: "auto",
    gap: "16px",
    maxWidth: "1200px",
    width: "100%",
    margin: "0 auto"
  },
  emptyState: {
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    flex: 1
  },
  emptyContent: {
    textAlign: "center",
    maxWidth: "500px",
    padding: "40px"
  },
  logoCircle: {
    width: "80px",
    height: "80px",
    margin: "0 auto 24px",
    background: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
    borderRadius: "50%",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    fontSize: "40px",
    boxShadow: "0 4px 20px rgba(102, 126, 234, 0.4)"
  },
  welcomeTitle: {
    margin: "0 0 12px",
    fontSize: 32,
    fontWeight: 700,
    background: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
    WebkitBackgroundClip: "text",
    WebkitTextFillColor: "transparent",
    backgroundClip: "text"
  },
  welcomeText: {
    margin: "0 0 8px",
    fontSize: 18,
    color: "#666",
    fontWeight: 500
  },
  welcomeSubtext: {
    margin: 0,
    fontSize: 14,
    color: "#999"
  },
  messageWrapper: {
    display: "flex",
    alignItems: "flex-end",
    gap: 12,
    maxWidth: "800px",
    width: "100%",
    margin: "0 auto"
  },
  avatar: {
    width: 36,
    height: 36,
    borderRadius: "50%",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    fontSize: 20,
    flexShrink: 0,
    background: "#f0f0f0"
  },
  message: {
    padding: "12px 18px",
    borderRadius: 18,
    maxWidth: "calc(100% - 60px)",
    wordWrap: "break-word",
    lineHeight: 1.5
  },
  messageText: {
    margin: 0,
    fontSize: 15
  },
  escalation: {
    marginTop: 12,
    background: "#fff9e6",
    padding: 14,
    borderRadius: 10,
    fontSize: 13,
    border: "1px solid #ffe58f"
  },
  escalationTitle: {
    display: "block",
    marginBottom: "8px",
    color: "#d48806"
  },
  escalationText: {
    margin: 0,
    whiteSpace: "pre-wrap",
    fontFamily: "'Courier New', monospace",
    fontSize: 12,
    color: "#595959"
  },
  loadingMessage: {
    background: "#fff",
    padding: "12px 18px",
    borderRadius: 18,
    border: "1px solid #e8e8e8",
    boxShadow: "0 2px 10px rgba(0,0,0,0.08)"
  },
  typingIndicator: {
    display: "flex",
    gap: 6,
    alignItems: "center"
  },
  dot: {
    width: 8,
    height: 8,
    borderRadius: "50%",
    background: "#999",
    animation: "pulse 1.4s infinite ease-in-out both"
  },
  inputArea: { 
    display: "flex", 
    padding: "20px",
    borderTop: "1px solid #e8e8e8",
    background: "#fff",
    gap: 12,
    flexShrink: 0,
    maxWidth: "1200px",
    width: "100%",
    margin: "0 auto",
    boxShadow: "0 -2px 10px rgba(0,0,0,0.05)"
  },
  input: { 
    flex: 1, 
    padding: "12px 20px",
    border: "2px solid #e8e8e8",
    borderRadius: 24,
    fontSize: 15,
    outline: "none",
    transition: "border-color 0.2s",
    fontFamily: "inherit"
  },
  button: { 
    padding: "12px 32px",
    background: "linear-gradient(135deg, #667eea 0%, #764ba2 100%)",
    color: "#fff",
    border: "none",
    borderRadius: 24,
    fontSize: 15,
    fontWeight: 600,
    transition: "all 0.2s",
    cursor: "pointer",
    boxShadow: "0 2px 8px rgba(102, 126, 234, 0.3)"
  }
};
