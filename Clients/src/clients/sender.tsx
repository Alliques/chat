import React, { useState } from "react";

// Интерфейс для DTO
interface MessageCreationDto {
  message: string;
}

const SendMessage: React.FC = () => {
  const [message, setMessage] = useState<string>("");
  const [response, setResponse] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const request: MessageCreationDto = { message };

    try {
      const res = await fetch("http://localhost:5090/api/messages/send", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(request),
      });

      if (!res.ok) {
        throw new Error("Network response was not ok");
      }

      const data = await res.json();
      setResponse(data.Message);
      setMessage("");
    } catch (error) {
      setError((error as Error).message);
    }
  };

  return (
    <div>
      <h1>Send a Message</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor='message'>Message:</label>
          <input
            type='text'
            id='message'
            value={message}
            onChange={(e) => setMessage(e.target.value)}
          />
        </div>
        <button type='submit'>Send</button>
      </form>
      {response && <p>{response}</p>}
      {error && <p>Error: {error}</p>}
    </div>
  );
};

export default SendMessage;
