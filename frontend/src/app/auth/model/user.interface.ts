import { Role } from "./role.enum";

export interface User {
  familyName: string;
  givenName: string;
  role: Role;
}
