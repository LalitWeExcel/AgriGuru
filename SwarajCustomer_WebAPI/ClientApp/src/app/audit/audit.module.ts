import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommanModule } from '../comman/comman.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';
import { AuditservicesService } from '../services/auditservices.service';
import { CKEditorModule } from 'ckeditor4-angular';
import { AuditPresidentRequestComponent } from './audit-president-request/audit-president-request.component';
import { RouterModule } from '@angular/router';
import { AuditAuditorRequestComponent } from './audit-auditor-request/audit-auditor-request.component';
import { AuditJuniorAuditorComponent } from './audit-junior-auditor/audit-junior-auditor.component';
import { ChiefAuditorComponent } from './chief-auditor/chief-auditor.component';
import { AuditOfficerRequestSendComponent } from './audit-officer/audit-officer-request-send/audit-officer-request-send.component';
import { AuditOfficerDashBoardComponent } from './audit-officer/audit-officer-dash-board/audit-officer-dash-board.component';
import { AuditOfficerAuditListComponent } from './audit-officer/audit-officer-audit-list/audit-officer-audit-list.component';

 
@NgModule({
  declarations: [
    AuditAuditorRequestComponent,
    AuditJuniorAuditorComponent,
    AuditPresidentRequestComponent,
    ChiefAuditorComponent,
    AuditOfficerAuditListComponent,
    AuditOfficerRequestSendComponent,
    AuditOfficerDashBoardComponent
  ],
  imports: [
    CommonModule,
    CommanModule,
    ReactiveFormsModule,
    RouterModule,
    FormsModule,
    CKEditorModule,
    CommanModule,
    ReactiveFormsModule,
    FormsModule,
    NgxPaginationModule,
    NgxSpinnerModule.forRoot({ type: 'ball-scale-multiple' }),
    NgxLoadingModule.forRoot({
      animationType: ngxLoadingAnimationTypes.rectangleBounce,
      backdropBackgroundColour: 'rgba(0,0,0,0.1)',
      backdropBorderRadius: '10px',
      primaryColour: '#ffffff',
      secondaryColour: '#ffffff',
      tertiaryColour: '#ffffff',
      fullScreenBackdrop: true,
    }),

  ],
  providers: [AuditservicesService]
})
export class AuditModule { }
