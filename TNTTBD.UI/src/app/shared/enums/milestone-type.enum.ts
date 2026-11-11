export enum MilestoneType {
  Baptism = 'Baptism',
  FirstPenance = 'FirstPenance',
  FirstCommunion = 'FirstCommunion',
  Confirmation = 'Confirmation',
  SolemnCommunion = 'SolemnCommunion',
  Graduation = 'Graduation',
}

export const MILESTONE_TYPE_KEYS: Record<MilestoneType, string> = {
  [MilestoneType.Baptism]: 'milestone.baptism',
  [MilestoneType.FirstPenance]: 'milestone.firstPenance',
  [MilestoneType.FirstCommunion]: 'milestone.firstCommunion',
  [MilestoneType.Confirmation]: 'milestone.confirmation',
  [MilestoneType.SolemnCommunion]: 'milestone.solemnCommunion',
  [MilestoneType.Graduation]: 'milestone.graduation',
};

/** Only Baptism and Confirmation record a Godfather/Godmother. */
export function milestoneSupportsSponsor(type: MilestoneType | null | undefined): boolean {
  return type === MilestoneType.Baptism || type === MilestoneType.Confirmation;
}
