export function tabIndexFromKey(tabKeys: readonly string[], requestedKey: string | null): number {
  const index = requestedKey == null ? -1 : tabKeys.indexOf(requestedKey);
  return index < 0 ? 0 : index;
}

export function tabKeyForSelection(
  tabKeys: readonly string[],
  selectedIndex: number,
  currentKey: string | null,
): string | null {
  const selectedKey = tabKeys[selectedIndex];
  if (!selectedKey || currentKey === selectedKey) return null;
  if (!currentKey && selectedIndex === 0) return null;
  if (currentKey && !tabKeys.includes(currentKey)) return null;
  return selectedKey;
}