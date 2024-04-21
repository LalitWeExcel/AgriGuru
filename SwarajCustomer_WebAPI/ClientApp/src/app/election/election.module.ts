import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommanModule } from '../comman/comman.module';
import { ElectionRequestComponent } from './election-request/election-request.component';
import { CKEditorModule } from 'ckeditor4-angular';
import { FormsModule, ReactiveFormsModule, FormGroup } from '@angular/forms';
 
import { ElectionRequestListComponent } from './election-request-list/election-request-list.component';
import { ElectionRequstForInsComponent } from './election-requst-for-ins/election-requst-for-ins.component';
import { ElectionSelectexecutiveMemberComponent } from './election-selectexecutive-member/election-selectexecutive-member.component';
import { ElectionVerificationRecordkeeparComponent } from './election-verification-recordkeepar/election-verification-recordkeepar.component';
import { ElectionArcsPresidentComponent } from './election-arcs/election-arcs-president.component';
import { ElectionArcsInspectorComponent } from './election-arcs/election-arcs-inspector.component';
import { ElectionArcsRecordkeeperComponent } from './election-arcs/election-arcs-recordkeeper.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';

import { ElectionReportInsComponent } from './election-report-ins/election-report-ins.component';
import { NgxBootstrapMultiselectModule } from 'ngx-bootstrap-multiselect';
import { NgxSpinnerModule } from "ngx-spinner";
import { NgxPaginationModule } from 'ngx-pagination';
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';

@NgModule({
  declarations: [
    ElectionRequestComponent,
    ElectionRequestListComponent,
    ElectionRequstForInsComponent,
    ElectionSelectexecutiveMemberComponent,
    ElectionVerificationRecordkeeparComponent,
    ElectionArcsPresidentComponent,
    ElectionArcsInspectorComponent,
    ElectionArcsRecordkeeperComponent,
    ElectionReportInsComponent
 
  ],
  imports: [
    CommonModule,
    CommanModule,
    CKEditorModule,
    FormsModule,
    ReactiveFormsModule,
    NgMultiSelectDropDownModule,
    NgxBootstrapMultiselectModule,
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
})
export class ElectionModule {}
