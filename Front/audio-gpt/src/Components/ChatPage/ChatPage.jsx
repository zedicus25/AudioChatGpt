import token from './../../jwtToken'
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import ChatNavBar from '../ChatNavBar/ChatNavBar';
import ChatComponent from '../ChatComponent/ChatComponent';

const ChatPage = () => {

    const navigate = useNavigate();


    useEffect(() => {
        const res = token.getUserData();
        if(res.UserFree === false)
          navigate('/');
      });

      return(
        <div style={{display:'flex', flexDirection:"row"}}>
        <ChatNavBar></ChatNavBar>
        <ChatComponent></ChatComponent>
        </div>
        
    );
}

export default ChatPage;