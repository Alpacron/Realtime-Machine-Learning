import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import AccountResetPw from './ResetPW';

describe('<AccountResetPw />', () => {
  test('it should mount', () => {
    render(<AccountResetPw />);

    const accountResetPw = screen.getByTestId('AccountResetPw');

    expect(accountResetPw).toBeInTheDocument();
  });
});