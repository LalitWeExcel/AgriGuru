import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { error } from 'jquery';
import { NgxSpinnerService } from 'ngx-spinner';
import { MemberdetailsService } from "../../services/member-services/memberdetails.service";
import { ElectionServiceService } from 'src/app/services/election/election-service.service';
import { ToasterService } from 'src/app/services/toaster.service';
import { ElectionRequest, ExecutiveMembers, Member, SocietyPreviousCommetteMember } from 'src/app/shared/models/election/election_model';
import { RemarkstrackingModel } from 'src/app/shared/models/remarks/remarks-tracking-model';
import { SocietyDetailsModels } from 'src/app/shared/models/society-trans-model.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-election-request-list',
  templateUrl: './election-request-list.component.html',
  styleUrls: ['./election-request-list.component.scss']
})
export class ElectionRequestListComponent {

  SocietyDetailsModels!: SocietyDetailsModels;
 baseURl = environment.baseURL;
  pagenumber = 1;
  pageSize = 10
  recordsCount = 0;
  pageSizes = [10, 20, 50, 100];
  isLoading = true;
  loadingTitle = "Loading";
  electopnRequestList!: ElectionRequest[];
  remarksList!: RemarkstrackingModel[];
  
  societyPreviousCommetteMemberList: SocietyPreviousCommetteMember[] = [];
  memberList: Member[] = [];
  selectedExecutiveMembersList:ExecutiveMembers[];

  constructor(private toast: ToasterService,
    private service: ElectionServiceService, private router: Router,
    private SpinnerService: NgxSpinnerService,
    private _memberService: MemberdetailsService,
  ) {

  }
  id:number;

  ngOnInit() {
    this.SpinnerService.show();
 

    this.id = Number(localStorage.getItem("userID")!);
    this.GetSocietyDetails(this.id, 0);

  }

  

  GetSocietyDetails(userId: number, societyId: number): void {
    let params: any = {};
    params[`userId`] = userId;
    params[`societyId`] = societyId;

    this._memberService.GetSocietyDetail(params).subscribe({

      next: (response: any) => {
        this.SocietyDetailsModels = response.data;      
        console.log(this.SocietyDetailsModels);

       this.getElectionRequestList();

       // this.SpinnerService.hide();
      }

    });
  }


  getElectionRequestList() {
    this.isLoading = true;
    this.loadingTitle = "Loading Request List"; 



    const params = this.RequestParams(this.pagenumber, this.pageSize,this.SocietyDetailsModels.societyId);
    this.service.getElectionRequestList(params).subscribe(
      {
      next: (response: any) => {
        this.isLoading = false;
        console.log(response);

        this.electopnRequestList = response.data;
        this.recordsCount = response.recordsCount;

        // setTimeout(() => {
        //   this.SpinnerService.hide();
        // }, 1000);

      },
      error: (error: any) => {
        console.log(error);
      }
    }
  );
  }

  loadRemarksTracking(rId:any) {
    debugger;
    this.service.getRemarksByRequest(rId,'Election').subscribe({
      next: (response: any) => {
      
        console.log(response);

        this.remarksList = response.data; 

        
      },
      error: (error: any) => {
        console.log(error);
      }
    });
  }

  RequestParams(pagenumber: number, pageSize: number, socityId:number): any {
    let params: any = {};
    if (pagenumber) {
      params[`pagenumber`] = pagenumber;
    }

    if (pageSize) {
      params[`pageSize`] = pageSize;
    }

    if (pageSize) {
      params[`societyId`] = socityId;
    }
    return params;
  }

  PageChanged(event: number): void {
    this.pagenumber = event;
    this.getElectionRequestList();
  }

  PageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.pagenumber = 1;
    this.getElectionRequestList();
  }

  openStatusModel(id:any) {
    this.loadRemarksTracking(id);
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
  }


  report: ElectionRequest;

  viewReport(r: ElectionRequest) {
    this.report = r;
    //report = r;
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
    this.service.GetSocietyMembers({ socityTranId: this.report.societyTransactionId }).subscribe({
      next: (response: any) => {
        this.societyPreviousCommetteMemberList = response.data.societyPreviousCommetteMember;
        this.memberList = response.data.members;

        this.GetSelectedExecutiveMembers();
        console.log(response);
        this.SpinnerService.hide();
      },
      error: (err: any) => {
        console.log(err);
        this.SpinnerService.hide();
      }
    })
  }


  GetSelectedExecutiveMembers()
  {
    this.service.GetSelectedExecutiveMembers({electionId: this.report.electionRequestId,createdBy:this.report.inspectorId}).subscribe({
      next:(response:any)=>{
        this.selectedExecutiveMembersList=response.data;
      },
      error:(err:any)=>{
        console.log(err);
      }
    })
  }

  electionrequest(){
    this.router.navigateByUrl('/election-request')
  }

}
