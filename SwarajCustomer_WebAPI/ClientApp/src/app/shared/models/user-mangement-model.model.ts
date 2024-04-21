

export interface users {
  loginID: number;
  societyName: string;
  roleName: string;
  firstname: string;
  mobile: string;
  emailid: string;
  username: string;
  password: string;
  age: number;
  gender: string;
  address: string;
  address2: string;
  isActive: number;
  postalcode: string;
  districtName: string;
}
export interface SocietyType {
  societyName: string
  address: string
  districtName: string
  societyType: string
}
export interface usersModel {
  data: users[];
  recordsCount: number;
}

export interface District {
  districtName: string;
  districtCode: string;
  coopDisCode: string;
}
 
export interface SocietyType {
  societyName: string
  address: string
  districtName: string
  societyType: string
}
export interface DistrictType {
  societyClassCode: number;
  societySubClassName: string;
}
