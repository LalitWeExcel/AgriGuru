import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToasterService } from 'src/app/services/toaster.service';

import {
  faHome,
  faTasks,
  faUserFriends,
  faNewspaper,
  faLandmark,
} from '@fortawesome/free-solid-svg-icons';



@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss'],
})
export class SideNavComponent {
  isLoading = false;
  loadingTitle = 'Loading';
  faHome = faHome;
  faTasks = faTasks;
  faUserFriends = faUserFriends;
  faNewspaper = faNewspaper;
  faLandmark = faLandmark;
  roleName: any;

  constructor(private router: Router, private toaster: ToasterService) {
    this.roleName = localStorage.getItem('roleName');
  }


  rcsPanle = false;
  byeLawsPanle = false;
  memberPanle = false;
  electionPanle = false;
  dashbaordPanle = false;
  arcsboardPanle = false;
  insboardPanle = false;
  recordkeeparPanle = false;
  auditPanle = false;

 
  ngOnInit() { 
 

    var url = this.router.url;
    if (url == '/rcs-home') {
      this.rcsPanle = true;
    } else if (url == '/rcs-amendment-request') {
      this.rcsPanle = true;
    } else if (url == '/member-home') {
      this.memberPanle = true;
    } else if (url == '/member-details' && this.roleName == 'Society Member') {
      this.memberPanle = true;
    } else if (
      url == '/member-backtrack' &&
      this.roleName == 'Society Member'
    ) {
      this.memberPanle = true;
    } else if (url == '/member-grivance') {
      this.memberPanle = true;
    } else if (url.includes('/election') && this.roleName == 'Inspector') {
      this.insboardPanle = true;

    } else if (url.includes("/election") && this.roleName === "Record Keepar") {
      this.recordkeeparPanle = true;
    } else if (url.includes('/election') && this.roleName == 'ARCS') {
      this.arcsboardPanle = true;
    } else if (url.includes('/election')) {
      this.electionPanle = true;
    } else if (url.includes('/admin') || this.roleName == 'Admin') {
      this.dashbaordPanle = true;
    } else if (url.includes('/audit')) {
      this.auditPanle = true;
    } else {
      this.byeLawsPanle = true;
    }
  }

  routToRequest() {
    console.log('request');
    this.router.navigate(['/bye-laws']);
  }

  routToRequestList() {
    console.log('request list');
    this.router.navigate(['/bye-laws-request-list']);
  }

  memberHome() {
    this.router.navigate(['/member-home']);
  }

  memberDetails() {
    //member-details
    this.router.navigate(['/member-details']);
  }
  memberBackTrack() {
    this.router.navigate(['/member-backtrack']);
  }

  grivance() {
    //member-grivance
    this.router.navigate(['/member-grivance']);
  }

  routToRCSHome() {
    this.router.navigate(['/rcs-home']);
  }

  routToRCSRequestList() {
    this.router.navigate(['/rcs-amendment-request']);
  }

  electionRequest() {
    this.router.navigate(['/election-request']);
  }

  electionDashboard() {
    this.router.navigate(['/election-dashboard']);
  }

  dashBoardRequest() {
    this.router.navigate(['/admin']);
  }
  userManagementRequest() {
    this.router.navigate(['/admin-ums']);
  }
  usersDetailRequest() {
    this.router.navigate(['/admin-users']);
  }
  usersSocietyType() {
    this.router.navigate(['/admin-SocietyType']);
  }
  newsfeedsRequest() {
    this.router.navigate(['/admin-news']);
  }

  SocietyTranslationRequest() {
    this.router.navigate(['/admin-society']);
  }
  AuditRequestRequest() {

    if (this.roleName == 'Chief Auditor  (CA) ') {
      this.router.navigate(['/audit-chief-auditor']);
    }
    else if (this.roleName == 'President') {
      this.router.navigate(['/audit-president-request']);
    }
    else if (this.roleName == 'Auditor') {
      this.router.navigate(['/audit-auditor-request']);
    }
    else if (this.roleName == 'Junior Auditor') {
      this.router.navigate(['/audit-junior-auditor']);
    }

  }
  AuditOfficerRequest(page: string) {
    if (page == 'audit-Officer-dashBoard') {
      this.router.navigate(['/audit-Officer-dashBoard']);
    }
    else if (page == "audit-Officer-audit-list") {
      this.router.navigate(['/audit-Officer-audit-list']);
    }
    else if (page == 'audit-Officer-audit-request') {
      this.router.navigate(['/audit-Officer-audit-request']);
    }
  }

  electionRequestList() {
    this.router.navigate(['/election-requestlist']);
  }

  ElectionRequestForIns() {
    this.router.navigate(['/election-requestforins']);
  }

  ElectionExecutiveMemberIns() {
    this.router.navigate(['/election-executive-member']);
  }

  ElectionRequestByARCS() {
    this.router.navigate(['/election-request-arcs-president']);
  }
  ElectionReportByInspector() {
    this.router.navigate(['/election-request-arcs-inspector']);
  }
  ElectionVerifiedByRecordKeeper() {
    this.router.navigate(['/election-request-arcs-recordkeeper']);
  }
  ElectionHistory() {
    this.router.navigate(['/election-history']);
  }
  ElectionReportList() {
    this.router.navigate(['/election-report-ins']);
  }
 
}
