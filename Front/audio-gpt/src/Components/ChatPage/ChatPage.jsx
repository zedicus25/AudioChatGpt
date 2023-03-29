import token from './../../jwtToken'
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import ChatNavBar from '../ChatNavBar/ChatNavBar';

const ChatPage = () => {

    const navigate = useNavigate();


    useEffect(() => {
        const res = token.getUserData();
        if(res.UserFree === false)
          navigate('/');
      });

      return(
        <ChatNavBar></ChatNavBar>
    );
}

export default ChatPage;