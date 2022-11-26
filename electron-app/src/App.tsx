import React, { Dispatch, SetStateAction, useContext, useEffect, useState } from 'react';
import { HashRouter, Routes, Route, Navigate } from "react-router-dom";
import Account, { View } from './components/Account/Account';
import Home from './components/Home/Home';

interface AppContext {
  signedIn: boolean | undefined;
  setSignedIn: Dispatch<SetStateAction<boolean | undefined>>;
}

const context: React.Context<AppContext> = React.createContext({} as AppContext);

function App() {
  const [signedIn, setSignedIn] = useState<boolean | undefined>();

  const contextValue = {
    signedIn: signedIn,
    setSignedIn: setSignedIn
  };

  return (
    <div data-testid="App">
      <context.Provider value={contextValue}>
        <AppRouter />
      </context.Provider>
    </div>
  );
}

function AppRouter() {
  const appContext = useContext(context);

  useEffect(() => {
    window.electron.issignedin().then((signedin: boolean) => {
      appContext.setSignedIn(signedin);
    });
  }, []);

  const currentRoutes = (signedIn: boolean | undefined) => {
    if (signedIn === undefined) return;

    if (!signedIn) {
      return (
        <Routes>
          <Route path="account">
            <Route path="signin" element={<Account view={View.signin} />} />
            <Route path="signup" element={<Account view={View.signup} />} />
            <Route path="reset_password" element={<Account view={View.resetpw} />} />
            <Route path="" element={<Navigate to="/account/signin" replace />} />
          </Route>
          <Route path="*" element={<Navigate to="/account/signin" replace />} />
        </Routes>
      );
    } else {
      return (
        <Routes>
          <Route index element={<Home />} />
          <Route path="account/*" element={<Navigate to="/" replace />} />
        </Routes>
      );
    }
  };

  return (
    <HashRouter>
      {currentRoutes(appContext.signedIn)}
    </HashRouter>
  );
}

export default App;
export { context };
export type { AppContext };
