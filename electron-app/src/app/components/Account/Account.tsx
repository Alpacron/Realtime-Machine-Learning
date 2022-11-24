import { FC, useContext, useEffect, useState } from 'react';
import styles from './Account.module.scss';
import SignInForm from './SignIn';
import SignUpForm from './SignUp';
import ResetPWForm from './ResetPW';
import { ApiResult } from '../../services/request';
import { ResetPW, SignIn, SignUp } from '../../services/account';
import { AppContext, context } from '../../../App';

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

  const onSubmit = (e: React.FormEvent<HTMLFormElement>, ApiCall: (context: AppContext, account: AccountState) => Promise<ApiResult>) => {
    e.preventDefault();

    ApiCall(appContext, account).then(r => {
      setAccount(a => ({ ...a, ...errorState }));
      if (!r.ok)
        handleErrors(r);
    });
  };

  const currentView = (view: View) => {
    return view === View.signin
      ? <SignInForm onSignIn={e => onSubmit(e, SignIn)} state={account} setState={setAccount} />
      : view === View.signup
        ? <SignUpForm onSignUp={e => onSubmit(e, SignUp)} state={account} setState={setAccount} />
        : <ResetPWForm onResetPW={e => onSubmit(e, ResetPW)} state={account} setState={setAccount} />;
  };

  useEffect(() => {
    setAccount({
      ...({
        username: "",
        email: "",
        password: ""
      }),
      ...errorState
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
