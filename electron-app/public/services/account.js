const { Post, Get } = require("./request");
const Store = require('electron-store');

const store = new Store();

async function SignOut() {
    const bearer = store.get('bearer');
    if (bearer) store.delete('bearer');
    return Promise.resolve(false);
}

async function SignIn(_event, account) {
    const bearer = store.get('bearer');

    return await Post('/auth/authenticate', bearer, JSON.stringify({ email: account.email, password: account.password })).then((response) => {
        if (response.ok) {
            store.set('bearer', response.result.token);
        }
        return response;
    });
}

async function SignUp(_event, account) {
    return await Post("/auth/register", undefined, JSON.stringify({
        username: account.username,
        email: account.email,
        password: account.password
    })).then((response) => {
        if (response.ok) {
            store.set('bearer', response.result.token);
        }
        return response;
    });
}

async function IsSignedIn() {
    const bearer = store.get('bearer');

    if (!bearer) return false;

    return await Get("/auth/authenticated", bearer).then((response) => {
        if (!response.ok) {
            store.delete('bearer');
        }
        return response.ok;
    });
}

module.exports = { SignOut, SignIn, SignUp, IsSignedIn };