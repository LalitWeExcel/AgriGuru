export class SocietyTransModel {
  rowNum: number = 0;
  societyId: number = 0;
  societyName: string = "";
  arcsName: string = "";
  disttName: string = "";
  noOfMembers: number = 0;
  address1: string = "";
  dateOfCreation: Date = new Date();
  dateOfApproval: Date = new Date();
  isActive: boolean = false;
  status: number = 0;
  societyTransID: string = "";
  userId: number = 0;
  isMembers: number = 0;
}
export interface SocietyViewModel {
  data: SocietyTransModel[];
  recordsCount: number;
}
export interface SocietyDetailsModels {
    noOfMemberss: number;
    societyId: number;
    divCode: string;
    aRCSCode: string;
    societyTransID: string;
    societyCodeManual: string;
    societyName: string;
    classSocietyCode: string;
    subClassSocietyCode: string;
    address1: string;
    address2: string;
    postOffice: string;
    pin: string;
    areaOfOperation: string;
    mainobject1: string;
    mainobject2: string;
    mainobject3: string;
    mainobject4: string;
    noOfMembers: number | null;
    occupationOfMember: number | null;
    cateOfSociety: string;
    debtsOfMembers: string;
    areaMortgaged: string;
    detailsOfShares: string;
    valueOfShare: string;
    modeOfPayment: string;
    nameAndAddressPromoters: string;
    dateOfCreation: string | null;
    dateofApplicationReceived: string | null;
   dateOfApproval: string | null;
    resolutionFilePath: string;
    formDFilePath: string;
    formB: boolean | null;
    formC: boolean | null;
    formD: boolean | null;
    formE: boolean | null;
    byLawsDocs: boolean | null;
    challan: boolean | null;
    userId: number | null;
    createDate: string;
    finalSubmitDate: string | null;
    isActive: boolean | null;
    status: number | null;
    inspectorCode: number | null;
    automaticallyApproved: number | null;
    name1: string | null;
    mobile1: string | null;
    email1: string | null; 
}

export interface SocietiesMembersList {
  rowNum: number;
  inspectName: string;
  societyName: string;
  societyTransID: string;
  committeMemberID: number | null;
  memberSNo: number;
  memberName: string;
  fatherName: string;
  gender: string;
  age: number | null;
  occupationCode: number | null;
  address1: string;
  address2: string;
  postOffice: string;
  pin: number | null;
  distCode: string;
  noOfShares: number | null;
  nomineeName: string;
  nomineeAge: number | null;
  relationshipCode: number | null;
  mobile: string;
  aadharNo: string;
  emailId: string;
  img: string;
  occupationVal: string;
  dob: string;
  flfile: string;
  imgg: string;
  extension: string;
  fullPath: string;
  loginID: number | null;
  iPAddress: string;
  browserName: string;
  rowStatus: number | null;
  isCustodian: number | null;
  mNG_RelationshipName: number | null;
  shareDateApproval: string;
  membershipNo: string;
  pFTCode: string;
  pFTValue: string;
  is_Litigation: string;
  caseInBrief: string;
  towerNo: string;
  courtNameId: number | null;
  is_Mortgage: string;
  mortgagedDetail: string;
  createdDate: string | null;
  onlyName: number | null;
  oTPValue: number | null;
  oTPSendDate: string | null;
  requestSendBy: number | null;
  isSent: boolean | null;
  sMSCount: number | null;
  userName: string;
  password: string;
  salt: string;
  affidavitDoc: string;
  identityDoc: string;
  flatType: number | null;
  flatArea: string;
}


export interface SocietiesDropDown {
  societyId: number;
  societiesName: string;
}

export interface DistrictDropDown {
  districtCode: string;
  districtName: string;
  coopDisCode: string;
}

export interface ARCSDropDown {
  arcsCode: number;
  arcsName: string;
}


export interface AuditorDropDown {
  audit_Auditor_Id: number;
  audit_Auditor_Name: string;
}

export interface MemberViewModel {
  rowNum: number;
  inspectName: string;
  societyTransID: string;
  committeMemberID: number;
  memberSNo: number;
  memberName: string;
  fatherName: string;
  gender: string;
  age: number;
  fullAddress: string;
  noOfShares: number;
  nomineeName: string;
  mobile: string;
  isSTCase:number;
}

export interface MembersListViewModel {
  data: MemberViewModel[];
  recordsCount: number;
}
