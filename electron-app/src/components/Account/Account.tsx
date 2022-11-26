import { FC, useContext, useEffect, useState } from 'react';
import styles from './Account.module.scss';
import SignInForm from './SignIn';
import SignUpForm from './SignUp';
import ResetPWForm from './ResetPW';
import { AppContext, context } from '../../App';

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

const defaultErrorState = {
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
    ...defaultErrorState
  });

  const handleErrors = (response: any) => {
    const { result } = response;

    setAccount(a => ({ ...a, error: result.title }));
    if (result.errors) {
      for (let [error, value] of Object.entries(result.errors)) {
        setAccount(a => ({ ...a, [error.toLowerCase() + "_error"]: (value as string[])[0] }));
      }
    }
  };

  const onSubmit = (e: React.FormEvent<HTMLFormElement>, ApiCall: (account: AccountState) => Promise<any>) => {
    e.preventDefault();

    ApiCall(account).then(r => {
      setAccount(a => ({ ...a, ...defaultErrorState }));
      if (r.ok) {
        appContext.setSignedIn(true);
      } else {
        handleErrors(r);
      }
    });
  };

  const currentView = (view: View) => {
    return view === View.signin
      ? <SignInForm onSignIn={e => onSubmit(e, window.electron.signin)} state={account} setState={setAccount} />
      : view === View.signup
        ? <SignUpForm onSignUp={e => onSubmit(e, window.electron.signup)} state={account} setState={setAccount} />
        : <ResetPWForm onResetPW={e => onSubmit(e, window.electron.resetpw)} state={account} setState={setAccount} />;
  };

  useEffect(() => {
    setAccount({
      ...({
        username: "",
        email: "",
        password: ""
      }),
      ...defaultErrorState
    });
  }, [view]);

  return (
    <section data-testid="Account" className={styles.Account}>
      {currentView(view)}
    </section>
  );
};

export default Account;
export type { AccountState };
