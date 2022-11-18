import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import ResetPW from './ResetPW';

describe('<ResetPw />', () => {
  test('it should mount', () => {
    render(<ResetPW />);

    const resetPW = screen.getByTestId('ResetPw');

    expect(resetPW).toBeInTheDocument();
  });
});