import { AuthService } from './../../services/AuthService';
import { User } from './../../models/user';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  user: User;
  successfulLogin = true;
  isLogging = true;

  constructor(private authService: AuthService,
    private router: Router) {

    this.user = new User();
  }
  OnInit() {
    this.isLogging = false;
  }

  public onForgottenPasswordClick() {
    this.router.navigate(['/register']);
  }

  public onFormSubmit({ value, valid }: { value: User, valid: boolean }) {
    this.isLogging = true;

    this.authService.getToken(value.username, value.password)
      .subscribe(data => {
        this.isLogging = false;
  
        if (data == null || data.token == null) {
          this.successfulLogin = false;
          return;
        }

        localStorage.setItem('JWT_TOKEN', data.token);
        localStorage.setItem('REFRESH_TOKEN', data.refreshToken);

        this.router.navigate(['/dashboard']);

      }, error => {
        this.successfulLogin = false;
        this.isLogging = false;
      });
  }
}