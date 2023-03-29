import logo from './logo.svg';
import './App.css';
import AudioRecorder from './Components/AudioRecorder/AudioRecorder';
import LoginModal from './Components/LoginModal/LoginModal';
import { useState } from 'react';
import api from './apiAccess';
import LandingPage from './Components/LandingPage/LandingPage';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import ChatPage from './Components/ChatPage/ChatPage';
import HistoryPage from './Components/HistoryPage/HistoryPage';

function App() {

  return(
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<LandingPage></LandingPage>}></Route>
        <Route path='/chat' element={<ChatPage></ChatPage>}></Route>
        <Route path='/chat/history' element={<HistoryPage></HistoryPage>}></Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App;
