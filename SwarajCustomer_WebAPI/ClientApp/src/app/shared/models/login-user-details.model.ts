export interface LoginDataModal {
  data: LoginUserDetails;
  status: boolean;
  jwtToken: any
}

export interface LoginUserDetails {
  userID: number;
  firstName: string;
  username: string;
  emailID: string;
  salt: string;
  isActive: number;
  mobile: string;
  userTypeCode: string;
  loginAttempts: number;
  intervalPending: number;
  roleID: number;
  roleName: string;
  societyTransID: number;
  subClassSocietyCode: number;
  backLogResetStatus: number;
  societyStatus: number;
  statusEditable: number;
  formE: number;
  total: number;
  lastLogin: Date;
  societyType: string;
  caseAutoId: number;
  caseId: number;
  finalSubmissionDate: string;
  errorMessage: string;
  memberSNo: number;
  arcsCode: number;
  societyId: number;
}
