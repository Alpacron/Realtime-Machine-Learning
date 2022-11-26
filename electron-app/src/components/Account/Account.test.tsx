import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import Account, { View } from './Account';
import { context } from './../../App';
import { BrowserRouter } from 'react-router-dom';

describe('<SignIn />', () => {
  beforeEach(() => {
    const contextValue = {
      signedIn: undefined,
      setSignedIn: () => { }
    };

    render(
      <BrowserRouter>
        <context.Provider value={contextValue}>
          <Account view={View.signin} />
        </context.Provider>
      </BrowserRouter>
    );
  });

  test('it should mount', () => {
    const signin = screen.getByTestId('SignIn');

    expect(signin).toBeInTheDocument();
  });
});