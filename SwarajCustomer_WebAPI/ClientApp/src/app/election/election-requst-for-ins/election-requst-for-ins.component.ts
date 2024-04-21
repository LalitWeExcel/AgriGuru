import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ElectionServiceService } from 'src/app/services/election/election-service.service';
import { ToasterService } from 'src/app/services/toaster.service';
import { ElectionRequest, Member, SocietyPreviousCommetteMember } from 'src/app/shared/models/election/election_model';
import { RemarkstrackingModel } from 'src/app/shared/models/remarks/remarks-tracking-model';
import { Utility } from 'src/app/shared/utility/utils';
import { environment } from 'src/environments/environment';
import { IDropdownSettings } from 'ng-multiselect-dropdown';

@Component({
  selector: 'app-election-requst-for-ins',
  templateUrl: './election-requst-for-ins.component.html',
  styleUrls: ['./election-requst-for-ins.component.scss']
})
export class ElectionRequstForInsComponent {


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
  dropdownSettings: IDropdownSettings = {};
  selectedExecutiveMember: Member[] = [];
  societyPreviousCommetteMemberList: SocietyPreviousCommetteMember[] = [];
  memberList: Member[] = [];
  societyTransID: string = '';

  
  noofmem:number=0;
  attendmem:number=0;
  curom:number=0;
  asPerRule:boolean=false;
  Membersss:string = "";


  constructor(private toast: ToasterService,
    private service: ElectionServiceService, private router: Router,
    private SpinnerService: NgxSpinnerService
  ) {

  }


  ngOnInit() {
    this.SpinnerService.show();
    this.getElectionRequestList();
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
  }


  getElectionRequestList() {
    this.isLoading = true;
    this.loadingTitle = "Loading Request List";
    this.id = Number(localStorage.getItem("userID")!);
    const params = this.RequestParams(this.pagenumber, this.pageSize, this.id);
    this.service.getElectionRequestListForInspReport(params).subscribe({
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
    params['inspId'] = inspId;
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

  request: ElectionRequest;
  updateReport(r: ElectionRequest) {
    this.request = r;
    this.GetSocietyMembers();
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
  GetSocietyMembers() {
    this.service.GetSocietyMembers({ socityTranId: this.request.societyTransactionId }).subscribe({
      next: (response: any) => {
        this.societyPreviousCommetteMemberList = response.data.societyPreviousCommetteMember;
        this.memberList = response.data.members;
        console.log(response);
        this.SpinnerService.hide();
      },
      error: (err: any) => {
        console.log(err);
        this.SpinnerService.hide();
      }
    })
  }


  saveReport() {



    debugger;
    Object.entries(this.selectedExecutiveMember)?.forEach(([key, value]) => {
      if (this.Membersss == "")
        this.Membersss += value.memberId;
      else
        this.Membersss += "," + value.memberId;

    });
    var date = new Utility();
    this.request.totalMemberSociety = this.noofmem;
    this.request.totalAttendMember = this.attendmem;
    this.request.quorum = this.curom;
    this.request.isMeetingAsPerRule = this.asPerRule;
    this.request.userId = this.id;
    this.request.reportSubmitedDate = date.formatDate(new Date());
    this.request.isMembersSubmited = false;
    this.request.recordKeeparAssignDate = date.formatDate(new Date());
    this.request.recordKeeparId = 0;
    this.request.isRecordKeeperVerified = false;
    this.request.dateOfHoldingGBMM='';
    this.request.dateOfIssue='';

    this.request.selectedMember = this.Membersss;
    console.log(this.request);

    var list = new FormData();

    var formData = new FormData();
    Object.entries(this.request)?.forEach(([key, value]) => {
      formData.append(key, value);
    });
     

    this.service.saveElectionRequest(formData).subscribe({
      next: (res: any) => {
        console.log(res, 'Response');
        this.isLoading = false;
        if (res.isSuccess) {
          this.toast.success(res.massage);

          this.router.navigate(['/election-report-ins']);
        } else {
          this.toast.info(res.massage);

        }
      },
      error: (err: any) => {
        this.isLoading = false;
        console.log(err, 'Error');
        this.toast.info(err.massage);
      },
    });

    var statusModal = document.getElementById('updateModal');
    if (statusModal != null) {
      statusModal.style.display = 'none';
    }

  }

  onItemSelect(item: any) {
    this.selectedExecutiveMember.push(item);
  }
  onItemDeSelect(item: any) {
    this.selectedExecutiveMember.forEach((member) => {
      if (member.memberId === item.memberId) {
        this.selectedExecutiveMember.splice(this.selectedExecutiveMember.indexOf(member), 1);
      }
    });
  }
  onSelectAll(items: any) {
    console.log('onSelectAll', items);
  }
  onUnSelectAll() {
    console.log('onUnSelectAll fires');
  }

}
