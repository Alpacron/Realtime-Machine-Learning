import React, { Dispatch, FC, SetStateAction } from 'react';
import { Link } from "react-router-dom";
import { AccountState } from '../Account';
import FormInput from '../FormInput/FormInput';

interface SignInProps {
  onSignIn: (e: React.FormEvent<HTMLFormElement>) => void;
  state: AccountState,
  setState: Dispatch<SetStateAction<AccountState>>;
}

const SignIn: FC<SignInProps> = ({ onSignIn, state, setState }: SignInProps) => (
  <form onSubmit={onSignIn} data-testid="SignIn">
    <h2>ANTIVALOR</h2>

    <FormInput name="Email" id="email" type="email" autoComplete="email" error={state.email_error} value={state.email} onChange={e => setState(a => ({ ...a, email: e.target.value }))} />
    <FormInput name="Password" id="password" type="password" autoComplete="password" error={state.password_error} value={state.password} onChange={e => setState(a => ({ ...a, password: e.target.value }))} />

    <Link className="forgot-password" to="/account/reset_password" data-testid="forgot_password">Forgot your password?</Link>

    <input className="button" type="submit" value="Login" data-testid="submit" formNoValidate />
    <label className="error">{state.error}</label>
    <Link to="/account/signup" data-testid="create_account">Create an Account</Link>
  </form>
);

export default SignIn;
