import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MHomeComponent } from './m-home/m-home.component';
import { CommanModule } from '../comman/comman.module';
import { MDetailsComponent } from './m-details/m-details.component';
import { MGrivanceComponent } from './m-grivance/m-grivance.component';
import { CKEditorModule } from 'ckeditor4-angular';
import { FormsModule } from '@angular/forms';
import { MMemberbacktrackComponent } from './m-memberbacktrack/m-memberbacktrack.component';
import { MSharedComponent } from './m-shared/m-shared.component';
import { ReactiveFormsModule } from '@angular/forms';
@NgModule({
  declarations: [
    MHomeComponent,
    MDetailsComponent,
    MGrivanceComponent,
    MMemberbacktrackComponent,
    MSharedComponent,
  ],
  imports: [
    CommonModule,
    CommanModule,
    CKEditorModule,
    FormsModule,
    ReactiveFormsModule
  ]
})  
export class MemberModule { }
