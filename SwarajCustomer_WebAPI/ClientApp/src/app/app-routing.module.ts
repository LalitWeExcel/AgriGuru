import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountSelectionComponent } from './account-selection/account-selection.component';
import { LoginComponent } from './login/login.component';
import { BlHomeComponent } from './bye-laws/bl-home/bl-home.component';
import { BlRequestListComponent } from './bye-laws/bl-request-list/bl-request-list.component';
import { AmendmntRequestComponent } from './rcs/amendmnt-request/amendmnt-request.component';
import { MHomeComponent } from './member/m-home/m-home.component';
import { MDetailsComponent } from './member/m-details/m-details.component';
import { MGrivanceComponent } from './member/m-grivance/m-grivance.component';
import { RegisterComponent } from './register/register.component';
import { AuthGuard } from './guards/auth-guard.service';
import { RoleModuleComponent } from './my-admin/role-module/role-module.component';
import { AdminDashboardComponent } from './my-admin/admin-dashboard/admin-dashboard.component';
import { AdminNewsComponent } from './my-admin/admin-news/admin-news.component';
import { UserDispComponent } from './my-admin/user-disp/user-disp.component';
import { AdminSocietyTranslationComponent } from './my-admin/admin-society-translation/admin-society-translation.component';
import { AdminMemberListComponent } from './my-admin/admin-member-list/admin-member-list.component';
import { ElectionRequestComponent } from './election/election-request/election-request.component';
import { MMemberbacktrackComponent } from './member/m-memberbacktrack/m-memberbacktrack.component';
import { ElectionRequestListComponent } from './election/election-request-list/election-request-list.component';
import { ElectionRequstForInsComponent } from './election/election-requst-for-ins/election-requst-for-ins.component';
import { ElectionSelectexecutiveMemberComponent } from './election/election-selectexecutive-member/election-selectexecutive-member.component';
import { ElectionVerificationRecordkeeparComponent } from './election/election-verification-recordkeepar/election-verification-recordkeepar.component';
import { ElectionArcsPresidentComponent } from './election/election-arcs/election-arcs-president.component';
import { ElectionArcsInspectorComponent } from './election/election-arcs/election-arcs-inspector.component';
import { ElectionArcsRecordkeeperComponent } from './election/election-arcs/election-arcs-recordkeeper.component';
import { UserSocietyTypeComponent } from "./my-admin/user-society-type/user-society-type.component";
import { ElectionReportInsComponent } from './election/election-report-ins/election-report-ins.component';
import { HomeComponent } from './home/home.component';
import { CooperativeDetailsComponent } from './my-admin/cooperative-details/cooperative-details.component';
import { AuditPresidentRequestComponent } from './audit/audit-president-request/audit-president-request.component';
import { AuditAuditorRequestComponent } from './audit/audit-auditor-request/audit-auditor-request.component';
import { ElectionDashboardComponent } from './comman/election-dashboard/election-dashboard.component';
import { AuditJuniorAuditorComponent } from './audit/audit-junior-auditor/audit-junior-auditor.component';
import { ChiefAuditorComponent } from './audit/chief-auditor/chief-auditor.component';
import { AuditOfficerRequestSendComponent } from './audit/audit-officer/audit-officer-request-send/audit-officer-request-send.component';
import { AuditOfficerDashBoardComponent } from './audit/audit-officer/audit-officer-dash-board/audit-officer-dash-board.component';
import { AuditOfficerAuditListComponent } from './audit/audit-officer/audit-officer-audit-list/audit-officer-audit-list.component';


const routes: Routes = [
  {
    path: "",
    component: HomeComponent,
    title: "Home"
  },
  {
    path: "login",
    component: LoginComponent,
    title: "RCS | Login"
  },
  {
    path: "account",
    component: AccountSelectionComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "bye-laws",
    component: BlHomeComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "bye-laws-request-list",
    component: BlRequestListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "rcs-amendment-request",
    component: AmendmntRequestComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "member-home",
    component: MHomeComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "member-details",
    component: MDetailsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "member-backtrack",
    component: MMemberbacktrackComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "member-grivance",
    component: MGrivanceComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "election-request",
    component: ElectionRequestComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "election-requestlist",
    component: ElectionRequestListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "election-requestforins",
    component: ElectionRequstForInsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "election-executive-member",
    component: ElectionSelectexecutiveMemberComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'election-verification-record',
    component: ElectionVerificationRecordkeeparComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "member-grivance",
    component: MGrivanceComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "election-request",
    component: ElectionRequestComponent,
    canActivate: [AuthGuard],

  },
  {
    path: "election-request-arcs-president",
    component: ElectionArcsPresidentComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "election-request-arcs-inspector",
    component: ElectionArcsInspectorComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "election-request-arcs-recordkeeper",
    component: ElectionArcsRecordkeeperComponent,
    canActivate: [AuthGuard],

  },

  {
    path: "user-register",
    component: RegisterComponent
  },
  {
    path: "admin",
    component: AdminDashboardComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "admin-news",
    component: AdminNewsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "admin-ums",
    component: RoleModuleComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin-ums',
    component: RoleModuleComponent,
    canActivate: [AuthGuard]
  }
  ,
  {
    path: "admin-users",
    component: UserDispComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin-SocietyType',
    component: UserSocietyTypeComponent,
    canActivate: [AuthGuard]
  }
  ,
  {
    path: 'admin-users',
    component: UserDispComponent,
    canActivate: [AuthGuard]
  }
  ,
  {
    path: "admin-society",
    component: AdminSocietyTranslationComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "admin-cooperative",
    component: CooperativeDetailsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "admin-member-list/:userId/:societyId/:societyName",
    component: AdminMemberListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "audit-president-request",
    component: AuditPresidentRequestComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'audit-auditor-request',
    component: AuditAuditorRequestComponent,
    canActivate: [AuthGuard],
  },
  {
    path: "audit-junior-auditor",
    component: AuditJuniorAuditorComponent,
    canActivate: [AuthGuard]
  }
  ,
  {
    path: 'audit-chief-auditor',
    component: ChiefAuditorComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'audit-Officer-audit-list',
    component: AuditOfficerAuditListComponent,
    canActivate: [AuthGuard]
  },
  ,
  {
    path: 'audit-Officer-audit-request',
    component: AuditOfficerRequestSendComponent,
    canActivate: [AuthGuard]
  },
  ,
  {
    path: 'audit-Officer-dashBoard',
    component: AuditOfficerDashBoardComponent,
    canActivate: [AuthGuard]
  },

  
  {
    path: 'admin-SocietyType',
    component: UserSocietyTypeComponent,
    canActivate: [AuthGuard]
  }
  ,
  {
    path: 'election-report-ins',
    component: ElectionReportInsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'election-dashboard',
    component: ElectionDashboardComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
