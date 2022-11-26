const { desktopCapturer } = require('electron');

module.exports = async function Screenshot(_event, title) {
    var _resolve;
    var _reject;
    desktopCapturer.getSources({
        types: ['window'], thumbnailSize: {
            height: 1000,
            width: 1000
        }
    })
        .then(sources => {
            sources.forEach(source => {
                if (source.name && source.name.includes(title))
                    _resolve(source.thumbnail.toDataURL());
            });
            _reject(`Application with title ${title} not found in ${sources.map(s => s.name).join(", ")}.`);
        });

    return new Promise(function (resolve, reject) {
        _resolve = resolve;
        _reject = reject;
    });
};