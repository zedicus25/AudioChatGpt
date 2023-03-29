import { useEffect, useState } from "react";
import ChatNavBar from "../ChatNavBar/ChatNavBar";
import api from '../../apiAccess';
import token from './../../jwtToken'
import { useNavigate } from "react-router-dom";
import './HistoryPage.css'
const HistoryPage = () => {

    useEffect(() => {
        const res = token.getUserData();
        if(res.UserPlus === false)
          navigate('/');
        api.getHistory().then((res) => {
            setRequests(res.request);
            setResponces(res.response);
        });
    }, []);
    const [requests, setRequests] = useState([]);
    const [response, setResponces] = useState([]);
    const navigate = useNavigate();

    

    return(
        <div>
            <ChatNavBar></ChatNavBar>
            <div style={{ zIndex: -5, height: "100%", position: 'fixed', width: "100%", backgroundColor: "#93B7BE" }}>
                <div style={{ marginLeft: 220 }}>{requests.map((x, idx) => {
                    return <div className="block-res" key={`div-${idx}`}>
                        <div className="p-req" key={`requ-${idx}`}>{x}</div>
                        <p className="p-res" key={`res-${idx}`}>{response[idx]}</p>
                    </div>
                })}</div>
            </div>
        </div>
    )
}

export default HistoryPage;