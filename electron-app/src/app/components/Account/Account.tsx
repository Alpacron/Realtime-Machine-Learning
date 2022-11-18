import { FC } from 'react';
import styles from './Account.module.scss';
import SignIn from './SignIn/SignIn';
import SignUp from './SignUp/SignUp';
import ResetPW from './ResetPW/ResetPW';

export enum View {
  signin = "signin",
  signup = "signup",
  resetpw = "resetpw"
}

interface AccountProps {
  view: View;
}

const defaultState = {
  error: "",
  email_error: "",
  password_error: "",
  phonenumber_error: "",
  username_error: ""
};

const currentView = (view: View) => {
  return view === View.signin
    ? <SignIn />
    : view === View.signup
      ? <SignUp />
      : <ResetPW />;
};

const Account: FC<AccountProps> = ({ view }: AccountProps) => (
  <section id="account" data-testid="account" className={styles.Account}>
    {currentView(view)}
  </section>
);

export default Account;
