import axios from "axios";
import token from './jwtToken';

//const apiUrl = "https://assetstoreapi.azurewebsites.net/api";
//const apiUrl = "http://wonof44260-001-site1.itempurl.com/api";
const apiUrl = "https://localhost:7231/api";

const get = async (url) => {
    let res = [];
    await axios.get(url, {
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + token.getToken()
        }
    }).then(function(response){
        if(response.status == '200'){
            res = response.data;
        }
        
    });
    return res;
}
const post = async(url, data) => {
    let res = {};

    await axios.post(url, data, {
        headers:{
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': "Bearer " + token.getToken()
        }
    }).then(function(response) {
        if(response.data.token)
            token.setToken(response.data.token);
        res = response;
    }).catch(() => res = undefined); 
    return res;
    
}

const postWithFile = async(url, data) => {
    let res = {};
    await axios.post(url,data, {
        headers:{
            'Accept': 'application/json',
            'Content-Type': `multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW`,
            'Authorization': "Bearer " + token.getToken()
            }
            }).then(function(response) {
                if(response.data.token)
                    token.setToken(response.data.token);
                res = response;
            }).catch(() => res = undefined); 
    return res;
    
}

//----------------------gpt-----------------------
const getAudioResponce = async(data) => {
    return await postWithFile(`${apiUrl}/Gpt/getReponseFromAudio`, data);
}
const getPhotoResponce = async(data) => {
    return await postWithFile(`${apiUrl}/Gpt/getReponseFromImage`, data);
}
const getTextResponce = async(data) => {
    return await post(`${apiUrl}/Gpt/getReponseFromText?requestText=${data}`,{});
}



//--------------------authorization--------------------
const signIn = async(login, password) => {
    return await post(`${apiUrl}/Authentication/login`, {Password: password, UserName: login});
}

const signUp = async(login, password) => {
    return await post(`${apiUrl}/Authentication/regUser`, {Password: password, UserName: login});
}
const methods = {
    signIn: signIn,
    signUp: signUp,
    getAudioResponce: getAudioResponce,
    getPhotoResponce: getPhotoResponce,
    getTextResponce: getTextResponce
}

export default methods;