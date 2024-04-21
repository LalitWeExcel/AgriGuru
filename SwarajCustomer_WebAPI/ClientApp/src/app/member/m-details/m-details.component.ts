import { Component, OnInit,ElementRef, ViewChild } from '@angular/core';
import { IMemberDetails } from '../m-models/memberdetails';
import { MemberdetailsService } from '../m-services/memberdetails.service';
import { environment } from 'src/environments/environment';
import { memberAreaPath } from '../m-services/constants';
import { ToasterService } from 'src/app/services/toaster.service';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { JwtHelperService } from "@auth0/angular-jwt";
import { ActivatedRoute, ParamMap, Router } from "@angular/router";

@Component({
  selector: 'app-m-details',
  templateUrl: './m-details.component.html',
  styleUrls: ['./m-details.component.scss']
})
export class MDetailsComponent {
  memberDetails:IMemberDetails={
    societyTransID:'',
    memberSNo:0,
    memberName:'',
    fatherName:'',
    gender:'',
    age:0,
    occupationVal:'',
    address1:'',
    address2:'',
    postOffice:'',
    pin:0,
    distCode:0,
    noOfShares:0,
    nomineeName:'',
    nomineeAge:0,
    relationshipCode:0,
    mobile:'',
    aadharNo:'',
    emailId:'',
    dob:'',
    distName:'',
    relationship_Nominee_Name:'',
    relationship_WithMember_Name:'',
    occupationName:'',
    shareDateApproval:'',
    membershipNo:'',
    is_Mortgage:'',
    mortgagedDetail:'',
    newImageSrc:'',
    identityDoc:'',
    affidavitDoc:'',
    pftCode:'',
    pftValue:'',
    flatType:0,
    flatArea:'',
    towerNo:'',
    previousMemberRelation:'',
    transferorRNomineeName:'',
    courtNameId:0,
    courtName:'',
    caseInBrief:'',
    societyId:0,
    societyName:'',
    is_Litigation:'',
    
  };
  IsAuthorised = true;
  errorMessage = '';
  isLoading = true;
  loadingTitle = "Loading";
  memberId:any=0;
  userId: any;
  useridQP: number;
  roleId:any;
  roleName:any;
  firstName:any;
  userName: string;
  societyId: number;
  societyName: string;
  affidavidUrl=`${environment.baseURL+memberAreaPath}/GetPdfFile/11/`;
  identityUrl=`${environment.baseURL+memberAreaPath}/GetPdfFile/12/`;
  constructor(private toast: ToasterService,
    private memberService:MemberdetailsService,
    private jwtHelper: JwtHelperService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    ){
  }
  ngOnInit(){
    this.isUserAuthenticated();
    this.activatedRoute.queryParamMap.subscribe(params => {

      this.useridQP = Number(params.get("Id"));
      this.societyId = Number(params.get("sId"));
      this.societyName = String(params.get("sName"));
    });
     this.memberId=localStorage.getItem('memberSNo');
     if(this.memberId==0){
      this.memberId=sessionStorage.getItem('memberSNo');
     }
     this.userId=localStorage.getItem('userID');
     this.roleId=localStorage.getItem('roleID');
     this.roleName=localStorage.getItem('roleName');
     this.firstName=localStorage.getItem('firstName');
     this.userName=localStorage.getItem('username');
        
    this.getMemberDetails(this.memberId,this.memberDetails);  //4265
}
  getMemberDetails(MemberSNo:number,memberDetails: IMemberDetails){
    this.isLoading = true;
    this.loadingTitle = "Loading Details";
   this.memberService.getSocietyMemberDetails(MemberSNo)
      .subscribe({
    next:(response:IMemberDetails)=>{
      this.memberDetails=response
      this.affidavidUrl+=`${response.societyTransID}/${response.memberSNo}`;
      this.identityUrl+=`${response.societyTransID}/${response.memberSNo}`;
      this.isLoading = false;
      this.memberDetails.newImageSrc=this.memberDetails.newImageSrc==''?"/imgpath/dummy.jpg":this.memberDetails.newImageSrc;
    },
    error:(err:any)=>{
      this.isLoading = false;
      console.log(err)
      
    }

    }
    ) ;

  }
  isUserAuthenticated() {
    const token = localStorage.getItem("access_token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.IsAuthorised = true;
    } else {
      this.IsAuthorised = false;
    }
  }
  public openPDF(): void {
    this.isLoading = true;
    this.loadingTitle="Generating PDF";
    let DATA: any = document.getElementById('content');
    html2canvas(DATA).then((canvas) => {
      let fileWidth = 208;
      let fileHeight = (canvas.height * fileWidth) / canvas.width;
      const FILEURI = canvas.toDataURL('image/png');
      let PDF = new jsPDF('p', 'mm', 'a4');
      let position = 0;
      PDF.addImage(FILEURI, 'PNG', 0, position, fileWidth, fileHeight);
      PDF.save('memberdetails.pdf');
      this.isLoading=false;
    });
  }
   getDetails(response:any):void {
    this.memberDetails=response;
  }
  BackRequest() {
    this.router.navigate(['/admin-member-list', this.useridQP, this.societyId, this.societyName]);
  }
 

}
