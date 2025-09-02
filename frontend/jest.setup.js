require('@testing-library/jest-dom');

/** @type {import('ts-jest').JestConfigWithTsJest} */
module.exports = {
  preset: 'ts-jest/presets/js-with-ts', // allows JS + TS transform
  testEnvironment: 'jsdom',
  setupFilesAfterEnv: ['<rootDir>/jest.setup.js'],
  transformIgnorePatterns: [
    '/node_modules/(?!@?swr|other-esm-lib)/' // <-- transform modern modules
  ],
  testMatch: ['**/?(*.)+(spec|test).[jt]s?(x)'],
};