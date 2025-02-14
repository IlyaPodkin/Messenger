import ".././styles/chat.css";
import axios from 'axios';
import React, { useEffect, useState } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";

const ChatComponent = () => {
  const ServerUrl = "http://localhost:5218/api/Message/message";
  const [messages, setMessage] = useState([]);
  const [newMessages, setNewMessage] = useState("");
  const [, setConnection] = useState(null);

  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl("http://localhost:5218/messageHub")
      .withAutomaticReconnect()
      .build();

    setConnection(connection);

    connection
      .start()
      .then(() => {
        console.log("SignalR подключен");
        connection.invoke("GetOldMessages");
      })
      .catch((error) => console.error("Ошибка подключения SignalR:", error));

      connection.on("ReceiveMessage", (user, message, timeStamp) => {
        const formattedDate = formatDate(timeStamp); 
        setMessage((prevMessages) => [
          ...prevMessages,
          { userName: user, text: message, timeStamp: formattedDate },
        ]);
      });
      

    connection.on("ReceiveOldMessages", (oldMessages) => {
      console.log("Получены старые сообщения:", oldMessages);
      setMessage(oldMessages);
    });

    return () => {
      connection.stop();
    };
  }, []);

  const handleSendMessage = () => {
    if (newMessages.trim()) {
      const currentTime = new Date().toISOString(); 
      axios
        .post(ServerUrl, { content: newMessages, userName: "user", timeStamp: currentTime })
        .then((response) => {
          console.log(`Данные добавлены:`, response.data);
          setNewMessage("");
        })
        .catch((error) => {
          console.error(`Ошибка при запросе:`, error);
        });
    }
  };

  const formatDate = (timestamp) => {
    const date = new Date(timestamp);
    if (isNaN(date.getTime())) return "Invalid Date"; 
    return `${date.getFullYear()}-${(date.getMonth() + 1)
      .toString()
      .padStart(2, "0")}-${date
      .getDate()
      .toString()
      .padStart(2, "0")} ${date
      .getHours()
      .toString()
      .padStart(2, "0")}:${date
      .getMinutes()
      .toString()
      .padStart(2, "0")}:${date
      .getSeconds()
      .toString()
      .padStart(2, "0")}`;
  };
  

  return (
    <div className="message-container">
      <div className="header-chat">Чат</div>
      <div className="message-box">
        {messages.map((message, index) => (
          <div key={index} className="message">
            {message.userName}: {message.text}
            <p className="message-time">{formatDate(message.timeStamp)}</p>
          </div>
        ))}
      </div>
      <div className="input-container">
        <textarea
          type="text"
          placeholder="Введите сообщение"
          className="input-field"
          value={newMessages}
          onChange={(e) => setNewMessage(e.target.value)}
        />
        <button onClick={handleSendMessage} className="send-button">
          &#128172;
        </button>
      </div>
    </div>
  );
};

export default ChatComponent;
