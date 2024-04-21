import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToasterService } from 'src/app/services/toaster.service';
import { ARCSResponse_Election, ElectionRemarksFor, ElectionRequest, ExecutiveMembers, Member, Remarks, RemarkstrackingModel, SocietyPreviousCommetteMember, User, electionRole } from 'src/app/shared/models/election/election_model';
import { ElectionServiceService } from 'src/app/services/election/election-service.service';
import { NgForm } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { SocietyDetailsModels } from 'src/app/shared/models/society-trans-model.model';
import { MemberdetailsService } from "../../services/member-services/memberdetails.service";

@Component({
  selector: 'app-election-arcs-inspector',
  templateUrl: './election-arcs-inspector.component.html',
  styleUrls: ['./election-arcs-inspector.component.scss'],
})
export class ElectionArcsInspectorComponent implements OnInit {
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
  public Users!:User[];
  ARCSRemarks_fetched!:Remarks;
  roleIds:number=16;
  Delimitor:string=',';
  selectedRole:any='';
  selectedUser:any='';
  ARCSCode:number;
  UserSelections = [];
  ARCSRemarks:string='';
  ElectionId:number;
  dataForRole:number=3;
  userId:number;
  arcsResponse_Election!:ARCSResponse_Election;
  module:string='Election';
  remarksList!: RemarkstrackingModel[];
  baseURl = environment.baseURL;
  societyPreviousCommetteMemberList: SocietyPreviousCommetteMember[] = [];
  memberList: Member[] = [];
  selectedExecutiveMembersList:ExecutiveMembers[];
  SocietyDetailsModels!: SocietyDetailsModels;

  constructor( private electionService: ElectionServiceService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService,
    private toaster: ToasterService,
    private _memberService: MemberdetailsService
    ) 
    {
    this.Users = [];
    this.arcsResponse_Election=new ARCSResponse_Election();
    this.ElectionRequestList.length=0;
  }
  ngOnInit() {
    this.SpinnerService.show();
    this.isUserAuthenticated();
    this.userid_loggedIn = Number(localStorage.getItem("userID"));
    this.GetARCSCode();
    this.GetElectionRequestsList();
    
  }

  GetSocietyDetails(userId: number, societyId: number): void {
    let params: any = {};
    params[`userId`] = userId;
    params[`societyId`] = societyId;

    this._memberService.GetSocietyDetail(params).subscribe({

      next: (response: any) => {
        this.SocietyDetailsModels = response.data;      
        console.log(this.SocietyDetailsModels);

       
       // this.SpinnerService.hide();
      }

    });
  }

  lastData: RemarkstrackingModel;

  loadRemarksTracking(rId:any) {
    debugger;
    this.electionService.getRemarksByRequest(rId,'Election').subscribe({
      next: (response: any) => {
      
        console.log(response);

        this.remarksList = response.data;
        this.lastData = this.remarksList.at(-1);
        
      },
      error: (error: any) => {
        console.log(error);
      }
    });
  }
  electionRequestRow: ElectionRequest;
  openRemarskModel(id: any, index: any) {
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
  GetARCSCode()
  {
    this.electionService.GetARCSCode({userId:this.userid_loggedIn}).subscribe({
      next:(response:any)=>{
        this.ARCSCode=response.data;
      }
    })
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
    dataForRole:number,
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
      this.dataForRole
    );
    this.electionService.GetElectionRequestsList(params).subscribe({
      next:(response: any)=>{
        // console.log(response.recordsCount);
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
    this.electionService.GetRoles({roleIds:this.roleIds,delemitor:this.Delimitor}).subscribe({
      next:(response:any)=>{
        this.Roles=response.data;
      },
      error:(err:any)=>{
        console.log(err);
      }
    })

  }

  GetRemarks(index){
    let remarksTo=this.ElectionRequestList[index].recordKeeparId;
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
    this.ElectionId= this.ElectionRequestList[index].electionRequestId;
    this.userId=this.ElectionRequestList[index].userId;
    console.log(this.ElectionId)
    this.GetRoles();
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

  report:ElectionRequest;
  viewReport(r:ElectionRequest){
    this.SpinnerService.show();
    this.report = r;
    this.inspectorId=r.inspectorId;
    //report = r;
    this.GetSocietyDetails(r.createdBy,r.societyId);
    this.GetSocietyMembers();

    


    var statusModal = document.getElementById('updateModal');
    if (statusModal != null) {
      statusModal.style.display = 'block';
    }

  }

  closeReportModel() {
    var statusModal = document.getElementById('updateModal');
    if (statusModal != null) {
      statusModal.style.display = 'none';
    }
  }
  GetSocietyMembers() {
    this.electionService.GetSocietyMembers({ socityTranId: this.report.societyTransactionId }).subscribe({
      next: (response: any) => {
        this.societyPreviousCommetteMemberList = response.data.societyPreviousCommetteMember;
        this.memberList = response.data.members;

        this.GetSelectedExecutiveMembers();
        // console.log(response);
      },
      error: (err: any) => {
        console.log(err);
        this.SpinnerService.hide();
      }
    })
  }

  GetSelectedExecutiveMembers()
  {
    this.electionService.GetSelectedExecutiveMembers({electionId: this.report.electionRequestId,createdBy:this.inspectorId}).subscribe({
      next:(response:any)=>{
        this.selectedExecutiveMembersList=response.data;
        this.SpinnerService.hide();

      },
      error:(err:any)=>{
        console.log(err);
        this.SpinnerService.hide();

      }
    })
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
    this.arcsResponse_Election.remarksFor=ElectionRemarksFor.ARCSAssignRecKeepVerifyRecord;
    this.electionService.approveElectionRequest(this.arcsResponse_Election).subscribe({
      next:(response:any)=>{
        debugger;
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
}
