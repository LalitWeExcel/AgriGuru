 
import { Component } from "@angular/core";
import { UserDispService } from "../../../app/services/user-managment/user-disp.service";
import { Role } from "../../shared/models/role-wise-module.model";

import { NgxSpinnerService } from "ngx-spinner";
import { JwtHelperService } from "@auth0/angular-jwt";
import {
  usersModel,
  users,
  District,
  DistrictType,
  SocietyType
} from "../../shared/models/user-mangement-model.model";

import { faEye } from "@fortawesome/free-solid-svg-icons";
@Component({
  selector: "app-user-society-type",
  templateUrl: "./user-society-type.component.html",
  styleUrls: ["./user-society-type.component.scss"]
})
export class UserSocietyTypeComponent {
  pagenumber = 1;
  pageSize = 10;
  recordsCount = 0;
  pageSizes = [10, 20, 50, 100];
 
  IsAuthorised = true;

  public Role: Role[] = [];
  selectedRoleId: number = 0;
  selectedStatusId: number = -1;
  societyName: string = '';
  societyData = [];
  Status: any[] = [
    { id: 0, statusName: 'Active' },
    { id: 1, statusName: 'Inactive' },
 
  ];

  public District: District[] = [];
  public DistrictType: DistrictType[] = [];
  public SocietyType: SocietyType[] = [];

  selectedDisId: number = 0;
  selectedSocietyId: number = 0;
 
  radioOptions: string[] = ["Yes", "No"];
  selectedOption: any = "";
  selectedUserId: any;
  emailid: any;
  password: any;
  showpassword: boolean = true;
  visible: boolean = true;
  refreshTable: boolean = false;
  // Variables for filters
  societyFilter: string = '';
  roleFilter: string = '';
 
  faEye = faEye;

  constructor(
    private userservice: UserDispService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService
  ) { }

  ngOnInit(): void {
    this.SpinnerService.show();
    this.isUserAuthenticated();

    this.userservice.getData().subscribe((result) => {
 
      this.Role = result;
      setTimeout(() => {
        this.SpinnerService.hide();
      }, 100);
    });

    this.userservice.getDistrictItems().subscribe((data) => {
 
      this.District = data;
      setTimeout(() => {
        this.SpinnerService.hide();
      }, 100);
    });
    this.userservice.getDistrictTypeItems().subscribe((data) => {
 
      this.DistrictType = data;
      setTimeout(() => {
        this.SpinnerService.hide();
      }, 100);
    });
  }

  getSocietyNames(): string[] {
    // Use Set to store unique Societynames
    const uniqueSocietyNames = new Set<string>();

    this.SocietyType.forEach((user) => {
     // uniqueSocietyNames.add(user.societyName);
 
    });

    // Convert Set to array
    return Array.from(uniqueSocietyNames);
  }

  onRoleChange() {
    this.getCommonData();
    if (this.selectedRoleId != 0) {
      this.societyFilter = '';
 
    }
  }
  onDistrictChange() {
    this.getCommonData();

    if (this.selectedDisId !== 0) {
      this.societyFilter = '';
 
    } else {
      this.SocietyType = []; // Clear the table data if 'default' is selected
    }
  }

  onDistrictTypeChange() {
    this.getCommonData();

    if (this.selectedDisId !== 0) {
      this.societyFilter = '';
 
    } else {
      this.SocietyType = []; // Clear the table data if 'default' is selected
    }
  }

  private getCommonData(): void {
    this.userservice.getDataBySocietyType(this.selectedDisId, this.selectedSocietyId, decodeURIComponent(this.societyFilter),this.pagenumber, this.pageSize)
      .subscribe(response => {
        this.SocietyType = response.data; // Bind tableData'
        this.recordsCount = response.recordsCount;
      });
  }
  OnSocietyNameEnter(): void {
    if (this.societyFilter.length >= 3) {
      this.getCommonData();
    }
  }

  openUserDetailsModal(
    userId: number,
    emailID: string,
    isActive: number,
    password: string
  ): void {
    this.selectedUserId = userId;
    this.emailid = emailID;
 
    this.selectedOption = isActive === 1 ? "No" : "Yes";
    this.password = password;
  }

  onStatusChange(): void {
 
    const valueToSend: number = this.selectedOption === "Yes" ? 0 : 1;
    this.userservice
      .ChangeUserStatus(valueToSend, this.selectedUserId)
      .subscribe(data => {
        var a = data; // Assign the fetched data to your tableData
        this.onDistrictChange();
      });
  }

  onEmailSend(): void {
    this.userservice
      .UpdatePassword(this.emailid, this.password)
 
      .subscribe(data => {
        var a = data;
      });
  }

  public shouldDisplayUser(user: any): boolean {
    const isSocietyMatch =
 
      this.societyFilter === "" ||
      user.societyName.toLowerCase().includes(this.societyFilter.toLowerCase());
    return isSocietyMatch /*&& isRoleMatch*/;
  }

  clearFilters() {
    this.societyFilter = "";
    /*  this.roleFilter = '';*/
  }
  clearDropdowns() {
    this.selectedDisId = 0;
    this.selectedRoleId = 0;
    this.selectedStatusId = -1;
    this.societyFilter = '';
 
    this.SocietyType = [];
  }
  passwordtype() {
    this.visible = !this.visible;
    this.showpassword = !this.showpassword;
  }
  isUserAuthenticated() {
    const token = localStorage.getItem('access_token');
 
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.IsAuthorised = true;
    } else {
      this.IsAuthorised = false;
    }
  }
 
  PageChanged(event: number): void {
    this.pagenumber = event;
    this.getCommonData();
  }

  PageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.pagenumber = 1;
    this.getCommonData();
  }
}
