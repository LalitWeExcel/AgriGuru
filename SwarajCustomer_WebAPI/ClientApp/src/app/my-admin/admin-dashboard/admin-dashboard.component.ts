import { Component } from "@angular/core";
import { AdminService } from "src/app/services/admin.service";
import { DashBoard, TotalSocitiesModel } from "src/app/shared/models/dash-board.model";
import { NgxSpinnerService } from "ngx-spinner";
import { JwtHelperService } from "@auth0/angular-jwt";

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent {

  _admimModel: TotalSocitiesModel;
  IsAuthorised = true;
   myvalues:any;
  

  constructor(
    private _AdminService: AdminService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService
  ) {
    this._admimModel = {} as TotalSocitiesModel;
  }

  ngOnInit() {
    this.SpinnerService.show();
    this.isUserAuthenticated();
    this.GetDashBoardData();
  }

  private GetDashBoardData() {
    this._AdminService.GetDashBoardData().subscribe(result => {
      this._admimModel.totalNewSocieties = result.data?.totalNewSocieties;
      this._admimModel.totalBackLogSocieties = result.data?.totalBackLogSocieties;
      this._admimModel.workingSociety = result.data?.workingSociety;
      this._admimModel.notWorkingSociety = result.data?.notWorkingSociety;
      this._admimModel.defunctSocieties = result.data?.defunctSocieties;
      this._admimModel.underARCS = result.data?.underARCS;
      this._admimModel.underInspector = result.data?.underInspector;
      this._admimModel.deemedApproval = result.data?.deemedApproval;
      this._admimModel.hearingSocieties = result.data?.hearingSocieties;
      this._admimModel.rejected = result.data?.rejected;
      this._admimModel.totalApproved = result.data?.totalApproved;
      this._admimModel.backlogSociety = result.data?.backlogSociety;


      console.log(this._admimModel);
      setTimeout(() => {
        this.SpinnerService.hide();
      }, 200);
      return this._admimModel;

    })
  }

  isUserAuthenticated() {
    const token = localStorage.getItem("access_token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.IsAuthorised = true;
    } else {
      this.IsAuthorised = false;
    }
  }


}
