import { Component, HostListener } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss',]
 
})
export class AppComponent {
  title = 'ClientApp';

  @HostListener("window:onbeforeunload", ["$event"])
  clearLocalStorage() {
    localStorage.clear();

  }
   ngOnDestroy() {
    localStorage.removeItem('access_token');
  }
}
