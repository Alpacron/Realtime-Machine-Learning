import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import Home from './Home';
import { context } from './../../App';

describe('<Home />', () => {
  beforeEach(() => {
    const contextValue = {
      signedIn: undefined,
      setSignedIn: () => { }
    };

    render(
      <context.Provider value={contextValue}>
        <Home />
      </context.Provider>
    );
  });

  test('it should mount', () => {
    const home = screen.getByTestId('Home');

    expect(home).toBeInTheDocument();
  });
});