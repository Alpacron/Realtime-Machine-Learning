import { AppContext } from "../../App";

interface ApiResult {
    ok: boolean,
    result: any;
}

async function Get(context: AppContext, path: string): Promise<ApiResult> {
    return Api(context, "Get", path, undefined);
}

async function Post(context: AppContext, path: string, body: any): Promise<ApiResult> {
    return Api(context, "Post", path, body);
}

async function Api(context: AppContext, method: "Post" | "Get", path: string, body: any): Promise<ApiResult> {
    let responseOk: boolean;

    return await fetch(process.env.REACT_APP_GATEWAY + path, {
        method: method,
        body: body,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': context.signedIn ? 'Bearer ' + localStorage.getItem("token") : ""
        }
    }).then(response => {
        responseOk = response.ok;
        return response.json();
    }).then((response: any) => {
        return {
            ok: responseOk,
            result: response
        };
    });
}

export { Get, Post };
export type { ApiResult };
