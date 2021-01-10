import { AuthService } from './../../services/AuthService';
import { User } from './../../models/user';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
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


  public onFormSubmit({ value, valid }: { value: User, valid: boolean }) {
    this.isLogging = true;

    this.authService.register(value.username, value.password)
      .subscribe(data => {
        this.router.navigate(['/']);
      });
  }
}