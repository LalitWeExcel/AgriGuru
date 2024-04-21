import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { MemberdetailsService } from "../../services/member-services/memberdetails.service";
import { ElectionServiceService } from 'src/app/services/election/election-service.service';
import { ToasterService } from 'src/app/services/toaster.service';
import { ElectionRequest, ExecutiveMembers, Member, SocietyPreviousCommetteMember } from 'src/app/shared/models/election/election_model';
import { RemarkstrackingModel } from 'src/app/shared/models/remarks/remarks-tracking-model';
import { SocietyDetailsModels } from 'src/app/shared/models/society-trans-model.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-election-verification-recordkeepar',
  templateUrl: './election-verification-recordkeepar.component.html',
  styleUrls: ['./election-verification-recordkeepar.component.scss']
})
export class ElectionVerificationRecordkeeparComponent {

  //

  baseURl = environment.baseURL;
  pagenumber = 1;
  pageSize = 10
  recordsCount = 0;
  pageSizes = [10, 20, 50, 100];
  isLoading = true;
  loadingTitle = "Loading";
  electopnRequestList!: ElectionRequest[];
  remarksList!: RemarkstrackingModel[];
  id: number;
  remarks = "";

  societyPreviousCommetteMemberList: SocietyPreviousCommetteMember[] = [];
  memberList: Member[] = [];
  selectedExecutiveMembersList:ExecutiveMembers[];
  SocietyDetailsModels!: SocietyDetailsModels;

  constructor(private toast: ToasterService,
    private service: ElectionServiceService, private router: Router,
    private SpinnerService: NgxSpinnerService,
    private _memberService: MemberdetailsService,
  ) {

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
  ngOnInit() {
    this.SpinnerService.show();
    this.getElectionRequestList();
  }


  getElectionRequestList() {
    debugger;
    this.isLoading = true;
    this.loadingTitle = "Loading Request List";
    this.id = Number(localStorage.getItem("userID")!);
    const params = this.RequestParams(this.pagenumber, this.pageSize, this.id);
    this.service.getElectionRequestListForRecordKeepar(params).subscribe({
      next: (response: any) => {
        debugger
        this.isLoading = false;
        console.log(response);

        this.electopnRequestList = response.data;
        this.recordsCount = response.recordsCount;


      },
      error: (error: any) => {
        console.log(error);
      }
    });
  }

  RequestParams(pagenumber: number, pageSize: number, inspId: Number): any {
    let params: any = {};
    if (pagenumber) {
      params[`pagenumber`] = pagenumber;
    }

    if (pageSize) {
      params[`pageSize`] = pageSize;
    }
    params['rkeepId'] = inspId;
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


  report: ElectionRequest;

  viewReport(r: ElectionRequest) {
    this.report = r;
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

  viewStatus(r: ElectionRequest) {
    this.service.getRemarksByRequest(r.electionRequestId, 'Election').subscribe({
      next: (response: any) => {

        console.log(response);

        this.remarksList = response.data;
      },
      error: (error: any) => {
        console.log(error);
      }
    });


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

  viewReportModel() {
    var statusModal = document.getElementById('viewReport');
    if (statusModal != null) {
      statusModal.style.display = 'block';
    }
  }

  viewExecutiveMemberModel() {
    var statusModal = document.getElementById('viewMembers');
    if (statusModal != null) {
      statusModal.style.display = 'block';
    }
  }

  requestRemarks: ElectionRequest;

  addRemarks(rem: ElectionRequest) {
    this.requestRemarks = rem;
    var statusModal = document.getElementById('remarks');
    if (statusModal != null) {
      statusModal.style.display = 'block';
    }
  }

  // closeReportModel() {
  //   var statusModal = document.getElementById('viewReport');
  //   if (statusModal != null) {
  //     statusModal.style.display = 'none';
  //   }
  // }

  closeMemberModel() {
    var statusModal = document.getElementById('viewMembers');
    if (statusModal != null) {
      statusModal.style.display = 'none';
    }
  }

  closeRemarksModel() {
    var statusModal = document.getElementById('remarks');
    if (statusModal != null) {
      statusModal.style.display = 'none';
    }
  }

  newRemark: RemarkstrackingModel;
  saveRemarks() {
    //requestRemarks
    //SaveRemarksForRecordKeepar
    ///saveRemarksElectionRequest
    this.newRemark = new RemarkstrackingModel;
    this.newRemark.societyId = this.requestRemarks.societyId;
    this.newRemark.remarksBy = this.id;
    this.newRemark.requestId = this.requestRemarks.electionRequestId;
    this.newRemark.msg = this.remarks;

    this.service.saveRemarksElectionRequest(this.newRemark).subscribe({
      next: (response: any) => {
        console.log(response);
        this.getElectionRequestList();
        // this.remarksList = response.data;
        this.toast.success("","New Remarks Added");
        this.closeRemarksModel();
        this.remarks = "";
      },
      error: (error: any) => {
        console.log(error);
      }
    });
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

}
