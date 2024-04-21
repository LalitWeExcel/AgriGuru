import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { AdminNewsComponent } from './admin-news/admin-news.component';
import { AdminService } from 'src/app/services/admin.service';
import { CommanModule } from '../comman/comman.module';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxSpinnerModule } from "ngx-spinner";
import { NgxPaginationModule } from 'ngx-pagination';
import { AdminSocietyTranslationComponent } from './admin-society-translation/admin-society-translation.component'; 
import { AdminMemberListComponent } from './admin-member-list/admin-member-list.component';
import { UserSocietyTypeComponent } from './user-society-type/user-society-type.component';
import { NgxLoadingModule, ngxLoadingAnimationTypes } from 'ngx-loading';
import { CooperativeDetailsComponent } from './cooperative-details/cooperative-details.component';
interface NgxSpinnerConfig { type?: string; }


@NgModule({
  declarations: [
    AdminDashboardComponent,
    AdminNewsComponent,
    AdminSocietyTranslationComponent, 
    CooperativeDetailsComponent,
    AdminMemberListComponent, UserSocietyTypeComponent
  ],
  imports: [
    CommonModule,
    CommanModule, ReactiveFormsModule,
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
  providers: [AdminService]
})
export class MyAdminModule { }
