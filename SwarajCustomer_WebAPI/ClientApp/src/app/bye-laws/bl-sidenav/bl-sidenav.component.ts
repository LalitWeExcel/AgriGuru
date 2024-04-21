import { Component } from '@angular/core'; 
import { Router } from '@angular/router';
import { ToasterService } from 'src/app/services/toaster.service';

@Component({
  selector: 'app-bl-sidenav',
  templateUrl: './bl-sidenav.component.html',
  styleUrls: ['./bl-sidenav.component.scss']
})
export class BlSidenavComponent {

  constructor(private router: Router,private toaster:ToasterService) {
  }

  routToRequest()
  {
    console.log("request");
    this.router.navigate(['/bye-laws']);
  }

  routToRequestList()
  {
    
    console.log("request list");
    this.router.navigate(['/bye-laws-request-list']);
  }
}
