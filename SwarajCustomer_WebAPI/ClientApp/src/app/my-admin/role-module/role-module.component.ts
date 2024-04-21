import { Component } from "@angular/core";
import { UsermanagementserviceService } from "../../services/user-managment/usermanagementservice.service";
import { JwtHelperService } from '@auth0/angular-jwt';
import { RoleWiseModule, Role, Module ,RoleModule} from '../../shared/models/role-wise-module.model';
import { NgxSpinnerService } from "ngx-spinner";


import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
  NgForm,
  NgModel
} from "@angular/forms";
@Component({
  selector: "app-role-module",
  templateUrl: "./role-module.component.html",
  styleUrls: ["./role-module.component.scss"]
})
export class RoleModuleComponent {
  public Role: Role[] = [];
  public Module: Module[] = [];
  public RoleWiseModule: RoleWiseModule[] = [];
  selectedRoleId: number = 1;
  rolevalue: any;
  model: RoleModule;

  IsAuthorised: boolean = false;


  constructor(private usermanagement: UsermanagementserviceService,
    private jwtHelper: JwtHelperService,
    private SpinnerService: NgxSpinnerService
  ) {
    this.model = {
      ModuleId: 0,
      RoleId: 0,
      id: 0
    };
  }
  ngOnInit(): void {
    this.RoleWiseModule = [];
    this.SpinnerService.show();
    this.isUserAuthenticated();



    this.usermanagement.getData().subscribe(result => {
      this.Role = result;

      setTimeout(() => {
        this.SpinnerService.hide();
      }, 100);

    });

    this.usermanagement.getmodule().subscribe(result => {
      this.Module = result;

      setTimeout(() => {
        this.SpinnerService.hide();
      }, 100);

    });
    if (this.rolevalue != "default")
    this.onSelectChange(this.selectedRoleId);
  }

  onSelectChange(RoleId: number) {
    this.rolevalue = RoleId;
    if (this.rolevalue != "default") {

      this.SpinnerService.show();

      this.usermanagement
        .GetRoleWiseModuleId(RoleId, true)
        .subscribe(result => {
          this.RoleWiseModule = result;

          setTimeout(() => {
            this.SpinnerService.hide();
          }, 100);
        });
      this.usermanagement
        .GetRoleWiseModuleId(RoleId, false)
        .subscribe(result => {
          this.Module = result;

          setTimeout(() => {
            this.SpinnerService.hide();
          }, 100);
        });
    }
  }

  DeleteModule(Id: number, RoleId: number) {

    this.usermanagement.DeleteRoleModule(Id).subscribe(result => {
      var msg = result;
      this.usermanagement
        .GetRoleWiseModuleId(RoleId, true)
        .subscribe(result => {

          this.RoleWiseModule = result;

          this.usermanagement
            .GetRoleWiseModuleId(RoleId, false)
            .subscribe(result => {

              this.Module = result;
            });
        });
    });
  }
  addModule(Moduleid: number, RoleId: number) {

    const model = {
      Moduleid: Moduleid,
      RoleId: RoleId
    };

    this.usermanagement.postModule(Moduleid, RoleId).subscribe({
      next: (response: any) => {

        console.log("This was successful", response);
        this.usermanagement
          .GetRoleWiseModuleId(RoleId, true)
          .subscribe(result => {

            this.RoleWiseModule = result;
          });
        this.usermanagement
          .GetRoleWiseModuleId(RoleId, false)
          .subscribe(result => {
            this.Module = result;
          });
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
}


