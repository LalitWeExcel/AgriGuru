// import { ModuleWithProviders } from '@angular/core';

export interface IMemberDetails{
    societyTransID:string;
    memberSNo:number;
    memberName:string;
    fatherName:string;
    gender:string;
    age:number;
    occupationVal:string;
    address1:string;
    address2:string;
    postOffice:string;
    pin:number;
    distCode:number;
    noOfShares:number;
    nomineeName:string;
    nomineeAge:number;
    relationshipCode:number;
    mobile:string;
    aadharNo:string;
    emailId:string;
    dob:string;
    distName:string;
    relationship_Nominee_Name:string;
    relationship_WithMember_Name:string;
    occupationName:string;
    shareDateApproval:string;
    membershipNo:string;
    is_Mortgage:string;
    mortgagedDetail:string;
    newImageSrc:string;
    identityDoc:string;
    affidavitDoc:string;
    pftCode:string;
    pftValue:string;
    flatType:number;
    flatArea:string;
    towerNo:string;
    previousMemberRelation:string;
    transferorRNomineeName:string;
    courtNameId:number;
    courtName:string;
    caseInBrief:string;
    societyId:number;
    societyName:string;
    is_Litigation:string;
}

export class SocietyMembersListModal{
     inspectorCode=0;
    userID=0;
    inspectorName='';
    societyTransID='';
    societyId=0;
    societyName='';
    address1='';
    address2='';
    address3='';
    correspondenceAddress='';
    city='';
    noOfMembers=0;
    pin=0;
    memberSNo=0;
    memberName='';
}

export class Complaint {

    constructor(
      public  id:number,
      public complaintTitle: string,
      public complaintType: string,
      public description: string,
      public path?: string,
      public file:File|null=null
    ) { 
        this.complaintTitle='';
        this.complaintType='';
        this.description='',
        this.id=0;
        this.path=''
        this.file=null
     }
  
}


