const { app, BrowserWindow, Menu, Tray, nativeImage, ipcMain, desktopCapturer, protocol, net } = require("electron");
const path = require("path");
const url = require("url");
const Screenshot = require("./services/screenshot");
const { SignOut, SignIn, SignUp, IsSignedIn } = require("./services/account");

const icon = nativeImage.createFromPath(path.join(__dirname, "icon.ico"));
const isDev = !app.isPackaged;
let mainWindow;
let tray;

process.env.GATEWAY = 'http://192.168.240.10/api';

// Create the native browser window.
function createWindow() {
  mainWindow = new BrowserWindow({
    width: 900,
    minWidth: 900,
    height: 675,
    minHeight: 675,
    icon: icon,
    autoHideMenuBar: true,
    webPreferences: {
      devTools: isDev,
      preload: path.join(__dirname, "preload.js"),
    }
  });

  const appUrl = isDev
    ? "http://localhost:3000"
    : url.format({
      pathname: path.join(__dirname, "index.html"),
      protocol: "file:",
      slashes: true,
    });

  mainWindow.loadURL(appUrl);

  // Automatically open Chrome's DevTools in development mode.
  if (isDev) {
    mainWindow.webContents.openDevTools();
  } else {
    mainWindow.removeMenu();
  }

  // Hide mainwindow instead of closing when pressing close
  mainWindow.on('close', event => {
    event.preventDefault();
    mainWindow.hide();
  });

  app.on('before-quit', () => {
    tray.destroy();
    mainWindow.destroy();
  });

  // Show mainwindow once ready and create tray
  mainWindow.once('ready-to-show', () => {
    createTray();
    mainWindow.show();
  });
}

// Create tray app
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

// Setup a local proxy to adjust the paths of requested files when loading
// them from the local production bundle (e.g.: local fonts, etc...).
function setupLocalFilesNormalizerProxy() {
  protocol.registerHttpProtocol(
    "file",
    (request, callback) => {
      const url = request.url.substr(8);
      callback({ path: path.normalize(`${__dirname}/${url}`) });
    },
    (error) => {
      if (error) console.error("Failed to register protocol");
    }
  );
}

// Create main window if it doesn't exist already
if (!app.requestSingleInstanceLock()) {
  app.quit();
} else {
  app.on('second-instance', () => {
    if (mainWindow) mainWindow.show();
  });

  app.whenReady().then(() => {
    ipcMain.handle('screenshot', Screenshot);
    ipcMain.handle('signin', SignIn);
    ipcMain.handle('signup', SignUp);
    ipcMain.handle('signout', SignOut);
    ipcMain.handle('issignedin', IsSignedIn);
    createWindow();
    setupLocalFilesNormalizerProxy();
  });
}
