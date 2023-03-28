import logo from './logo.svg';
import './App.css';
import AudioRecorder from './Components/AudioRecorder/AudioRecorder';
import LoginModal from './Components/LoginModal/LoginModal';
import { useState } from 'react';
import api from './apiAccess';

function App() {

  const [file, setFile] = useState({});
  const [responce, setResponce] = useState("");
  const [textResponce, setTextResponce] = useState("");
  const [userText, setUserText] = useState("");


  const sendFile = async (e) => {
    e.preventDefault();
    const formData = new FormData();
    formData.append('file', file);
    let res = await api.getPhotoResponce(formData);
    if (res.status == '200')
      setResponce(res.data);
  }
  const sendText = async (e) => {
    e.preventDefault();
    let res = await api.getTextResponce(userText);
    if (res.status == '200')
      setResponce(res.data);
  }
  return (
    <div className="App">
      <AudioRecorder></AudioRecorder>
      <LoginModal></LoginModal>
      <form>
        <input type='file' onChange={(e) => setFile(e.target.files.item(0))}></input>
        <input type='button' onClick={(e) => sendFile(e)} value='Send'></input>
        <p>{responce}</p>
      </form>

      <form>
        <input type='text' onChange={(e) => setUserText(e.target.value)}></input>
        <input type='button' onClick={(e) => sendText(e)} value='Send'></input>
        <p>{textResponce}</p>
      </form>

      
    </div>
  );
}

export default App;
