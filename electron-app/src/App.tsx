import React, { Dispatch, SetStateAction, useContext, useEffect, useState } from 'react';
import {
  BrowserRouter,
  Routes,
  Route,
  Navigate
} from "react-router-dom";
import { IsSignedIn } from './app/services/account';
import Account, { View } from './app/components/Account/Account';
import Home from './app/components/Home/Home';

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
    IsSignedIn(appContext).then((signedIn) => appContext.setSignedIn(signedIn));
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
    <BrowserRouter>
      {currentRoutes(appContext.signedIn)}
    </BrowserRouter>
  );
}

export default App;
export { context };
export type { AppContext };
