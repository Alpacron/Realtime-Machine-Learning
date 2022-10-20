const { app, BrowserWindow, Menu, Tray, nativeImage } = require('electron');

let mainWindow;
let tray;
const icon = nativeImage.createFromPath(__dirname + "/src/assets/icon.ico");

function createWindow() {
    mainWindow = new BrowserWindow({
        width: 900, height: 675,
        minWidth: 900, minHeight: 675,
        show: false,
        icon: icon,
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

    mainWindow.once('ready-to-show', () => {
        createTray();
        mainWindow.show();
    });
}


const trayMenu = Menu.buildFromTemplate([

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

function createTray() {
    tray = new Tray(icon);
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
    });

    app.on('before-quit', () => {
        mainWindow.destroy();
        tray.destroy();
    });
}