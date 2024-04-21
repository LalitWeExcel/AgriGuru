import { Component } from "@angular/core";
import { AdminService } from "src/app/services/admin.service";
import { NewsFeedModel } from "src/app/shared/models/dash-board.model";
import { NgxSpinnerService } from "ngx-spinner";
import { JwtHelperService } from "@auth0/angular-jwt";

@Component({
  selector: "app-admin-news",
  templateUrl: "./admin-news.component.html",
  styleUrls: ["./admin-news.component.scss"]
})
export class AdminNewsComponent {
  public loading = false;
  pagenumber = 1;
  pageSize = 10;
  recordsCount = 0;
  pageSizes = [10, 20, 50, 100];
  newsfeedslist!: NewsFeedModel[];
  IsAuthorised = true;

  constructor(
    private _AdminService: AdminService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.SpinnerService.show();
    this.isUserAuthenticated();
    this.GetNewsFeedsData();
  }

  GetNewsFeedsData() {
    const params = this.RequestParams(this.pagenumber, this.pageSize);
    this.loading = true;
    this._AdminService.GetNewsFeedsData(params).subscribe({
      next: (response: any) => {
        console.log(response);
        this.newsfeedslist = response.data;
        this.recordsCount = response.recordsCount;
        this.loading = false;
        this.SpinnerService.hide();
      }
    });
  }

  isUserAuthenticated() {
    const token = localStorage.getItem("access_token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.IsAuthorised = true;
    } else {
      this.IsAuthorised = false;
    }
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
    this.GetNewsFeedsData();
  }

  PageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.pagenumber = 1;
    this.GetNewsFeedsData();
  }
}
