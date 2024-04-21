import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToasterService } from 'src/app/services/toaster.service';
import { ARCSResponse_Election, ElectionRequest, Remarks, User, electionRole ,ElectionRemarksFor, SocietyPreviousCommetteMember, Member, RemarkstrackingModel, ExecutiveMembers} from 'src/app/shared/models/election/election_model';
import { ElectionServiceService } from 'src/app/services/election/election-service.service';
import { environment } from 'src/environments/environment';
import { MemberdetailsService } from "../../services/member-services/memberdetails.service";
import { SocietyDetailsModels } from 'src/app/shared/models/society-trans-model.model';

@Component({
  selector: 'app-election-history',
  templateUrl: './election-history.component.html',
  styleUrls: ['./election-history.component.scss']
})
export class ElectionHistoryComponent implements OnInit {
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
    this.SpinnerService.show();
    this.isUserAuthenticated();
    this.userid_loggedIn = Number(localStorage.getItem("userID"));
    this.GetARCSCode();
    this.GetElectionRequestsList();
  }

  public isUserAuthenticated() {
    const token = localStorage.getItem("access_token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.IsAuthorised = true;
    } else {
      this.IsAuthorised = false;
    }
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
