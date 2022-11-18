import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import Account, { View } from './Account';

describe('<Account />', () => {
  test('it should mount', () => {
    render(<Account view={View.signin} />);

    const account = screen.getByTestId('Account');

    expect(account).toBeInTheDocument();
  });
});