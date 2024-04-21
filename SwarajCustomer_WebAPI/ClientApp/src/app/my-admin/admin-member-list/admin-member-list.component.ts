import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, ParamMap, Router } from "@angular/router";
import { NgxSpinnerService } from "ngx-spinner";
import { JwtHelperService } from "@auth0/angular-jwt";
import { MemberdetailsService } from "../../services/member-services/memberdetails.service";
import { MemberViewModel } from "../../shared/models/society-trans-model.model";


@Component({
  selector: "app-admin-member-list",
  templateUrl: "./admin-member-list.component.html",
  styleUrls: ["./admin-member-list.component.scss"]
})
export class AdminMemberListComponent implements OnInit {
  public loading = false;
  pagenumber = 1;
  pageSize = 10;
  recordsCount = 0;
  pageSizes = [10, 20, 50, 100, 200];
  SocietiesMembersList!: MemberViewModel[];

  societyName: string = "";
  societyId: number = 0;
  userId: number = 0;

  IsAuthorised = true;

  constructor(
    private _memberService: MemberdetailsService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.SpinnerService.show();
    this.isUserAuthenticated();

    this.activatedRoute.paramMap.subscribe(params => {


      this.userId = Number(params.get("userId"));
      this.societyId = Number(params.get("societyId"));
      this.societyName = String(params.get("societyName"));
      this.GetMembersListOfSocities(this.userId, this.societyId);
    });
  }

  GetMembersListOfSocities(userId: number, societyId: number) {
    const params = this.RequestParams(
      userId,
      societyId,
      this.pagenumber,
      this.pageSize
    );
    this.loading = true;
    this._memberService.GetMembersListOfSocities(params).subscribe({
      next: (response: any) => {

        this.SocietiesMembersList = response.data;
        this.recordsCount = response.recordsCount;
        console.log(this.SocietiesMembersList);

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

  RequestParams(
    userID: number,
    societyId: number,
    pagenumber: number,
    pageSize: number
  ): any {
    let params: any = {};

    if (userID) {
      params[`userID`] = userID;
    }
    if (societyId) {
      params[`societyId`] = societyId;
    }
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
    this.GetMembersListOfSocities(this.userId, this.societyId);
  }

  PageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.pagenumber = 1;
    this.GetMembersListOfSocities(this.userId, this.societyId);
  }

  BackRequest() {
    this.router.navigate(['/admin-society']);
  }
  GetMembersDetails(memberSNo:any){
    sessionStorage.setItem('memberSNo',String(memberSNo));
    this.router.navigate(['/member-details'],{ queryParams: { Id: this.userId, sId: this.societyId, sName: this.societyName } });
  }

  GetMembersBackTrackDetails(memberSNo:any){
    sessionStorage.setItem('memberSNo',String(memberSNo));
    this.router.navigate(['/member-backtrack'],{ queryParams: { Id: this.userId, sId: this.societyId, sName: this.societyName } });
  }
}
