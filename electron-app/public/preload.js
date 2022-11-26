const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('electron', {
  screenshot: async (title) => await ipcRenderer.invoke('screenshot', title),
  signout: async (context) => await ipcRenderer.invoke('signout', context),
  signin: async (context, account) => await ipcRenderer.invoke('signin', context, account),
  signup: async (context, account) => await ipcRenderer.invoke('signup', context, account),
  issignedin: async () => await ipcRenderer.invoke('issignedin'),
});