import { useEffect, useState } from 'react';
import './ChatComponent.css'
import token from './../../jwtToken'
import { useRef } from 'react';
import api from '../../apiAccess';
const mimeType = "audio/webm";

const ChatComponent = () => {

    useEffect(() => {
        const res = token.getUserData();
        setUserInfo(res);
    }, []);

    const inputFile = useRef(null)

    const [text, setText] = useState("");
    const [userInfo, setUserInfo] = useState({});
    const [resText, setResText] = useState([]);
    const sendText = async (e) => {
        e.preventDefault();
        if (text == "")
            return;
        let res = await api.getTextResponce(text);
        console.log(res);
        if (res.status == 200){
            setText("");
            setResText(resText => [...resText, <p key={resText.length + 1} className='res'>{res.data}</p>]);
        }
           
    }

    const sendImage = async (e) => {
        e.preventDefault();
        const formData = new FormData();
        formData.append('file', e.target.files.item(0));
        formData.append('userId', sessionStorage.getItem('userId'));
        let res = await api.getPhotoResponce(formData);
        if (res.status == '200')
            setResText(resText => [...resText, <p key={resText.length + 1} className='res'>{res.data}</p>]);
    }

    const selectFile = (e) => {
        e.preventDefault();
        inputFile.current.click();
    }

    const sendAudio = async (data) => {
        data.append('userId', sessionStorage.getItem('userId'));
        let res = await api.getAudioResponce(data);
        if(res.status == '200')
            setResText(resText => [...resText, <p key={resText.length + 1} className='res'>{res.data}</p>]);
    }


    const [permission, setPermission] = useState(false);

    const mediaRecorder = useRef(null);

    const [recordingStatus, setRecordingStatus] = useState("inactive");

    const [stream, setStream] = useState(null);

    const [audioChunks, setAudioChunks] = useState([]);

    const getMicrophonePermission = async () => {
        if ("MediaRecorder" in window) {
            try {
                const mediaStream = await navigator.mediaDevices.getUserMedia({
                    audio: true,
                    video: false,
                });
                setPermission(true);
                setStream(mediaStream);
            } catch (err) {
                alert(err.message);
            }
        } else {
            alert("The MediaRecorder API is not supported in your browser.");
        }
    };

    const startRecording = async () => {
        setRecordingStatus("recording");
        const media = new MediaRecorder(stream, { type: mimeType });

        mediaRecorder.current = media;

        mediaRecorder.current.start();

        let localAudioChunks = [];

        const stopRecordingAfter15Seconds = () => {
            setRecordingStatus("inactive");
            mediaRecorder.current.stop();

            mediaRecorder.current.onstop = () => {
                const audioBlob = new Blob(audioChunks, { type: mimeType });
                const formData = new FormData();
                formData.append('file', audioBlob, 'audio.webm');
                sendAudio(formData);
                setAudioChunks([]);
                setAudioChunks([]);
            };
        };

        setAudioChunks(localAudioChunks);
        setTimeout(stopRecordingAfter15Seconds, 15000);

        mediaRecorder.current.ondataavailable = (event) => {
            if (typeof event.data === "undefined") return;
            if (event.data.size === 0) return;
            localAudioChunks.push(event.data);
        };

    };

    const stopRecording = () => {
        setRecordingStatus("inactive");
        mediaRecorder.current.stop();

        mediaRecorder.current.onstop = () => {
            const audioBlob = new Blob(audioChunks, { type: mimeType });
            const formData = new FormData();
            formData.append('file', audioBlob, 'audio.webm');
            sendAudio(formData);
            setAudioChunks([]);
        };
    };

    return (
        <>
            <div id="responce-container" className='responce-text'>
                {resText.map(x => {
                    return x;
                })}
            </div>
            <div style={{ zIndex: -5, height: "100%", position: 'fixed', width: "100%", backgroundColor: "#93B7BE" }}>
                <div style={{ margin: 10 }}>
                    <div className='input-absolute'>
                        <form className='chat-form'>
                            <div style={{ display: 'flex', flexDirection: 'column', flexGrow: 1, position: 'relative' }}>
                                <textarea onChange={(e) => setText(e.target.value)} className='main-input'></textarea>
                                <input ref={inputFile} accept="image/*" type='file' id='file' onChange={(e) => sendImage(e)} style={{ display: 'none' }} />
                                <button onClick={(e) => sendText(e)} className='send-text-btn'>
                                    <img src='send.png' style={{ width: "1rem", height: "1rem" }}>
                                    </img>
                                </button>
                                {userInfo.UserFreePlus &&
                                    <button onClick={(e) => selectFile(e)} className='send-img-btn'>
                                        <img src='image.png' style={{ width: "1rem", height: "1rem" }}>
                                        </img>
                                    </button>}


                                {userInfo.UserPremium && !permission ? (
                                    <button onClick={getMicrophonePermission} type="button" className='send-audio-btn'>
                                        <img src='perm.png' style={{ width: "1rem", height: "1rem" }}>
                                        </img>
                                    </button>
                                ) : null}
                                {userInfo.UserPremium && permission && recordingStatus === "inactive" ? (
                                    <button onClick={startRecording} type="button" className='send-audio-btn'>
                                        <img src='mic.png' style={{ width: "1rem", height: "1rem" }}>
                                        </img>
                                    </button>
                                ) : null}
                                {userInfo.UserPremium && recordingStatus === "recording" ? (
                                    <button onClick={stopRecording} type="button" className='send-audio-btn'>
                                        <img src='record.png' style={{ width: "1rem", height: "1rem" }}>
                                        </img>
                                    </button>
                                ) : null}
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </>
    )
}
export default ChatComponent;