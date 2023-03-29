const tokenKey = 'token';

const setToken = (token) => {
    sessionStorage.setItem(tokenKey, token);
}

const getToken = () => {
    return sessionStorage.getItem(tokenKey);
}

const getUserData = () => {
    const token = getToken();

    if (!token) {
        return {
            username: '',
            UserFree: false,
            UserFreePlus: false,
            UserPlus: false,
            UserPremium: false,
            Admin: false,
        };
    }

    const uncodedToken = unParseToken(token);
    return {
        username: Object.values(uncodedToken)[0],
        userId: Object.values(uncodedToken)[3],
        UserFree: Object.values(uncodedToken)[2].includes('UserFree'),
        UserFreePlus: Object.values(uncodedToken)[2].includes('UserFreePlus'),
        UserPlus: Object.values(uncodedToken)[2].includes('UserPlus'),
        UserPremium: Object.values(uncodedToken)[2].includes('UserPremium'),
        Admin: Object.values(uncodedToken)[2].includes('Admin'),
    };
}

const logOut = () => {
    sessionStorage.removeItem(tokenKey);

    return {
        username: '',
        UserFree: false,
        UserFreePlus: false,
        UserPlus: false,
        UserPremium: false,
        Admin: false,
    };
}

function unParseToken(token) {
    if (!token) {
        return undefined;
    }

    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

const functions = {
    setToken: setToken,
    getToken: getToken,
    getUserData: getUserData,
    logOut: logOut
};

export default functions;