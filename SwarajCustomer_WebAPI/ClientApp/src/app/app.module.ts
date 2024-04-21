import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { CarouselModule } from 'ngx-owl-carousel-o';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { AccountSelectionComponent } from './account-selection/account-selection.component';
import { CommanModule } from './comman/comman.module';
import { ByeLawsModule } from './bye-laws/bye-laws.module';
import { RcsModule } from './rcs/rcs.module';
import { MemberModule } from './member/member.module';
import { ElectionModule } from './election/election.module';
import { RegisterComponent } from './register/register.component';
import { HttpClientModule } from '@angular/common/http'; 

import {
  RECAPTCHA_SETTINGS,
  RecaptchaFormsModule,
  RecaptchaModule,
  RecaptchaSettings,
} from 'ng-recaptcha';
import { environment } from '../environments/environment';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';
import { JwtModule } from '@auth0/angular-jwt';
import { AuthGuard } from '../app/guards/auth-guard.service';
import { RoleModuleComponent } from './my-admin/role-module/role-module.component';
import { UserDispComponent } from './my-admin/user-disp/user-disp.component';
import { MyAdminModule } from './my-admin/my-admin.module';

import { NgxSpinnerModule } from 'ngx-spinner';
import { NgxPaginationModule } from 'ngx-pagination';
import { AuditModule } from './audit/audit.module';
import { HomeComponent } from './home/home.component';


export function tokenGetter() {
  return localStorage.getItem('access_token');
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AccountSelectionComponent,
    RegisterComponent,
    RoleModuleComponent,
    UserDispComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,       
    FontAwesomeModule,
    ToastrModule.forRoot({
      timeOut: 3000, // Time to close the toaster (in milliseconds)
      positionClass: 'toast-top-right', // Toast position
      closeButton: true, // Show close button
      progressBar: true, // Show progress bar
    }),
    ByeLawsModule,
    CommanModule,
    RcsModule,
    MemberModule,
    ElectionModule,
    HttpClientModule,
    RecaptchaModule,
    RecaptchaFormsModule,
    NgxSpinnerModule,
    NgxPaginationModule,
    MyAdminModule, 
    AuditModule,
    CarouselModule
  ],

  providers: [
    {
      provide: RECAPTCHA_SETTINGS,
      useValue: {
        siteKey: environment.recaptcha.RECAPTCHA_KEY,
      } as RecaptchaSettings,
    },
    { provide: JWT_OPTIONS, useValue: JWT_OPTIONS },
    JwtHelperService,
    AuthGuard,
  ],

  bootstrap: [AppComponent],
})
export class AppModule {}
