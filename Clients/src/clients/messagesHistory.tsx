import React, { useState, useEffect } from "react";

interface Message {
  message: string;
  date: string;
}

const MessagesHistory: React.FC = () => {
  const [messages, setMessages] = useState<Message[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchMessages = async () => {
      const end = new Date();
      const start = new Date(end.getTime() - 10 * 60 * 1000); // 10 минут назад
      const startISOString = start.toISOString();
      const endISOString = end.toISOString();

      try {
        const response = await fetch(
          `http://localhost:5090/api/messages?start=${startISOString}&end=${endISOString}`
        );
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        const data: Message[] = await response.json();
        setMessages(data);
      } catch (error) {
        setError((error as Error).message);
      } finally {
        setLoading(false);
      }
    };

    fetchMessages();
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div>
      <h1>Messages from the last 10 minutes</h1>
      {messages.length === 0 ? (
        <p>No messages found</p>
      ) : (
        <ul>
          {messages.map((message, index) => (
            <li key={index}>
              <p>{message.message}</p>
              <p>
                <em>{new Date(message.date).toLocaleString()}</em>
              </p>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default MessagesHistory;
