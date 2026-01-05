const express = require("express");
const dotenv = require("dotenv");
const supportAgent = require("./agent/supportAgent");

// 2️⃣ Load environment variables
dotenv.config();

// 3️⃣ Create express app
const app = express();

// 4️⃣ Middleware to read JSON from requests
app.use(express.json());

// 5️⃣ Health check route (important for testing)
app.get("/health", (req, res) => {
  res.json({ status: "SupportFlow backend is running" });
});

// 6️⃣ Port configuration
const PORT = process.env.PORT || 5000;

supportAgent("What is your return policy?")
  .then(console.log);

// 7️⃣ Start server
app.listen(PORT, () => {
  console.log(`🚀 Server running on port ${PORT}`);
});
