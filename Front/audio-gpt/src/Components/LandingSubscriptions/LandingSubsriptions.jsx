import './LandinfSubscriptions.css'
import Card from '../Card/Card';

const LandinfSubscriptions = () => {
    return (
        <section className='subscriptions'>
            <div className='container'>
                <div className='row'>
                    <div className='col-4'>
                        <div>
                            <div style={{ marginBottom: 50 }}>
                                <h2 className='title'>Subscriptions</h2>
                            </div>
                            <p>
                                Lorem ipsum dolor sit amet, consectetur adipiscing elit,
                                sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                                Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                            </p>
                        </div>
                    </div>
                    <div className='col-8'>
                        <div className='flex-wrap'>
                            <Card icon='free.png' name="Free" description='Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor 
                                    incididunt ut labore et dolore magna aliqua.  Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'></Card>
                            <Card icon='freePlus.png' name="Free Plus" description='Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor 
                                    incididunt ut labore et dolore magna aliqua.  Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'></Card>
                            <Card icon='plus.png' name="Plus" description='Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor 
                                    incididunt ut labore et dolore magna aliqua.  Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'></Card>
                            <Card icon='premium.png' name="Premium" description='Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor 
                                    incididunt ut labore et dolore magna aliqua.  Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'></Card>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
}
export default LandinfSubscriptions;