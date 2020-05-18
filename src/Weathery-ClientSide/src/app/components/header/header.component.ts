import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less']
})
export class HeaderComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  loginNavigation(){
    this.router.navigate(["login"]);
  }

  registerNavigation(){
    this.router.navigate(["register"]);
  }

  homeNavigation(){
    this.router.navigate(['/']);
  }
}
