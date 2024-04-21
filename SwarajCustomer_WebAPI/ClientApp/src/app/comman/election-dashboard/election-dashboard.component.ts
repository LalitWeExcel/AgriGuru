import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ElectionServiceService } from 'src/app/services/election/election-service.service';
import { MemberdetailsService } from 'src/app/services/member-services/memberdetails.service';
import { ToasterService } from 'src/app/services/toaster.service';
import { SocietyPreviousCommetteMember } from 'src/app/shared/models/election/election_model';
import { SocietyDetailsModels } from 'src/app/shared/models/society-trans-model.model';

@Component({
  selector: 'app-election-dashboard',
  templateUrl: './election-dashboard.component.html',
  styleUrls: ['./election-dashboard.component.scss']
})
export class ElectionDashboardComponent {

  SocietyDetailsModels!: SocietyDetailsModels;
  societyPreviousCommetteMemberList: SocietyPreviousCommetteMember[] = [];
  id: number;

  constructor(private toast: ToasterService,
    private service: ElectionServiceService, private router: Router,
    private SpinnerService: NgxSpinnerService,
    private _memberService: MemberdetailsService,
  ) {

  }

  ngOnInit() {
    this.SpinnerService.show();


    this.id = Number(localStorage.getItem("userID")!);
    this.GetSocietyDetails(this.id, 0);

  }



  GetSocietyDetails(userId: number, societyId: number): void {
    let params: any = {};
    params[`userId`] = userId;
    params[`societyId`] = societyId;

    this._memberService.GetSocietyDetail(params).subscribe({

      next: (response: any) => {
        this.SocietyDetailsModels = response.data;
        console.log(this.SocietyDetailsModels);

        this.GetSocietyMembers(this.SocietyDetailsModels.societyTransID);
      },
      error(err) {

      },
    });
  }

  GetSocietyMembers(stid: any) {
    this.service.GetSocietyMembers({ socityTranId: stid }).subscribe({
      next: (response: any) => {
        this.societyPreviousCommetteMemberList = response.data.societyPreviousCommetteMember;
        console.log(this.societyPreviousCommetteMemberList);
        //this.memberList = response.data.members;  
      },
      error: (err: any) => {
        console.log(err);
        this.SpinnerService.hide();
      }
    })
  }

}
