import { tabIndexFromKey, tabKeyForSelection } from './tab-query-state';

describe('tab query state', () => {
  const tabKeys = ['profile', 'history', 'photos'];

  it('defaults missing or invalid tab keys to the first tab', () => {
    expect(tabIndexFromKey(tabKeys, null)).toBe(0);
    expect(tabIndexFromKey(tabKeys, 'unknown')).toBe(0);
  });

  it('restores a known tab key', () => {
    expect(tabIndexFromKey(tabKeys, 'photos')).toBe(2);
  });

  it('does not add a query parameter for the default tab or invalid keys', () => {
    expect(tabKeyForSelection(tabKeys, 0, null)).toBeNull();
    expect(tabKeyForSelection(tabKeys, 0, 'unknown')).toBeNull();
  });

  it('returns the selected key when changing tabs', () => {
    expect(tabKeyForSelection(tabKeys, 2, 'history')).toBe('photos');
    expect(tabKeyForSelection(tabKeys, 0, 'history')).toBe('profile');
  });
});