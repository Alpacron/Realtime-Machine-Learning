const { app, BrowserWindow, Menu, Tray, nativeImage, ipcMain, desktopCapturer } = require('electron');

const maindir = __dirname.substring(0, __dirname.lastIndexOf('\\'));
const icon = nativeImage.createFromPath(maindir + "/public/icon.ico");
const isDev = !app.isPackaged;
let mainWindow;
let tray;

function createWindow() {
    mainWindow = new BrowserWindow({
        width: 900, height: 675,
        minWidth: 900, minHeight: 675,
        show: false,
        icon: icon,
        autoHideMenuBar: true,
        webPreferences: {
            devTools: isDev,
            preload: __dirname + '/preload.js'
        }
    });

    if (isDev) {
        mainWindow.loadURL('http://localhost:3000');
    }
    else {
        mainWindow.removeMenu();
        mainWindow.loadFile('dist/electron-app/index.html');
    }

    mainWindow.on('close', event => {
        event.preventDefault();
        mainWindow.hide();
    });

    mainWindow.once('ready-to-show', () => {
        createTray();
        mainWindow.show();
    });
}

function createTray() {
    const trayMenuTemplate = Menu.buildFromTemplate([
        {
            icon: icon.resize({ width: 16, height: 16 }),
            label: app.getName(),
            enabled: false
        },
        {
            type: 'separator'
        },
        {
            label: 'Quit ' + app.getName(),
            click: _ => {
                app.quit();
            }
        }
    ]);

    tray = new Tray(icon);
    tray.setContextMenu(trayMenuTemplate);
    tray.setToolTip(app.getName());
    tray.on('click', () => {
        mainWindow.show();
    });
}

async function screenshot(_event, title) {
    var _resolve;
    var _reject;
    desktopCapturer.getSources({
        types: ['window'], thumbnailSize: {
            height: 500,
            width: 500
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
}

if (!app.requestSingleInstanceLock()) {
    app.quit();
} else {
    app.on('second-instance', () => {
        if (mainWindow) mainWindow.show();
    });

    app.whenReady().then(() => {
        ipcMain.handle('screenshot', screenshot);
        createWindow();
    });

    app.on('before-quit', () => {
        tray.destroy();
        mainWindow.destroy();
    });
}
