async function Get(path: string) {
    return Fetch("Get", path, undefined);
}

async function Post(path: string, body: any) {
    return Fetch("Post", path, body);
}

async function Fetch(method: "Post" | "Get", path: string, body: any) {
    return await fetch(process.env.REACT_APP_GATEWAY + path, {
        method: method,
        body: body,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': localStorage.getItem("token") ? 'Bearer ' + localStorage.getItem("token") : ""
        }
    }).then((response: any) => {
        return response.json();
    });
}

export { Get, Post };
