import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { ToasterService } from "../services/toaster.service";
import { LoginService } from "../services/login.service";
import { hex_hmac_md5 } from "../../assets/ts/hex_hmac_md5";
import { LoginDataModal } from "../shared/models/login-user-details.model";
import { FormBuilder, FormControl, FormGroup, Validators, NgForm } from "@angular/forms";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  $_md5_key: string = "sblw-3hn8-sqoy19";
  showLogin: boolean = true;
  showLoginOtp: boolean = false;
  showForgotUi: boolean = false;
  showForgotOtpUi: boolean = false;
  showVerifyForgotOtpUi: boolean = false;
  Passwordfield: boolean = false;
  userName: string = "";
  password: string = "";
  isLoading: boolean = false;
  _loginModel: LoginDataModal;
  token: string | undefined;

  constructor(
    private router: Router,
    private toaster: ToasterService,
    private _LoginService: LoginService,
    private jwtHelper: JwtHelperService
  ) {
    this._loginModel = {} as LoginDataModal;
  }

  ngOnInit() {
    this.logOut();

  }

  ShowHide() {
    this.Passwordfield = !this.Passwordfield;
  }

  public signIn(form: NgForm): void {  
    
    //    if (form.invalid) {
    //     for (const control of Object.keys(form.controls)) {
    //       form.controls[control].markAsTouched();
    //     }
    //     return;
    //   }

    //console.debug(`Token [${this.token}] generated`);

    if (this.userName == "" && this.password == "") {
      this.toaster.error("", "Please enter all the field");
    } else if (this.userName == "") {
      this.toaster.error("Please enter User Name", "Error");      
    } else if (this.password == "") {
      this.toaster.error("Please enter Password", "Error");
      
    } else {

      var JsonData =   {
          UserName: hex_hmac_md5(this.$_md5_key, this.userName),
          Password: hex_hmac_md5(this.$_md5_key, this.password),
          DecodeUserName: this.userName,
          DecodePassword: this.password
      }
      this.isLoading  = true;

      this._LoginService.LoginUser(JsonData).subscribe(result => {
        
        if (result.data !== null && result.data.errorMessage.indexOf("html") < 0) {
         
          if (result.data.errorMessage.indexOf("successfully") >= 0) {
           
           
            this._loginModel.data = result.data;

            console.log(this._loginModel.data);

            this.toaster.success(result.data.errorMessage, "Success");
            localStorage.setItem("access_token", (<any>result).jwtToken);
            localStorage.setItem("userID", (<any>result.data.userID));
            localStorage.setItem("username", (<any>result.data.username));
            localStorage.setItem("firstName", (<any>result.data.firstName));
            localStorage.setItem("salt", (<any>result.data.salt));
            localStorage.setItem("isActive", (<any>result.data.isActive));
            localStorage.setItem("roleID", (<any>result.data.roleID));
            localStorage.setItem("roleName", (<any>result.data.roleName));
            localStorage.setItem("memberSNo", (<any>result.data.memberSNo));
            localStorage.setItem("arcsCode", (<any>result.data.arcsCode));
            localStorage.setItem("societyId", (<any>result.data.societyId));
  
            
            this.showLogin = false;
            this.showLoginOtp = true;
           
          } else {
            this.toaster.error(result.data.errorMessage, "Error");
            this.showLogin = true;
            this.showLoginOtp = false;
          }
        } else {
          this.toaster.error("Error occurred", "Error");
          this.showLogin = true;
          this.showLoginOtp = false;
        }
      });
    }
  }


  OTPVerify() {

    if (this._loginModel.data.roleName == "Admin") {
      this.router.navigate(["/admin"]);
    } else {
      this.router.navigate(["/account"]);
    }

  }

  backtologin() {
    this.router.navigate(["/login"]);
    this.showLogin = true;
    this.showLoginOtp = false;
    location.reload();
  }

  showForgotPassword() {
    this.showLogin = false;
    this.showForgotUi = true;

  }

  requestOtpForFp() {
    this.showForgotUi = false;
    this.showForgotOtpUi = true;

  }

  changeNumber() {
    this.showForgotUi = true;
    this.showForgotOtpUi = false;
  }

  verifyOtpForForgotPass() {
    this.showVerifyForgotOtpUi = true;
    this.showForgotOtpUi = false;

  }

  saveNewPassword() {
    alert("New Password Saved");
    this.showLogin = true;
    this.showVerifyForgotOtpUi = false;
  }

  register() {
    console.log("fasfdasf");
    this.router.navigate(['/user-register']);
  }

  isUserAuthenticated() {
    const token = localStorage.getItem("access_token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    } else {
      return false;
    }
  }

  public logOut() {
    localStorage.removeItem("access_token");
    localStorage.clear();
    // this.router.navigate(['']);
  }

}
