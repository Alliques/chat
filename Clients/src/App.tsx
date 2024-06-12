import React from "react";
import { BrowserRouter as Router, Route, Routes, Link } from "react-router-dom";
import Recipient from "./clients/recipient";
import Sender from "./clients/sender";
import MessagesHistory from "./clients/messagesHistory";

const App: React.FC = () => {
  return (
    <Router>
      <div>
        <nav>
          <ul>
            <li>
              <Link to='/sender'>Sender</Link>
            </li>
            <li>
              <Link to='/recipient'>Recipient</Link>
            </li>
            <li>
              <Link to='/message-history'>Messages History</Link>
            </li>
          </ul>
        </nav>

        <Routes>
          <Route path='/sender' Component={Sender} />
          <Route path='/recipient' Component={Recipient} />
          <Route path='/message-history' Component={MessagesHistory} />
        </Routes>
      </div>
    </Router>
  );
};

export default App;
