import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ElectionServiceService } from 'src/app/services/election/election-service.service';
import { ToasterService } from 'src/app/services/toaster.service';
import { ElectionRequest } from 'src/app/shared/models/election/election_model';
import { RemarkstrackingModel } from 'src/app/shared/models/remarks/remarks-tracking-model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-election-selectexecutive-member',
  templateUrl: './election-selectexecutive-member.component.html',
  styleUrls: ['./election-selectexecutive-member.component.scss']
})
export class ElectionSelectexecutiveMemberComponent {

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

  constructor(private toast: ToasterService,
    private service: ElectionServiceService, private router: Router,
    private SpinnerService: NgxSpinnerService
  ) {

  }


  ngOnInit() {
    this.SpinnerService.show();
    this.getElectionRequestList();
  }


  getElectionRequestList() {
    this.isLoading = true;
    this.loadingTitle = "Loading Request List";
    this.id = Number(localStorage.getItem("userID")!);
    const params = this.RequestParams(this.pagenumber, this.pageSize, this.id);
    this.service.getElectionRequestListForExecutiveMem(params).subscribe({
      next: (response: any) => {
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
    params['reqstId'] = inspId;
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

  report:ElectionRequest;

  viewReport(r:ElectionRequest){
    this.report = r;
    //report = r;
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

  viewStatus(r:ElectionRequest){
    this.service.getRemarksByRequest(r.electionRequestId,'Election').subscribe({
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

  

  closeStatusModel(){
    var statusModal = document.getElementById('statusModal');
    if (statusModal != null) {
      statusModal.style.display = 'none';
    }

  }

  viewmember(r:ElectionRequest){
    this.report = r;
   
    //report = r;
    var statusModal = document.getElementById('viewmember');
    if (statusModal != null) {
      statusModal.style.display = 'block';
    }

  }
  

  
  closeviewmemberModel() {
    var statusModal = document.getElementById('viewmember');
    if (statusModal != null) {
      statusModal.style.display = 'none';
    }
  }
}
