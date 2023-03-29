import './LandingNavBar.css'
import { Link } from 'react-router-dom';
import LoginModal from '../LoginModal/LoginModal';
import { useEffect, useState } from 'react';
import RegistrationModal from '../RegistrationModal/RegistrationModal';
import token from './../../jwtToken';
const LandingNavBar = () => {
    const [modalShow, setModalShow] =  useState(false);
    const [modalRegShow, setModalRegShow] =  useState(false);
    const [userName, setUserName] =  useState("");


    useEffect(() => {
        let res = token.getUserData();
        if(res !== undefined){
            setUserName(res.username);
        }
    }, []);

return(
    <header className="header">
        <div className='container'>
            <div className='flex-row'>
                <div className='col-lg-2'>
                    <div style={{padding: "30px 0px"}}>
                        <Link to='/'>
                            <img style={{width:180, height:40, filter:'invert(1)'}} src='https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/OpenAI_Logo.svg/1024px-OpenAI_Logo.svg.png'></img>
                        </Link>
                    </div>
                </div>
                <div className='col-lg-10'>
                    <div className='nav-container'>
                        <nav className='nav-menu'>
                            <ul>
                            {userName !== "" &&        
                            <li><Link to='/chat'>Chat</Link></li>     }
                            </ul>
                        </nav>
                    </div>
                    <div className='control-btn'>
                            <Link className='control-btn-a'>{userName}</Link>
                            <Link className='control-btn-a' onClick={() => setModalShow(true)}>Login</Link>
                            <Link className='control-btn-a' onClick={() => setModalRegShow(true)}>Registration</Link>
                        </div>
                </div>
                
            </div>
        </div>
        <LoginModal show={modalShow}
    onHide={() =>{ setModalShow(false);
        let res = token.getUserData();
        if(res !== undefined){
            setUserName(res.username);
        }}}></LoginModal>
    <RegistrationModal show={modalRegShow}
    onHide={() => setModalRegShow(false)}></RegistrationModal>
    </header>
);
}

export default LandingNavBar;