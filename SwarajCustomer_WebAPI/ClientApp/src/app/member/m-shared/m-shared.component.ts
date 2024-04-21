import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-m-shared',
  templateUrl: './m-shared.component.html',
  styleUrls: ['./m-shared.component.scss']
})
export class MSharedComponent {
@Input() Title:string='';
@Input() Value:string='';
@Input() Unit:string='';
}
