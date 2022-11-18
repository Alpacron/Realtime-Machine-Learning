import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import AccountSignUp from './SignUp';

describe('<AccountSignUp />', () => {
  test('it should mount', () => {
    render(<AccountSignUp />);

    const accountSignUp = screen.getByTestId('AccountSignUp');

    expect(accountSignUp).toBeInTheDocument();
  });
});