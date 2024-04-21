import { Component } from '@angular/core';
import { UserDispService } from '../../../app/services/user-managment/user-disp.service';
import { Role } from '../../shared/models/role-wise-module.model';

import { NgxSpinnerService } from 'ngx-spinner';
import { JwtHelperService } from '@auth0/angular-jwt';
import {
  usersModel,
  users,
  District,
} from '../../shared/models/user-mangement-model.model';

import { faEye } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-user-disp',
  templateUrl: './user-disp.component.html',
  styleUrls: ['./user-disp.component.css'],
})
export class UserDispComponent {
  
  pagenumber = 1;
  pageSize = 10;
  recordsCount = 0;
  pageSizes = [10, 20, 50, 100];
  public users: users[] = [];
  IsAuthorised = true;

  isMobileValid: boolean = true;
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
  selectedDisId: number = 0;
  radioOptions: string[] = ['Yes', 'No'];
  selectedOption: any = '';
  selectedUserId: any;
  emailid: any;
  password: any;
  showpassword: boolean = true;
  visible: boolean = true;
  refreshTable: boolean = false;
  // Variables for filters
  societyFilter: string = '';
  roleFilter: string = '';
  mobile: string = '';
  email: string = '';
  faEye = faEye;
  uniqueSocietyNames = new Set<string>();

  constructor(
    private userservice: UserDispService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.email='';
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

  
   
  
  }

  getSocietyNames(): string[] {

    this.users.forEach((user) => {
      this.uniqueSocietyNames.add(user.societyName.toLowerCase());

    });

    // Convert Set to array
    return Array.from(this.uniqueSocietyNames);
  }

  getMobilenos(): string[] {
    const uniqueMobileNos = new Set<string>();
    this.users.forEach((user) => {
      uniqueMobileNos.add(user.mobile);
    });
    return Array.from(uniqueMobileNos);
  }

  onRoleChange() {
    this.GetRoleDistrict();
    if (this.selectedRoleId != 0) {
      this.societyFilter = '';
    }
  }
  onDistrictChange() {
    this.GetRoleDistrict();

    if (this.selectedDisId !== 0) {
      this.societyFilter = '';
    } else {
      this.users = []; // Clear the table data if 'default' is selected
    }
  }
 
  private getCommonData(): void {
    this.userservice.getDataByRoleDist(this.selectedDisId, this.selectedRoleId, decodeURIComponent(this.societyFilter), this.mobile, this.email, this.pagenumber, this.pageSize)
      .subscribe(response => {
        this.users = response.data; // Bind tableData'
        this.recordsCount = response.recordsCount;
      });
  }
  onEmailInputChange(): void {
    if (this.email.length >= 5) {
      this.getCommonData();
    }
  }
  OnMobileEnter(): void {
    this.isMobileValid = !isNaN(Number(this.mobile));

    if (this.mobile.length >= 5) {
      this.getCommonData();
    }
  }

  OnSocietyNameEnter(): void {
    if (this.societyFilter.length >= 3) {
      this.getCommonData();
    }
  }

  GetRoleDistrict(): void {
    this.getCommonData();
  }

  openUserDetailsModal(
    userId: number,
    emailID: string,
    isActive: number,
    password: string
  ): void {
    this.selectedUserId = userId;
    this.emailid = emailID;
    this.selectedOption = isActive === 1 ? 'No' : 'Yes';
    this.password = password;
  }

  onStatusChange(): void {
    const valueToSend: number = this.selectedOption === 'Yes' ? 0 : 1;
    this.userservice.ChangeUserStatus(valueToSend, this.selectedUserId)
      .subscribe(data => {
        var a = data; // Assign the fetched data to your tableData
        this.onDistrictChange();
      });
  }

  onEmailSend(): void {
    this.userservice.UpdatePassword(this.emailid, this.password).subscribe((data) =>
    {
        var a = data;
      });
  }

  public shouldDisplayUser(user: any): boolean {
    const isSocietyMatch =this.societyFilter === '' || user.societyName.toLowerCase().includes(this.societyFilter.toLowerCase());
    return isSocietyMatch /*&& isRoleMatch*/;
  }
  public getRowsValue(flag: any) {
    if (flag === null) {
      return this.users.length;
    } else {
      return this.users.filter((i) => i.isActive == flag).length;
    }
  }
  clearFilters() {
    this.societyFilter = '';
    /*  this.roleFilter = '';*/
  }
  clearDropdowns() {
    this.selectedDisId = 0;
    this.selectedRoleId = 0;
    this.selectedStatusId = -1;
    this.societyFilter = '';
    this.mobile = '';
    this.users = [];
    this.email='';
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
