// __tests__/EmployeeCheckinsPage.test.tsx
import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { SWRConfig } from 'swr';
import EmployeeCheckinsPage from '../app/(protected)/checkins/page';
import { authService } from '../lib/auth';

// Mock authService.getToken
jest.spyOn(authService, 'getToken').mockReturnValue(JSON.stringify({ id: 1, role: 1 }));

// Let Jest automatically use the __mocks__/next/navigation you already have
jest.mock('next/navigation');

// Mock global fetch (or keep using your setup)
global.fetch = jest.fn(() =>
  Promise.resolve({
    json: () =>
      Promise.resolve({
        data: [
          { id: 1, mood: 4, notes: 'Good', userId: 1, createdAt: '2025-09-02', user: { id: 1, name: 'Alice' } },
          { id: 2, mood: 5, notes: 'Great', userId: 1, createdAt: '2025-09-03', user: { id: 1, name: 'Alice' } }
        ],
        totalRecords: 2
      })
  })
) as jest.Mock;

describe('EmployeeCheckinsPage', () => {
  it('renders heading and checkins table', async () => {
    render(
      <SWRConfig value={{ dedupingInterval: 0 }}>
        <EmployeeCheckinsPage />
      </SWRConfig>
    );

    expect(screen.getByText(/My Check-ins/i)).toBeInTheDocument();
    await waitFor(() => expect(screen.getByText('Good')).toBeInTheDocument());
    expect(screen.getByText('Great')).toBeInTheDocument();
  });

  it('calls router.push when add button is clicked', async () => {
    render(
      <SWRConfig value={{ dedupingInterval: 0 }}>
        <EmployeeCheckinsPage />
      </SWRConfig>
    );

    fireEvent.click(screen.getByText('+ Add Check-in'));

    // Your __mocks__/next/navigation should export pushMock
    const { pushMock } = require('../__mocks__/next/navigation');
    expect(pushMock).toHaveBeenCalledWith('/checkins/add');
  });
});
