import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToasterService } from 'src/app/services/toaster.service';
import { ARCSResponse_Election, ElectionRequest, Remarks, User, electionRole, ElectionRemarksFor, RemarkstrackingModel } from 'src/app/shared/models/election/election_model';
import { ElectionServiceService } from 'src/app/services/election/election-service.service';
import { NgForm } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { SocietyDetailsModels } from '../../shared/models/society-trans-model.model';
import { MemberdetailsService } from '../../services/member-services/memberdetails.service';

@Component({
  selector: 'app-election-arcs-president',
  templateUrl: './election-arcs-president.component.html',
  styleUrls: ['./election-arcs-president.component.scss'],
})
export class ElectionArcsPresidentComponent implements OnInit {
  index: number;
  public loading = false;
  public pagenumber = 1;
  public pageSize = 10;
  public recordsCount = 0;
  public pageSizes = [10, 20, 50, 100];
  public userid_loggedIn:number=0;
  public societyid: number = 0;
  ElectionRequestList:ElectionRequest[]=[];
  public IsAuthorised=false;
  public inspectorName:string=''
  public inspectorId:number;
  public Roles!:electionRole[];
  Additional_Role!:electionRole[];
  public Users!:User[];
  userId:number;
  ARCSRemarks_fetched!:Remarks;
  roleIds:number=3;
  AdditionalRoleIds:string='3,21,22'
  AdditionalRoleId:number;
  Delimitor:string=',';
  selectedRole:any='';
  selectedUser:any='';
  ARCSCode:number;
  UserSelections = [];
  ARCSRemarks: string = '';
  statusText: string = '';
  ElectionId:number;
  status:number=0;
  dataForRole:number=1;
  arcsResponse_Election!: ARCSResponse_Election;
  SocietyDetailsModels!: SocietyDetailsModels;
  module:string='Election';
  remarksList!: RemarkstrackingModel[];
  baseURl = environment.baseURL;
    private _memberService: any;
  constructor(private electionService: ElectionServiceService, _memberService:MemberdetailsService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService,
    private toaster: ToasterService) 
    {
    this.Users = [];
    this.arcsResponse_Election=new ARCSResponse_Election();
  }
  ngOnInit() {
    this.SpinnerService.show();
    this.isUserAuthenticated();
    this.userid_loggedIn = Number(localStorage.getItem("userID"));
    this.GetARCSCode();
    this.GetElectionRequestsList();
  
  }
 

  GetARCSCode()
  {
    this.electionService.GetARCSCode({userId:this.userid_loggedIn}).subscribe({
      next:(response:any)=>{
        this.ARCSCode=response.data;
      }
    })
  }
 /* lastData: RemarkstrackingModel;*/
  lastData: RemarkstrackingModel;
  status1: number;
  loadRemarksTracking(rId:any) {
    this.electionService.getRemarksByRequest(rId,'Election').subscribe({
      next: (response: any) => {
      
        console.log(response);
        debugger
        this.remarksList = response.data; 
     /*   this.lastData = this.remarksList.at(-1);*/
        this.lastData = this.remarksList[this.remarksList.length - 1];
        this.status1 = this.status;
        if (this.lastData) {
          switch (this.lastData.remarksFor) {
            case 1:
              this.statusText = 'Election request has been submitted to the ARCS office';
              break;
            case 2:
              this.statusText = 'The ARCS office assigns the Election file to the Inspector';
              break;
            case 3:
              this.statusText = 'Inspector conducted the election and sent a report to the ARCS office.';
              break;
            case 4:
              this.statusText = 'ARCS assigns the Election file to the Record Keeper for verification records';
              break;
            case 5:
              this.statusText = 'Record keeper verified the records and sent to the ARCS office';
              break;
            case 6:
              this.statusText = 'The election members list has been approved by ARCS';
              break;
            default:
              this.statusText = 'Unknown status';
          }
        }
      },
      error: (error: any) => {
        console.log(error);
      }
    });
  }


  public isUserAuthenticated() {
    const token = localStorage.getItem("access_token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.IsAuthorised = true;
    } else {
      this.IsAuthorised = false;
    }
  }

  public RequestParams(
    pagenumber: number,
    pageSize: number,
    userId: number,
    societyid: number,
    status:number,
    dataForRole:number
  ): any {
    let params: any = {};
    if (pagenumber) {
      params[`pagenumber`] = pagenumber;
    }

    if (pageSize) {
      params[`pageSize`] = pageSize;
    }
    if (userId) {
      params[`userId`] = userId;
    }
    if (societyid) {
      params[`societyId`] = societyid;
    }
    if(status){
      params['status']=status;
    }
    if(dataForRole){
      params['dataForRole']=dataForRole;
    }
    return params;
  }

  GetElectionRequestsList(){
    const params = this.RequestParams(
      this.pagenumber,
      this.pageSize,
      this.userid_loggedIn,
      this.societyid,
      this.status,
      this.dataForRole
    );
    this.electionService.GetElectionRequestsList(params).subscribe({
      next:(response: any)=>{
         console.log(response.data);
        this.ElectionRequestList=response.data;
        this.recordsCount=response.recordsCount;
        this.loading = false;
        this.SpinnerService.hide();
      },
      error:(err:any)=>{
        console.log(err);
      }
    });
  }
  public PageChanged(event: number): void {
    this.pagenumber = event;
    this.GetElectionRequestsList();
  }
  public PageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.pagenumber = 1;
    this.GetElectionRequestsList();
  }

  GetRoles(){
    this.roleIds=3;
    this.electionService.GetRoles({roleIds:this.roleIds,delemitor:this.Delimitor}).subscribe({
      next:(response:any)=>{
        this.Roles=response.data;
   
        console.log(this.Roles) ;
      },
      error:(err:any)=>{
        console.log(err);
      }
    })

  }

  GetAdditionalRole()
  { 
    this.AdditionalRoleIds='3,21,22';
    this.electionService.GetRoles({roleIds:this.AdditionalRoleIds,delemitor:this.Delimitor}).subscribe({
      next:(response:any)=>{
        this.Additional_Role=response.data;
        console.log(this.Additional_Role) ;
      },
      error:(err:any)=>{
        console.log(err);
      }
    })
  }

  GetRemarks(index){
    let remarksTo=this.ElectionRequestList[index].inspectorId;
    let electionId=this.ElectionRequestList[index].electionRequestId;
    this.electionService.GetRemarks({electionId:electionId,remarksTo:remarksTo,remarksBy:this.userid_loggedIn,module:this.module}).subscribe({
      next:(response:any)=>{
        this.ARCSRemarks_fetched=response.data;
        this.openApprovalDetailsModel();
        this.SpinnerService.hide();
      },
      error:(err:any)=>{
        console.log(err);
        this.SpinnerService.hide();
      }
    })
  }

  onRoleChange(roleId) {
    if(roleId.value!==''){
      this.selectedRole=roleId.value;
      this.GetUserBasedOnRole();    
    }
    else{
      this.Users.length=0;
      this.selectedUser='';
    }
    
  }
  onAdditionalRoleChange(role)
  {
    this.AdditionalRoleId=role.value;
  }
  GetUserBasedOnRole(){
    this.electionService.GetUsersBasedOnRole({roleId:this.selectedRole,ARCSCode:this.userId}).subscribe({
      next:(response:any)=>{
        this.Users=response.data;
      },
      error:(err:any)=>{
        console.log(err);
      }
    })
  }
 
  OpenArcsRequest(index): any {
    this.ElectionId= this.ElectionRequestList[index].electionRequestId
    this.userId=this.ElectionRequestList[index].userId
    console.log(this.userId)
    this.GetRoles();
    this.GetAdditionalRole();
    this.openStatusModel();
    this.index = index;
  }
  OpenArcsResponse(index){
    this.index=index;
    this.GetRemarks(index);
    this.SpinnerService.show();
   
  }

  AssignUser(user) {
    this.selectedUser = user.value;
  }

  openApprovalDetailsModel() {
    var approvalDetailsModel = document.getElementById('approvalDetailsModel');
    if (approvalDetailsModel != null) {
      approvalDetailsModel.style.display = 'block';
    }
   
  }

  closeApprovalDetailsModel() {
    var approvalDetailsModel = document.getElementById('approvalDetailsModel');
    if (approvalDetailsModel != null) {
      approvalDetailsModel.style.display = 'none';
    }
  }
  openStatusModel() {
    var statusModal = document.getElementById('statusModal');
    if (statusModal != null) {
      statusModal.style.display = 'block';
    }
   
  }
  closeStatusModel() {
    var statusModal = document.getElementById('statusModal');
    if (statusModal != null) {
      statusModal.style.display = 'none';
    }
   this.clearControls();
  }

  electionRequestRow: ElectionRequest;
  openRemarskModel(id: any,index:any) {
    debugger
    this.electionRequestRow = this.ElectionRequestList[index];
    this.loadRemarksTracking(id);
    var statusModal = document.getElementById('remarksModal');
    if (statusModal != null) {
      statusModal.style.display = 'block';
    }
  }

  closeRemarksModel() {
    var statusModal = document.getElementById('remarksModal');
    if (statusModal != null) {
      statusModal.style.display = 'none';
    }
  }
  markAllAsDirty(form: NgForm) {
    for (const control of Object.keys(form.controls)) {
      form.controls[control].markAsDirty();
      form.controls[control].markAllAsTouched();
    }
  }


  clearControls(){
    this.index = null;
    this.Users.length=0;
    this.Roles.length=0;
    this.selectedRole='';
    this.selectedUser='';
    this.ARCSRemarks='';
  }

  ApproveElectionOrder(frm:NgForm) {
    console.log(frm);
    if (frm.status==='INVALID') {
      this.markAllAsDirty(frm);
      return;
    }
    debugger;
    this.arcsResponse_Election.arcsCode=this.ARCSCode;
    this.arcsResponse_Election.assignedUserId=this.selectedUser;
    this.arcsResponse_Election.electionRequestId=this.ElectionId;
    this.arcsResponse_Election.remarks=this.ARCSRemarks;
    this.arcsResponse_Election.userAssignedDate=new Date();
    this.arcsResponse_Election.assignedUserRoleId=this.roleIds;
    this.arcsResponse_Election.updateDate=new Date();
    this.arcsResponse_Election.userId=this.userid_loggedIn;
    this.arcsResponse_Election.module=this.module;
    this.arcsResponse_Election.status=false;
    this.arcsResponse_Election.additionalRole=this.AdditionalRoleId;
    this.arcsResponse_Election.remarksFor=ElectionRemarksFor.ARCSAssignInsForElectionReport;
    this.electionService.approveElectionRequest(this.arcsResponse_Election).subscribe({
      next:(response:any)=>{
        console.log(response);
        this.toaster.success(response.massage)
        this.SpinnerService.show();
       this.GetElectionRequestsList();
       this.clearControls();
      },
      error:(err:any)=>{
        console.log(err);
      }
    })
    
    this.closeStatusModel();
  }
  public SendBackElectionToPresident(): void {

    this.arcsResponse_Election.arcsCode = this.ARCSCode;
    this.arcsResponse_Election.assignedUserId = this.selectedUser;
    this.arcsResponse_Election.electionRequestId = this.ElectionId;
    this.arcsResponse_Election.remarks = this.ARCSRemarks;
    this.arcsResponse_Election.userAssignedDate = new Date();
    this.arcsResponse_Election.assignedUserRoleId = this.roleIds;
    this.arcsResponse_Election.updateDate = new Date();
    this.arcsResponse_Election.userId = this.userid_loggedIn;
    this.arcsResponse_Election.module = this.module;
    this.arcsResponse_Election.status = false;
    this.arcsResponse_Election.remarksFor = ElectionRemarksFor.ARCSAssignInsForElectionReport;
    this.electionService.approveElectionRequest(this.arcsResponse_Election).subscribe({
      next: (response: any) => {
        console.log(response);
        this.toaster.success(response.massage)
        this.SpinnerService.show();
        this.GetElectionRequestsList();
        this.clearControls();
      },
      error: (err: any) => {
        console.log(err);
      }
    })

    this.closeStatusModel();
  }
  
}
