declare global {
    interface Window {
        electron: {
            screenshot: any,
            signout: any,
            signin: any,
            signup: any,
            issignedin: any,
            resetpw: any;
        };
    }
}

export { };