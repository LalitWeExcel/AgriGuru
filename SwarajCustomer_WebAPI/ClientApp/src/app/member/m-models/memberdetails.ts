

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

export class MemberDetails{
   constructor(
   public societyTransID:string,
   public memberSNo:number,
   public memberName:string,
   public fatherName:string,
   public gender:string,
   public age:number,
   public occupationVal:string,
   public address1:string,
   public address2:string,
   public postOffice:string,
   public  pin:number,
   public distCode:number,
   public noOfShares:number,
   public nomineeName:string,
   public nomineeAge:number,
   public relationshipCode:number,
   public mobile:string,
   public aadharNo:string,
   public emailId:string,
   public dob:string,
   public distName:string,
   public relationship_Nominee_Name:string,
   public relationship_WithMember_Name:string,
   public occupationName:string,
   public shareDateApproval:string,
   public membershipNo:string,
   public is_Mortgage:string,
   public mortgagedDetail:string,
   public newImageSrc:string,
   public identityDoc:string,
   public affidavitDoc:string,
   public pftCode:string,
   public pftValue:string,
   public flatType:number,
   public flatArea:string,
   public towerNo:string,
   public previousMemberRelation:string,
   public  transferorRNomineeName:string,
   public courtNameId:number,
   public courtName:string,
   public caseInBrief:string,
   public societyId:number,
   public societyName:string,
   public is_Litigation:string,
   public affidavidUrl:string,
   public identityUrl:string
   ){}
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

  export class ST_ContentFileUpload{
    constructor(
        public  id:number,
        public  uniqueId:string,
        public  shareTransferID:string,
        public  contentUpload:string,
       
        public  formId :string,
       
        public  file_Name :string,
        public  isMandatory :string,


    ){

    }
  }

export class SocietyMemberBackTrack{
    constructor(
     public   previousOwner:MemberDetails,
     public uploadedDocs:ST_ContentFileUpload[],
     public shareTransferID:string,
     public inspectorCode:number,
     public arcsCode:number,
        public shareTransferCertificate:string,
        public approvalDate:string,
        public certificateType:string,
        public owner:string,
        public uniqueId:string,
        public sT_PrimaryId:number,
        public distName:string,
        public transferor_Id:number,
        public transfereeId:number,
        public affidavit:string,
        public idProof:string,
        public relationWithTransferee:string,
        public relationshipName:string,
        public transfer_Reason:string,
        public plot_GH_No:string,
        public flat_No:string,

    )
{

}

}  