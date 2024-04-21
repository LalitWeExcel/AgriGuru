import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AmendmntRequestComponent } from './amendmnt-request/amendmnt-request.component';
import { CommanModule } from '../comman/comman.module';



@NgModule({
  declarations: [
    AmendmntRequestComponent
  ],
  imports: [
    CommonModule,
    CommanModule
  ]
})
export class RcsModule { }
