const { app, BrowserWindow, Menu, Tray } = require('electron')

let tray
let mainWindow

const trayMenu = Menu.buildFromTemplate([
    {
        icon: __dirname + '/src/logo16.png',
        label: 'App',
        enabled: false
    },
    {
        type: 'separator'
    },
    {
        label: 'Quit App',
        click: _ => {
            app.quit()
        }
    }

])

function createWindow() {
    mainWindow = new BrowserWindow({
        width: 800, height: 800,
        icon: __dirname + '/src/logo256.png'
    });
    mainWindow.loadFile('dist/client-app/index.html');

    mainWindow.on('close', event => {
        event.preventDefault()
        mainWindow.hide()
    })
}

function createTray() {
    tray = new Tray(__dirname + '/src/logo256.png')
    tray.setContextMenu(trayMenu)
    tray.on('click', () => {
        mainWindow.show()
    })
}

app.whenReady().then(() => {
    createWindow()
    createTray()
})

app.on('before-quit', () => {
    mainWindow.destroy()
    tray.destroy()
});