import { AppContext } from "../../App";
import { AccountState } from "../components/Account/Account";
import { ApiResult, Post, Get } from "./request";

async function SignOut(context: AppContext) {
    if (context.signedIn === false) return;
    localStorage.removeItem("token");
    context.setSignedIn(false);
}

async function SignIn(context: AppContext, account: AccountState): Promise<ApiResult> {
    return await Post(context, "/auth/authenticate", JSON.stringify({ email: account.email, password: account.password })).then((response) => {
        if (response.ok) {
            localStorage.setItem("token", response.result.token);
            context.setSignedIn(true);
        }
        return response;
    });
}

async function SignUp(context: AppContext, account: AccountState): Promise<ApiResult> {
    return await Post(context, "/auth/register", JSON.stringify({
        username: account.username,
        email: account.email,
        password: account.password
    })).then((response) => {
        if (response.ok) {
            localStorage.setItem("token", response.result.token);
            context.setSignedIn(true);
        }
        return response;
    });
}

async function ResetPW(_context: AppContext, _account: AccountState): Promise<ApiResult> {
    return Promise.resolve({
        ok: false,
        result: {
            title: "not yet implemented"
        }
    });
}

async function IsSignedIn(context: AppContext) {
    if (localStorage.getItem("token") === null) return false;

    return await Get(context, "/auth/authenticated").then((response) => {
        if (!response.ok)
            localStorage.removeItem("token");
        return response.ok;
    });
}

export { SignOut, SignIn, SignUp, ResetPW, IsSignedIn };