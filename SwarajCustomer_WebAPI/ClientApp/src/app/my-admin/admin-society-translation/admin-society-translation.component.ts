import { Component } from "@angular/core";
import { NgxSpinnerService } from "ngx-spinner";
import { JwtHelperService } from "@auth0/angular-jwt";
import { MemberdetailsService } from "../../services/member-services/memberdetails.service";
import {
  SocietyViewModel,
  SocietyTransModel,
  SocietyDetailsModels,
  SocietiesMembersList,
  SocietiesDropDown,
  DistrictDropDown,
  ARCSDropDown
} from "../../shared/models/society-trans-model.model";
import { Router } from "@angular/router";

@Component({
  selector: "app-admin-society-translation",
  templateUrl: "./admin-society-translation.component.html",
  styleUrls: ["./admin-society-translation.component.scss"]
})
export class AdminSocietyTranslationComponent {

      public loading = false;
      public IsAuthorised = true;
      public societyid: number = 0;
      public divCode: number = 0;
      public arcsCode: number = 0;
      public arcsName: string = "";
      public pagenumber = 1;
      public pageSize = 10;
      public recordsCount = 0;
      public pageSizes = [10, 20, 50, 100, 200];
      public societyTranslist!: SocietyTransModel[];
      public SocietyDetailsModels!: SocietyDetailsModels;
      public SocietiesMembersList!: SocietiesMembersList[];
      public SocietiesDropDown!: SocietiesDropDown[];
      public DistrictDropDown!: DistrictDropDown[];
      public ARCSDropDown!: ARCSDropDown[];
  

  constructor(
    private _memberService: MemberdetailsService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService,
    private router: Router,
  ) {
    this.SocietyDetailsModels = {} as SocietyDetailsModels;
  }

  ngOnInit() {
    this.SpinnerService.show();
    this.isUserAuthenticated();
    this.GetDistrictDropDown();
    this.GetARCSDropDown();
    this.GetAllSocietyTransData();
  }

  GetAllSocietyTransData() {
 
    const params = this.RequestParams(this.pagenumber, this.pageSize, this.societyid, this.divCode, this.arcsCode);
    this.loading = true;
    this._memberService.GetAllSocietyTransData(params).subscribe({
      next: (response: any) => {

        this.societyTranslist = response.data;
        this.recordsCount = response.recordsCount;
        console.log(this.societyTranslist);

        this.loading = false;
        this.SpinnerService.hide();
      }
    });
  }

  GetDistrictDropDown() {
    this._memberService.GetDistrictDropDown().subscribe({
      next: (response: any) => {
        this.DistrictDropDown = response;
      }
    });
  }

  GetARCSDropDown() {
    let params: any = {};
    params[`divCode`] = this.divCode;

    this._memberService.GetARCSDropDown(params).subscribe({
      next: (response: any) => {
        this.ARCSDropDown = response.data;

      }
    });
  }


  GetSocietiesDropDown() {

    let params: any = {};
    params[`IsFlag`] = false;
    if (this.divCode) {
      params[`divCode`] = this.divCode;
    }
    if (this.arcsCode) {
      params[`arcsCode`] = this.arcsCode;
    }
 

    this._memberService.GetSocietiesDropDown(params).subscribe({
      next: (response: any) => {
        this.SocietiesDropDown = response.data;
      }
    });
  }


  onDistrictDropDownChange() {
   this.GetARCSDropDown();
    // this.GetSocietiesDropDown();// hide becuse Societies take time  to load  and Societies also depends on divCode & arcsCode
   this.GetAllSocietyTransData();
  }
  

  onSocietyDropDownChange() {
     this.GetAllSocietyTransData();
  }
  onARCDropDownChange() {
    this.GetSocietiesDropDown();
    this.GetAllSocietyTransData();
  }
  clearDropdowns() {

    this.arcsCode = 0;
    this.societyid = 0;
    this.divCode = 0;
   
    this.GetAllSocietyTransData();
  }

  GetSocietyDetail(userId: number, societyId:number): void {
    let params: any = {};
    params[`userId`] = userId;
    params[`societyId`] = societyId;

    this._memberService.GetSocietyDetail(params).subscribe({
      next: (response: any) => {

        this.SocietyDetailsModels = response.data;
        setTimeout(() => {
          this.SpinnerService.hide();
        }, 500);
      }
    });
  }

  GetMembersList(userId: number, societyId: number, societyName:string) {
    this.router.navigate(["admin-member-list", userId, societyId, societyName]);
 }
  
  isUserAuthenticated() {
    const token = localStorage.getItem("access_token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.IsAuthorised = true;
    } else {
      this.IsAuthorised = false;
    }
  }

  RequestParams(pagenumber: number, pageSize: number, societyid: number, divCode: number, arcsCode:number): any {
    let params: any = {};
    if (pagenumber) {
      params[`pagenumber`] = pagenumber;
    }

    if (pageSize) {
      params[`pageSize`] = pageSize;
    }
    if (societyid) {
      params[`societyid`] = societyid;
    }
    if (divCode) {
      params[`divCode`] = divCode;
    }
    if (arcsCode) {
      params[`arcsCode`] = this.arcsCode;
    }

    return params;
  }

  PageChanged(event: number): void {
    this.pagenumber = event;
    this.GetAllSocietyTransData();
  }

  PageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.pagenumber = 1;
    this.GetAllSocietyTransData();
  }
}
