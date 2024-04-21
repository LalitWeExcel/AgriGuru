
export interface CommanDropDown {
  id: number;
  name: string;
}
export interface AuditSaveRequest {
  auditPresidentRequestId: number;
  chief_audit_Officer_Id: number;
  audit_Officer_Id: number;
  audit_Auditor_Id: number;
  junior_Auditor_LoginId: number;
  society_President_LoginId: number;
  audit_SocietyId: number;
  arcsCode: number;
  audit_financial_year_Id: number;
  previousTurnOver: number;
  flags: boolean;
  isProfitable: boolean;
  auditStatus_Id: number;
  balanseSheet: string;
  treadingAccount: string;
  profitLossAccount: string;
  auditRemarks: string;
  filePath: string;
  toJrAuditor: boolean;
}

export interface AuditViewModel {
  data: view_GetAuditRequest[];
  recordsCount: number;
}  

export interface view_GetAuditRequest {
  rowNum: number;
  chief_audit_Officer_Id: number;
  audit_Officer_LoginId: number;
  auditor_LoginId: number;
  junior_Auditor_LoginId: number;
  audit_Office_Circle: string;
  auditor_Circle: string;
  juniorAuditor_Circle: string;
  auditStatus_Id: number;
  auditStatusName: string;
  statusClass: string;
  audit_FinancialYear_Id: number;
  financialYear: string;
  societyName: string;
  audit_PresidentRequest_Id: number;
  society_President_LoginId: number;
  audit_SocietyId: number;
  email_Id :string;
  mobile_No: string;
  aRCSCode: string;
  aRCSName: string;
  previousTurnOver: number;
  isProfitable: boolean;
  audit_PresidentRequestedDate: string;
  audit_AuditorAssignedDate: string;
  audit_AuditorOrderDate: string;
  audit_ForWardDate: string;
  audit_FinalSubmitDate: string;
  societyPresidentUserName: string;
  audit_Auditor: string;
  juniorAuditor: string;
  auditRemarks: string;
  president_BalanseSheetFile: string;
  president_TreadingAccountFile: string;
  president_ProfitLossAccountFile: string;
  auditFilePath: string;
  createdDate: string;
  audit_Reminder_Counter: number;
  isSelected: boolean;
  lastAudit_MonthYear: string;
  
}


