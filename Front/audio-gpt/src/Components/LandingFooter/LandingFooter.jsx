import './LandingFooter.css'
import { Link } from 'react-router-dom';
const LandingFooter = () => {
    return (
        <footer className="footer">
            <div className="container">
                <div className="footer-top">
                    <div className="flex-row">
                        <div className="footer-top-container">
                            <div style={{ padding: "30px 0px" }}>
                                <Link to='/'>
                                    <img style={{ width: 180, height: 40, filter:'invert(1)' }} src='https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/OpenAI_Logo.svg/1024px-OpenAI_Logo.svg.png'></img>
                                </Link>
                            </div>
                        </div>
                        <div className="footer-top-container"><p>Made by Zedicus</p></div>
                    </div>
                </div>
                <div className='footer-content'>
                    <div className='container'>
                        <div className='footer-about'>
                                <h5>About</h5>
                                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor 
                                    incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis 
                                    nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. </p>
                        </div>
                    </div>
                    <div className='container'>
                        <div className='footer-contacts'>
                            <h5>Who we are</h5>
                            <ul>
                                <li>Team</li>
                                <li>Carrers</li>
                                <li>Location</li>
                                <li>Contact us</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </footer>
    )
}

export default LandingFooter;