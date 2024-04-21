export interface RoleWiseModule {
  id: number;
  roleId: number;
  moduleId: number;
  module: String;
  checked: boolean;
}
export interface Role {
  id: number;
  name: string;
}
export interface Module {
  id: number;
  module: string;
  checked: boolean;
}
export interface RoleModule {
  id: number;
  RoleId: number;
  ModuleId: number;
}
