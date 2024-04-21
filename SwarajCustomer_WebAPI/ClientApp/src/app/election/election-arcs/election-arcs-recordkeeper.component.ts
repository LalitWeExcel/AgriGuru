import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToasterService } from 'src/app/services/toaster.service';
import { ARCSResponse_Election, ElectionRequest, Remarks, User, electionRole ,ElectionRemarksFor, SocietyPreviousCommetteMember, Member, RemarkstrackingModel, ExecutiveMembers} from 'src/app/shared/models/election/election_model';
import { ElectionServiceService } from 'src/app/services/election/election-service.service';
import { NgForm } from '@angular/forms';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { environment } from 'src/environments/environment';
import { MemberdetailsService } from "../../services/member-services/memberdetails.service";
import { SocietyDetailsModels } from 'src/app/shared/models/society-trans-model.model';

@Component({
  selector: 'app-election-arcs-recordkeeper',
  templateUrl: './election-arcs-recordkeeper.component.html',
  styleUrls: ['./election-arcs-recordkeeper.component.scss'],
  encapsulation: ViewEncapsulation.None // Add this line
})
export class ElectionArcsRecordkeeperComponent implements OnInit {
  dropdownSettings: IDropdownSettings = {};
  selectedExecutiveMember:Member[] = [];
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
  isSubmited:boolean=false;
  seletedUserName:string='';
  ARCSCode:number;
  UserSelections = [];
  ARCSRemarks:string='';
  ElectionId:number;
  dataForRole:number=16;
  arcsResponse_Election!:ARCSResponse_Election;
  societyPreviousCommetteMemberList:SocietyPreviousCommetteMember[]=[];
  memberList:Member[]=[];
  module:string='Election';
  societyTransID='';
  remarksList!: RemarkstrackingModel[];
  baseURl = environment.baseURL;
  selectedExecutiveMembersList:ExecutiveMembers[];
  SocietyDetailsModels!: SocietyDetailsModels;

  constructor( private electionService: ElectionServiceService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService,
    private toaster: ToasterService,
    private _memberService: MemberdetailsService) 
    {
      this.Users = [];
      this.arcsResponse_Election=new ARCSResponse_Election();
      this.ElectionRequestList.length=0;
  }
  ngOnInit() {
    this.dropdownSettings = {
      idField: 'memberId',
      textField: 'memberName',
      allowSearchFilter: true,
      enableCheckAll: false,
      selectAllText: 'Select All Items From List',
      unSelectAllText: 'UnSelect All Items From List',
      noDataAvailablePlaceholderText: 'There is no item availabale to show',
      maxHeight: 197,
    };
   
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
  GetARCSCode()
  {
    this.electionService.GetARCSCode({userId:this.userid_loggedIn}).subscribe({
      next:(response:any)=>{
        this.ARCSCode=response.data;
      },
      error:(err:any)=>{
        console.log(err);
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
      this.seletedUserName='';
    }
    
  }
  GetUserBasedOnRole(){
    this.electionService.GetUsersBasedOnRole({roleId:this.selectedRole,ARCSCode:this.ARCSCode}).subscribe({
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
    this.societyTransID=this.ElectionRequestList[index].societyTransactionId;
    this.selectedUser=this.ElectionRequestList[index].inspectorId;
    this.seletedUserName=this.ElectionRequestList[index].inspectorName;
   // console.log(this.ElectionId);
    this.GetSocietyMembers();
    // this.GetRoles();
    this.openStatusModel();
    this.index = index;
  }
  OpenArcsResponse(index){
    this.index=index;
    this.GetRemarks(index);
    this.SpinnerService.show();
   
  }

  loadRemarksTracking(rId:any) {
    debugger;
    this.electionService.getRemarksByRequest(rId,'Election').subscribe({
      next: (response: any) => {
      
        console.log(response);

        this.remarksList = response.data; 

        
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

  openselectedExecutiveMemberkModel(id:any) {
    this.ElectionId=id;
    this.GetSelectedExecutiveMembers();
    var statusModal = document.getElementById('selectedExecutiveMemberModal');
    if (statusModal != null) {
      statusModal.style.display = 'block';
    }
  }

  closeselectedExecutiveMemberModel() {
    var statusModal = document.getElementById('selectedExecutiveMemberModal');
    if (statusModal != null) {
      statusModal.style.display = 'none';
    }
    this.clearControls();
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
  
  markAllAsDirty(form: NgForm) {
    for (const control of Object.keys(form.controls)) {
      form.controls[control].markAsDirty();
      form.controls[control].markAllAsTouched();
    }
  }


  clearControls(){
    this.index = null;
    this.ARCSRemarks='';
    this.selectedExecutiveMember.length=0;
  }

  onItemSelect(item: any) {
    //console.log('onItemSelect', item);
    // this.selectedMembers.length = this.selectedMembers.length + 1;
    this.selectedExecutiveMember.push(item);
    // console.log('Selected members :', this.selectedExecutiveMember);
  }
  onItemDeSelect(item: any) {
   // console.log('onItemDeSelect', item);
    this.selectedExecutiveMember.forEach((member) => {
      if (member.memberId === item.memberId) {
        this.selectedExecutiveMember.splice(this.selectedExecutiveMember.indexOf(member), 1);
      }
    });
   // console.log('Selected members :', this.selectedExecutiveMember);
  }
  onSelectAll(items: any) {
    console.log('onSelectAll', items);
  }
  onUnSelectAll() {
    console.log('onUnSelectAll fires');
  }
 
  OpenMemberRequest(index): any {
    this.openMemberModel();
    this.index = index;
  }

  openMemberModel() {
    var statusModal = document.getElementById('memberModal');
    if (statusModal != null) {
      statusModal.style.display = 'block';
    }
  }
  closeMemberModel() {
    var statusModal = document.getElementById('memberModal');
    if (statusModal != null) {
      statusModal.style.display = 'none';
    }
   
  }
    isApprovedByARCS:boolean=false;
  ApproveElectionOrder(electionId:number) {
    console.log(electionId);
    this.electionService.saveARCSMemberStatus(electionId).subscribe({
      next:(response:any)=>{
        console.log(response );
        if(response.isSuccess)
        {
          this.toaster.success(response.massage)
          this.SpinnerService.show();
         this.GetElectionRequestsList();
         this.isApprovedByARCS=true;
        }
        this.closeReportModel();
       
      },
        error:(err:any)=>{
          console.log(err);
          this.closeReportModel();
        }
    })

    
    this.closeStatusModel();
    
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
  approvedStatus:boolean=false;
  GetSelectedExecutiveMembers()
  {
    this.electionService.GetSelectedExecutiveMembers({electionId: this.report.electionRequestId,createdBy:this.inspectorId}).subscribe({
      next:(response:any)=>{
        this.selectedExecutiveMembersList=response.data;
        const commonIdsObjects= this.selectedExecutiveMembersList.filter(x=>x.status===true)
        if(commonIdsObjects.length==this.selectedExecutiveMembersList.length)
        {
          this.approvedStatus=true;
        }
        else{
          this.approvedStatus=false;
        }
        this.SpinnerService.hide();
      },
      error:(err:any)=>{
        console.log(err);
        this.SpinnerService.hide();
      }
    })
  }
  
}
