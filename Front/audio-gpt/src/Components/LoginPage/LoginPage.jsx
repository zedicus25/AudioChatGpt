import { useState } from "react";
import api from './../../apiAccess';

const LoginPage = () => {
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");

    const logIn = async (e) => {
        e.preventDefault();
        let res = await api.signIn(login, password);
        if (res.status == '200') {
            setLogin("");
            setPassword("");
        }

    }
    return (
        <div>
            <form>
                <input value={login} onChange={(e) => setLogin(e.target.value)} type='text'></input>
                <input value={password} onChange={(e) => setPassword(e.target.value)} type='password'></input>
                <input onClick={(e) => logIn(e)} type='button' value="Login"></input>
            </form>
        </div>
    )
}
export default LoginPage;