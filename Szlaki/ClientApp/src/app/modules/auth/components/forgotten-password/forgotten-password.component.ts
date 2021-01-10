import { ForgottenPassword } from './../../models/forgotten-password';
import { AuthService } from './../../services/AuthService';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { error } from 'protractor';

@Component({
  selector: 'app-forgotten-password',
  templateUrl: './forgotten-password.component.html',
  styleUrls: ['./forgotten-password.component.css']
})
export class ForgottenPasswordComponent implements OnInit {

  forgottenPassword: ForgottenPassword;
  isValid = false;
  constructor(private authService: AuthService,
    private router: Router,private _snackBar: MatSnackBar) {         
      this.forgottenPassword = new ForgottenPassword();
    }

  ngOnInit(): void {

  }

  public sendEmail()
  {
    this.authService.sendEmailToChangePassword(this.forgottenPassword).subscribe(response => {
      this._snackBar.open("Email został wysłany"),
      error => {
        this._snackBar.open("Coś poszło nie tak")
      }
    });
  }

}
