import logo from './logo.svg';
import './App.css';
import AudioRecorder from './Components/AudioRecorder/AudioRecorder';
import LoginPage from './Components/LoginPage/LoginPage';

function App() {
  return (
    <div className="App">
      <AudioRecorder></AudioRecorder>
      <LoginPage></LoginPage>
    </div>
  );
}

export default App;
