import ".././styles/chat.css";

const ChatComponent = () => {

  return (
    <div className="message-container">
      <div className="header-chat">Чат</div>
      <div className="message-box">тут типо сообщения будут</div>
      <div className="input-container">
        <textarea  type="text" placeholder="Введите сообщение" className="input-field"/>
        <button className="send-button">&#128172;</button>
      </div>
    </div>
  );
};

export default ChatComponent;
