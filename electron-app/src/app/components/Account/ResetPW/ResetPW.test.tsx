import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import ResetPw from './ResetPw';

describe('<ResetPw />', () => {
  test('it should mount', () => {
    render(<ResetPw />);
    
    const resetPw = screen.getByTestId('ResetPw');

    expect(resetPw).toBeInTheDocument();
  });
});