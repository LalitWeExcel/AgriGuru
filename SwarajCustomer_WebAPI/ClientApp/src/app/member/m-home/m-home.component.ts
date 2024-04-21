import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToasterService } from 'src/app/services/toaster.service';

@Component({
  selector: 'app-m-home',
  templateUrl: './m-home.component.html',
  styleUrls: ['./m-home.component.scss']
})
export class MHomeComponent {

  constructor(private router: Router, private toaster: ToasterService) {

  }

  goToMemberView(){
      this.router.navigate(['/member-details']);
  }
}
