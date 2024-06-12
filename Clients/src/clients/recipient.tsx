import React, { useEffect, useState } from "react";

const Recipient: React.FC = () => {
  const [messages, setMessages] = useState<string[]>([]);

  useEffect(() => {
    const ws = new WebSocket("ws://localhost:5090/ws");
    let cleanup = false;

    ws.onopen = () => {
      console.log("WebSocket connection opened.");
    };

    ws.onmessage = (event) => {
      if (!cleanup) {
        setMessages((prevMessages) => [...prevMessages, event.data]);
      }
    };

    ws.onerror = (error) => {
      console.error("WebSocket error:", error);
    };

    ws.onclose = (event) => {
      console.log(`WebSocket closed: ${event.code}, reason: ${event.reason}`);
    };

    return () => {
      cleanup = true;
      if (
        ws.readyState === WebSocket.OPEN ||
        ws.readyState === WebSocket.CONNECTING
      ) {
        ws.close(1000, "Component unmounted"); // Normal closure
      }
    };
  }, []);

  return (
    <div>
      <h2>Recipient</h2>
      <ul>
        {messages.map((message, index) => (
          <li key={index}>{message}</li>
        ))}
      </ul>
    </div>
  );
};

export default Recipient;
