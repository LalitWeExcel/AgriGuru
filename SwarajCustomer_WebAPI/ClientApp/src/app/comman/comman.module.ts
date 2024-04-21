import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { SideNavComponent } from './side-nav/side-nav.component';
import { LoaderComponent } from './loader/loader.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CommonFilterComponent } from './common-filter/common-filter.component';
import { ElectionDashboardComponent } from './election-dashboard/election-dashboard.component';




@NgModule({
  declarations: [
    HeaderComponent,
    SideNavComponent,
    LoaderComponent,
    CommonFilterComponent,
    ElectionDashboardComponent
    
  ],
  imports: [
    CommonModule,
    FontAwesomeModule,
    
  ],
  exports:[
    HeaderComponent,
    SideNavComponent,
    LoaderComponent,
    CommonFilterComponent
    

  ],
  providers: []
})
export class CommanModule { }
