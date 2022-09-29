const { app, BrowserWindow, Menu, Tray } = require('electron');

let mainWindow;
let tray;

function createWindow() {
    mainWindow = new BrowserWindow({
        width: 900, height: 675,
        icon: __dirname + '/src/assets/icon256.png',
        autoHideMenuBar: true,
        webPreferences: {
            devTools: !app.isPackaged
        }
    });
    if (app.isPackaged) {
        mainWindow.removeMenu();
        mainWindow.loadFile('dist/client-app/index.html');
    }
    else {
        mainWindow.loadURL('http://localhost:4200');
    }

    mainWindow.on('close', event => {
        event.preventDefault();
        mainWindow.hide();
    });
}

const trayMenu = Menu.buildFromTemplate([
    {
        icon: __dirname + '/src/assets/icon16.png',
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

function createTray() {
    tray = new Tray(__dirname + '/src/assets/icon256.png');
    tray.setContextMenu(trayMenu);
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
        createTray();
    });

    app.on('before-quit', () => {
        mainWindow.destroy();
        tray.destroy();
    });
}