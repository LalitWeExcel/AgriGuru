import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ByeLawsRequestService } from 'src/app/services/bye-laws/bye-laws-request.service';
import { ToasterService } from 'src/app/services/toaster.service';
import { ByeLawsRequest, ByeLwasRequestListModel } from 'src/app/shared/models/bye-laws-request-list.model';
import { NgxSpinnerService } from "ngx-spinner";
@Component({
  selector: 'app-bl-request-list',
  templateUrl: './bl-request-list.component.html',
  styleUrls: ['./bl-request-list.component.scss']
})
export class BlRequestListComponent {

  pagenumber = 1;
  pageSize = 10
  recordsCount = 0;
  pageSizes = [10,20,50,100];

  byeLaswsRequestList!: ByeLwasRequestListModel[];
  isLoading = true;
  loadingTitle = "Loading";
  
  constructor(private toast: ToasterService,
    private service: ByeLawsRequestService, private router: Router,
    private SpinnerService: NgxSpinnerService
  ) {

  }
 
  ngOnInit() {
    this.SpinnerService.show();  
     this.getByeLawsRequestList();
  }

  getByeLawsRequestList() {
    this.isLoading = true;
    this.loadingTitle = "Loading Request List";
    const params = this.RequestParams(this.pagenumber, this.pageSize); 
    this.service.getBayeLawsRequestList(params).subscribe({
      next: (response: any) => {
        debugger;
        this.isLoading = false;
        console.log(response);

        this.byeLaswsRequestList = response.data;
        this.recordsCount = response.recordsCount;

        setTimeout(() => {
          this.SpinnerService.hide();
        }, 1000);

      }})
  }

  RequestParams(pagenumber: number, pageSize: number): any {
    let params: any = {};
    if (pagenumber) {
      params[`pagenumber`] = pagenumber;
    }

    if (pageSize) {
      params[`pageSize`] = pageSize;
    }
    return params;
  }

  PageChanged(event: number): void {
    this.pagenumber = event;
    this.getByeLawsRequestList();
  }

  PageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.pagenumber = 1;
    this.getByeLawsRequestList();
  }
}
