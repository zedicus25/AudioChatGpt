import './ChatNavBar.css'
import token from './../../jwtToken'
import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import api from './../../apiAccess';

const ChatNavBar = () => {

    const navigate = useNavigate();
    const [selectedSubscription, setSelectedSubsription] = useState();
    const [userData, setUserData] = useState({});

    useEffect(() => {
        setUserData(token.getUserData());
    }, []);
    const logOut = () => {
        token.logOut();
        navigate('/');
    }

    const updateSubscription = async () => {
        let res = await api.upadteSubscriptions(selectedSubscription);
        if (res.status == 200) {
            alert("You subscription was updated! You need re-authorise!");
            logOut();
        }

    }
    return (
        <div className='nav-wrap'>
            <div className="manager-nav">
                <div className="form-group mt-3">
                    <label style={{ color: '#93B7BE', margin: 10 }} >Select subscription</label>
                    <select onChange={(e) => setSelectedSubsription(e.target.value)} style={{ backgroundColor: '#93B7BE', width: 190, margin: 10 }} className="form-select" id='categoryDrop'>
                        <option defaultValue>Select</option>
                        <option value='1'>Free</option>
                        <option value='2'>Free Plus</option>
                        <option value='3'>Plus</option>
                        <option value='4'>Premium</option>
                    </select>
                </div>
                <input onClick={() => updateSubscription()} className='logout-btn' type='button' value='Upgrade'></input>
            </div>
            {
                userData.UserPlus &&
                <input onClick={() => navigate('/chat/history')} className='logout-btn' type='button' value='History'></input>
            }
            
            <div className="manager-nav">
                <input onClick={() => navigate('/')} className='logout-btn' type='button' value='Main'></input>
                <input onClick={() => logOut()} className='logout-btn' type='button' value='Exit'></input>
            </div>
        </div>
    );
}

export default ChatNavBar;