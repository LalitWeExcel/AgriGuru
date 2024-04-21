import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToasterService } from 'src/app/services/toaster.service';



@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  constructor(private router: Router, private toaster: ToasterService) {

  }

  //account
  loginPanle = false;
  dashboardPanle = false;
  accountPanle = false;
  memberPanle = false;
  arcsboardPanle = false;
  username: string = "";
  role: string = "";
  id: string = "";
  roleName: string = "";

  ngOnInit() {
    var uName = localStorage.getItem("firstName");
    this.username = uName ? uName : 'User Name';
    this.role = localStorage.getItem("roleName")!;
    this.id = localStorage.getItem("userID")!;
    this.roleName = localStorage.getItem("roleName")!;

    var url = this.router.url;

    console.log(url);

    if (url == "/") {
      this.loginPanle = true;
    }
    else if (url == '/account') {
      this.loginPanle = false;
      this.accountPanle = true;
    }
    else if (url == "/member-home" || url == "/member-details" || url == "/member-grivance" || url == "/rcs-home" || url == "/rcs-amendment-request") {
      this.dashboardPanle = false;
      this.memberPanle = true;
    }
    else if (url.includes("/admin")) {
      this.dashboardPanle = true;
    }
    else if (url.includes("/audit") || this.roleName == "ARCS") {
      this.arcsboardPanle = true;
    }
    else {
      this.memberPanle = true;
    }
  }

  backToAccountSelection() {
    this.router.navigate(['/account']);
  }

  sibartext: boolean = true;

  public logOut() {
    localStorage.removeItem("access_token");
    localStorage.clear();
    this.router.navigate(['']);
  }
  sidebarhide() {
       
  }
}
