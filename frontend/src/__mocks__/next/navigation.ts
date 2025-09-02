export const pushMock = jest.fn(); // this is our mock function

export const useRouter = () => ({
  push: pushMock
});