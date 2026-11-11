import { ContactRelationType } from '../enums';

export const CONTACT_RELATION_TYPES: readonly { v: ContactRelationType; k: string }[] = [
  { v: ContactRelationType.Father, k: 'contact.relation.Father' },
  { v: ContactRelationType.Mother, k: 'contact.relation.Mother' },
  { v: ContactRelationType.Guardian, k: 'contact.relation.Guardian' },
  { v: ContactRelationType.Grandfather, k: 'contact.relation.Grandfather' },
  { v: ContactRelationType.Grandmother, k: 'contact.relation.Grandmother' },
  { v: ContactRelationType.Babysitter, k: 'contact.relation.Babysitter' },
  { v: ContactRelationType.Sibling, k: 'contact.relation.Sibling' },
  { v: ContactRelationType.Godparent, k: 'contact.relation.Godparent' },
  { v: ContactRelationType.Undefined, k: 'contact.relation.Undefined' },
];
