import React, { Dispatch, FC, SetStateAction } from 'react';
import { Link } from 'react-router-dom';
import { AccountState } from './Account';
import FormInput from './FormInput';

interface ResetPwProps {
  onResetPW: (e: React.FormEvent<HTMLFormElement>) => void;
  state: AccountState,
  setState: Dispatch<SetStateAction<AccountState>>;
}

const ResetPw: FC<ResetPwProps> = ({ onResetPW, state, setState }: ResetPwProps) => (
  <form onSubmit={onResetPW} data-testid="ResetPW">
    <h2>ANTIVALOR</h2>
    <legend>Password Reset</legend>
    <em>A reset link will be sent to your inbox!</em>

    <FormInput name="Email" id="email" type="email" autoComplete="email" error={state.email_error} value={state.email} onChange={e => setState(a => ({ ...a, email: e.target.value }))} />

    <Link className="forgot-password" to="/account/reset_password" data-testid="forgot_password">Forgot your password?</Link>

    <input className="button" type="submit" value="Send Reset Link" data-testid="submit" formNoValidate />
    <label className="error">{state.error}</label>
    <Link to="/account/signin" data-testid="signin_account">Go back</Link>
  </form>
);

export default ResetPw;
