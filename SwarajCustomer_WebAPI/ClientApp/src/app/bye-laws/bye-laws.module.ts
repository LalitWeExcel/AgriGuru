import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BlSidenavComponent } from './bl-sidenav/bl-sidenav.component';
import { BlHomeComponent } from './bl-home/bl-home.component'; 
import { CommanModule } from '../comman/comman.module';
import { BlRequestListComponent } from './bl-request-list/bl-request-list.component';
import { NgxSpinnerModule } from "ngx-spinner";
import { NgxPaginationModule } from 'ngx-pagination';
interface NgxSpinnerConfig { type?: string; }

@NgModule({
  declarations: [ 
    BlSidenavComponent,
    BlHomeComponent,
    BlRequestListComponent
  ],
  imports: [
    CommonModule, 
    CommanModule,
    FormsModule,
    NgxSpinnerModule.forRoot({ type: 'ball-scale-multiple' }),
    NgxPaginationModule
  ],  

})

export class ByeLawsModule { }
