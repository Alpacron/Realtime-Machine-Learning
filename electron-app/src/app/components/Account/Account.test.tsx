import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import Account, { View } from './Account';

describe('<SignIn />', () => {
  test('it should mount', () => {
    render(<Account view={View.signin} />);

    const signin = screen.getByTestId('SignIn');

    expect(signin).toBeInTheDocument();
  });
});