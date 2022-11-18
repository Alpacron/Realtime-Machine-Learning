import React, { Dispatch, SetStateAction, useState } from 'react';
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate
} from "react-router-dom";
import Account, { View } from './app/components/Account/Account';

interface AppContext {
  signedIn: boolean | undefined;
  setSignedIn: Dispatch<SetStateAction<boolean | undefined>>;
}

function App() {
  const [signedIn, setSignedIn] = useState<boolean | undefined>();

  const contextValue = {
    signedIn: signedIn,
    setSignedIn: setSignedIn
  };

  const context: React.Context<AppContext> = React.createContext(contextValue);

  return (
    <context.Provider value={contextValue}>
      <Router>
        <Routes>
          <Route path="account">
            <Route path="signin" element={<Account view={View.signin} />} />
            <Route path="signup" element={<Account view={View.signup} />} />
            <Route path="reset_password" element={<Account view={View.resetpw} />} />
            <Route path="" element={<Navigate to="/account/signin" replace />} />
          </Route>
          <Route path="*" element={<Navigate to="/account/signin" replace />} />
        </Routes>
      </Router>
    </context.Provider>
  );
}

export default App;
export type { AppContext };
