const { app, BrowserWindow, Menu, Tray } = require('electron')

let mainWindow
let tray
let overlay

function createWindow() {
    mainWindow = new BrowserWindow({
        width: 800, height: 800,
        icon: __dirname + '/src/assets/logo256.png'
    });
    mainWindow.loadFile('dist/client-app/index.html');

    mainWindow.on('close', event => {
        event.preventDefault()
        mainWindow.hide()
    })
}

const trayMenu = Menu.buildFromTemplate([
    {
        icon: __dirname + '/src/assets/logo16.png',
        label: 'Valorant AI',
        enabled: false
    },
    {
        type: 'separator'
    },
    {
        label: 'Quit Valorant AI',
        click: _ => {
            app.quit()
        }
    }

])

function createTray() {
    tray = new Tray(__dirname + '/src/assets/logo256.png')
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