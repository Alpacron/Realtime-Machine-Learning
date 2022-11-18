import { AppContext } from "../../App";
import { ApiResult, Post } from "./request";

async function SignIn(context: AppContext, email: string, password: string): Promise<ApiResult> {
    return await Post(context, "/auth/authenticate", JSON.stringify({ email: email, password: password })).then((response) => {
        if (response.ok) {
            localStorage.setItem("token", response.result.token);
            context.setSignedIn(true);
        }
        return response;
    });
}

export { SignIn };