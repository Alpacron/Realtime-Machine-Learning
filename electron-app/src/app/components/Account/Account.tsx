import { FC, useContext, useState } from 'react';
import styles from './Account.module.scss';
import SignInForm from './SignIn/SignIn';
import SignUp from './SignUp/SignUp';
import ResetPW from './ResetPW/ResetPW';
import { ApiResult } from '../../api/request';
import { SignIn } from '../../api/account';
import { context } from '../../../App';

export enum View {
  signin = "signin",
  signup = "signup",
  resetpw = "resetpw"
}

interface AccountProps {
  view: View;
}

interface AccountState {
  username: string,
  email: string,
  password: string,
  error: string,
  username_error: string,
  email_error: string,
  password_error: string;
};

const errorState = {
  error: "",
  username_error: "",
  email_error: "",
  password_error: ""
};

const Account: FC<AccountProps> = ({ view }: AccountProps) => {
  const appContext = useContext(context);
  const [account, setAccount] = useState<AccountState>({
    ...({
      username: "",
      email: "",
      password: ""
    }),
    ...errorState
  });

  const handleErrors = (response: ApiResult) => {
    const { result } = response;

    setAccount(a => ({ ...a, error: result.title }));
    if (result.errors) {
      for (let [error, value] of Object.entries(result.errors)) {
        setAccount(a => ({ ...a, [error.toLowerCase() + "_error"]: (value as string[])[0] }));
      }
    }
  };

  const onSignIn = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setAccount(a => ({ ...a, ...errorState }));
    SignIn(appContext, account.email, account.password).then(r => {
      if (!r.ok)
        handleErrors(r);
    });
  };

  const currentView = (view: View) => {
    return view === View.signin
      ? <SignInForm onSignIn={onSignIn} state={account} setState={setAccount} />
      : view === View.signup
        ? <SignUp />
        : <ResetPW />;
  };

  return (
    <section data-testid="Account" className={styles.Account}>
      {currentView(view)}
    </section>
  );
};

export default Account;
export type { AccountState };
