import LandingFooter from '../LandingFooter/LandingFooter';
import LandingNavBar from '../LandingNavBar/LandingNavBar';
import LandinfSubscriptions from '../LandingSubscriptions/LandingSubsriptions';
import LandingMainInfo from '../LandinMainInfo/LandingMainInfo';
import './LandingPage.css'

const LandingPage = () => {
    return(
        <div>
            <LandingNavBar></LandingNavBar>
            <LandingMainInfo></LandingMainInfo>
            <LandinfSubscriptions></LandinfSubscriptions>
            <LandingFooter></LandingFooter>
        </div>
    );
}

export default LandingPage;