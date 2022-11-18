const { app, BrowserWindow, Menu, Tray, nativeImage } = require('electron');

const icon = nativeImage.createFromPath(__dirname + "/public/icon.ico");
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
            devTools: isDev
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

if (!app.requestSingleInstanceLock()) {
    app.quit();
} else {
    app.on('second-instance', () => {
        if (mainWindow) mainWindow.show();
    });

    app.whenReady().then(() => {
        createWindow();
    });

    app.on('before-quit', () => {
        mainWindow.destroy();
        tray.destroy();
    });
}
