import { SeriesHighlightingBehavior } from "igniteui-angular-charts";

export class ElectionRequest {
  electionRequestId: number;
  societyId: number;
  societyName: string;
  psName: string;
  address: string;
  societyTransactionId: string;
  userId: number;
  RemarksByPresident: string
  lastElectionHeldOn: string;
  dateOfIssue: string;
  dateOfHoldingGBMM: string;
  agendaHoldingGBMDoc: string;
  remarks: string;
  status: number;
  createdDate: string;
  updatedDate: string;
  createdBy: number;
  updatedBy: number;
  remarksByUser: string;
  inspectorAssignDate: string;
  reportSubmitedDate: string;

  submitedBy: string;
  inspectorId: number;
  inspectorName: string;
  remarksByInspector: string;
  inspectorassigndate: Date;
  totalMemberSociety: number;
  totalAttendMember: number;
  quorum: number;
  ismeetingasperrule: boolean;
  reportsubmiteddate: Date;
  recordKeeparId: number;
  recordKeeperName: string;
  remarksByRecordKeeper: string;
  recordKeeparAssignDate: string;
  isMembersSubmited?: boolean;
  isMeetingAsPerRule: boolean;
  selectedMember: string;
  isRecordKeeperVerified: boolean;
  isWithinTimeLimit:boolean;
  electionProceedingDoc:string;
  pO_BOA_Id:number;
  lastElectionRemarks:string;
}

export class electionRole {
  roleId: number;
  roleName: string;
}

export class User {
  userId: number;
  userName: string;
}

export class ARCSResponse_Election {
  electionRequestId: number;
  arcsCode: number;
  remarks: string;
  userId: number;
  updateDate: Date;
  assignedUserId: number;
  userAssignedDate: Date;
  assignedUserRoleId: number;
  remarksFor: number;
  module: string;
  status: boolean;
  selectedMemberList: Member[]
  dataFor: string;
  additionalRole:number;
}

export class Remarks {
  remarksId: number;
  electionId: number;
  msg: string;
  remarksTo: number;
  remarksBy: number;
  createdDate: Date;
  module: string;
}

export class Member {
  memberId: number;
  memberName: string;
  memberDesignationID: number;
  designation: string;
}

export class SocietyPreviousCommetteMember {
  memberId: number;
  memberName: string;
  memberDesignationID: number;
  designation: string;
}

export class ExecutiveMembers {
  executiveMemberId: number;
  electionId: number;
  memberId: number;
  memberName: string;
  roleId: number;
  roleName: string;
  createdBy: number;
  createdDate: Date;
  updatedBy: number;
  updatedDated: Date;
  status:boolean;
}

export enum ElectionRemarksFor {
  ElectionRequestFromPs = 1,
  ARCSAssignInsForElectionReport = 2,
  InsSaveElecReport = 3,
  ARCSAssignRecKeepVerifyRecord = 4,
  VerifyRecordKeepReport = 5,
  ARCSAssignInsForExecutiveMem = 6,
}

export class RemarkstrackingModel {
  remarksId: Number;
  requestId: Number;
  msg: String;
  remarksFor: Number;
  remarksTo: Number;
  remarksBy: Number;
  createdDate: Date;
  module: String;
  status: Boolean;
  remarksByName: String;
  byRole: String;
  remarksToName: String;
  toRole: String;
  societyId: number;
  inspectorAssignDate: string;
  psName: String;
  address: String;
  societyName: String;
  totalMemberSociety: number;
  totalAttendMember: number;
  quorum: number;
  isMeetingAsPerRule: boolean;
  reportSubmitedDate: string;
  recordKeeparId: number;
  recordKeeparAssignDate: string;
}

export class ActionStatus {
  public static actionStatus: string[] = ['', 'Election request has been submitted to the ARCS office',
    'The ARCS office assigns the Election file to the Inspector',
    'Inspector conducted the election and sent a report to the ARCS office.',
    'ARCS assigns the Election file to the Record Keeper for verification records',
    'Record keeper verified the records and sent to the ARCS office',
    'The election memberslist has been approved by ARCS'];

}
