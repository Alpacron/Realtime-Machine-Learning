import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import AccountSignIn from './SignIn';

describe('<AccountSignIn />', () => {
  test('it should mount', () => {
    render(<AccountSignIn />);

    const accountSignIn = screen.getByTestId('AccountSignIn');

    expect(accountSignIn).toBeInTheDocument();
  });
});