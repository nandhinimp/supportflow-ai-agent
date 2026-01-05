# LEARN.md  
## Hackathon 3 – Applied AI Agents  
### SupportFlow (Track 4: .NET)

---

## What I Studied

In this project, I explored how an AI system can **take actions**, not just answer questions.  
The focus was on building an **AI agent** that can:
- Understand user intent
- Decide what to do
- Call backend functions
- Use stored knowledge
- Remember past conversation context

---

## Main Question

Can an AI behave like a real support agent instead of a basic chatbot?

---

## What I Built

I built **SupportFlow**, an AI-powered support agent that:
- Answers policy questions using stored documents
- Fetches live order data using backend functions
- Combines both answers into one response
- Escalates to a human when needed

---

## What I Learned

### 1. Function Calling
AI should not guess real data.  
Instead, it should **call backend functions** to fetch correct information.

---

### 2. Tool Selection
The AI learns when to:
- Search documents (RAG)
- Call a database function
- Escalate the issue

This decision-making is what makes it an **agent**.

---

### 3. Hybrid Knowledge Use
Different data needs different sources:
- Policies → Document search
- Orders → Database queries

Using both together gives better results.

---

### 4. Conversation Memory
Storing past messages allows the AI to:
- Remember order numbers
- Answer follow-up questions naturally

---

### 5. Knowing When to Stop
A good agent knows its limits.  
If data is missing or the user is upset, it escalates to a human.

---

## Key Takeaway

This project helped me understand that:
> AI agents are systems that **decide and act**, not just generate text.

---

## Future Improvements

- Better sentiment detection
- More testing for backend tools
- Confidence scoring for responses

---
