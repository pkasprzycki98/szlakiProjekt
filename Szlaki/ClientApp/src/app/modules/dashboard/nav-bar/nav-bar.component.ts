import { AuthService } from './../../auth/services/AuthService';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import {MatIconModule} from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';


@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  text: string;
  isLog: boolean;
  constructor(private router: Router, private authService: AuthService) { 
    this.isLog = false;
  }

  ngOnInit(): void {
    if(localStorage.getItem('JWT_TOKEN').length > 1){
      this.text = "login";
      this.authService.changeLogginStatus().subscribe(response => {
        this.isLog = response;
      });
    }
    else{
      this.text = "login";
      this.authService.changeLogginStatus().subscribe(response => {
        this.isLog = response;
      });
    }
  }

  action(){
    if(localStorage.getItem('JWT_TOKEN').length > 1){
    localStorage.setItem('JWT_TOKEN','');
    localStorage.setItem('REFRESH_TOKEN','');
    this.router.navigate(['']);
    }
    else{
      this.router.navigate(['']);
    }
  }

}
