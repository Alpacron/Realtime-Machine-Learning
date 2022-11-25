const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('electronAPI', {
    screenshot: async (title) => await ipcRenderer.invoke('screenshot', title)
});
