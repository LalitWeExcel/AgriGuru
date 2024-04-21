import { Component } from '@angular/core';
import { AccountSectionService } from '../../app/services/account-section/account-section.service'
import { JwtHelperService } from '@auth0/angular-jwt';
import { RoleWiseModule } from '../shared/models/role-wise-module.model';
import { Router } from '@angular/router';
@Component({
  selector: 'app-account-selection',
  templateUrl: './account-selection.component.html',
  styleUrls: ['./account-selection.component.scss']
})
export class AccountSelectionComponent {

  IsAuthorised: boolean = false;
  RoleWiseModule: RoleWiseModule[] = [];
  isLoading = false;
  loadingTitle = "Loading";

  constructor(private _accountSectionService: AccountSectionService,
    private jwtHelper: JwtHelperService,
    private router: Router
  ) {
    this.RoleWiseModule = [] as RoleWiseModule[];
  }

  roleName = "";
  ngOnInit() {
    this.roleName = localStorage.getItem("roleName")!;
    this.isUserAuthenticated();
    this.GetRoleWiseModuleId();
  }


  isByeLaws = false;
  isAudit = false;
  isElection = false;
  isSocityProfile = false;
  isShareTransfer = false;
  isMember = false;

  GetRoleWiseModuleId() {

    this.isLoading = true;
    let roleID = localStorage.getItem("roleID");
    this.roleName = localStorage.getItem("roleName")!;

    this._accountSectionService.GetRoleWiseModuleId(roleID).subscribe(result => {
      this.isLoading = false;
      this.RoleWiseModule = result;


      for (var m of this.RoleWiseModule) {
        if (m.module == 'Bye-Laws') {
          this.isByeLaws = true;
        }

        if (m.module == 'Audit') {
          this.isAudit = true;
        }
        if (m.module == 'Election') {
          this.isElection = true;
        }
        if (m.module == 'SocityProfile') {
          this.isSocityProfile = true;
        }

        if (m.module == 'ShareTransfer') {
          this.isShareTransfer = true;
        }

        if (m.module == 'Member') {
          this.isMember = true;
        }
      }

      console.log(this.RoleWiseModule);
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

  public clickrouterlink(_roleName: string) {
    if (_roleName == 'Chief Auditor  (CA) ') {
      this.router.navigate(['/audit-chief-auditor']);
    }
    else if (_roleName == "Audit Officer  (AO) ") {
      this.router.navigate(['/audit-Officer-dashBoard']);
    }
    else if (_roleName == 'President') {
      this.router.navigate(['/audit-president-request']);
    }
    else if (_roleName == 'Auditor') {
      this.router.navigate(['/audit-auditor-request']);
    }
    else if (_roleName == 'Junior Auditor') {
      this.router.navigate(['/audit-junior-auditor']);
    }
  }

}
