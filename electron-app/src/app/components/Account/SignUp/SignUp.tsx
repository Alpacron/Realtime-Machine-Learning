import React, { Dispatch, FC, SetStateAction } from 'react';
import { Link } from 'react-router-dom';
import { AccountState } from '../Account';
import FormInput from '../FormInput/FormInput';

interface SignUpProps {
  onSignUp: (e: React.FormEvent<HTMLFormElement>) => void;
  state: AccountState,
  setState: Dispatch<SetStateAction<AccountState>>;
}

const SignUp: FC<SignUpProps> = ({ onSignUp, state, setState }: SignUpProps) => (
  <form onSubmit={onSignUp} data-testid="SignUp">
    <h2>ANTIVALOR</h2>
    <legend>Request account</legend>

    <FormInput name="Username" id="username" type="text" autoComplete="name" error={state.username_error} value={state.username} onChange={e => setState(a => ({ ...a, username: e.target.value }))} />
    <FormInput name="Email" id="email" type="email" autoComplete="email" error={state.email_error} value={state.email} onChange={e => setState(a => ({ ...a, email: e.target.value }))} />
    <FormInput name="Password" id="password" type="password" autoComplete="password" error={state.password_error} value={state.password} onChange={e => setState(a => ({ ...a, password: e.target.value }))} />

    <Link className="forgot-password" to="/account/reset_password" data-testid="forgot_password">Forgot your password?</Link>

    <input className="button" type="submit" value="Submit" data-testid="submit" formNoValidate />
    <label className="error">{state.error}</label>
    <Link to="/account/signin" data-testid="signin_account">Have an Account?</Link>
  </form>
);

export default SignUp;
