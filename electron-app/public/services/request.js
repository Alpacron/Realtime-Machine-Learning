const { net } = require('electron');

async function Get(path, bearer) {
    return Api('Get', path, bearer, undefined);
}

async function Post(path, bearer, body) {
    return Api('Post', path, bearer, body);
}

async function Api(method, path, bearer, body) {
    let _resolve;
    let _reject;

    const request = net.request({
        method: method,
        url: process.env.GATEWAY + path,
        body: body,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': bearer ? bearer : ''
        }
    });

    request.on('response', (response) => {
        response.on('data', (chunk) => {
            if (chunk) {
                _resolve({
                    ok: response.statusMessage === 'OK',
                    result: JSON.parse(chunk)
                });
            }
        });
    });

    request.on('error', (error) => {
        _reject(error);
    });

    if (body)
        request.write(body);
    request.end();

    return new Promise(function (resolve, reject) {
        _resolve = resolve;
        _reject = reject;
    });
}

module.exports = { Get, Post };