import { Component } from '@angular/core';
import { OwlOptions } from 'ngx-owl-carousel-o'; 
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  customOptions: OwlOptions = {
    loop: true,
    autoplay:true,
    animateIn:true,
    autoHeight: true,
    animateOut:true,
    nav: false,
    margin: 20,
    mouseDrag: true,
    touchDrag: true,
    pullDrag: true,
    dots: true,
    navSpeed: 200,
    navText: ['', '']
  }
}
