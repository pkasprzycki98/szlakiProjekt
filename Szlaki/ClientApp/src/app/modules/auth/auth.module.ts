import { MatCardModule } from '@angular/material/card';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { TokenInterceptor } from './token.interceptor';
import { AuthGuard } from './guard/auth.guard';
import { AuthService } from './services/AuthService';
import { LoginComponent } from './components/login/login.component';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import { AuthRoutingModule } from './auth-routing.module';
import {MatGridListModule} from '@angular/material/grid-list';
import { ForgottenPasswordComponent } from './components/forgotten-password/forgotten-password.component';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { RegisterComponent } from '../auth/components/register/register.component';


@NgModule({
  declarations: [LoginComponent, ForgottenPasswordComponent, RegisterComponent],
  providers: [
    AuthGuard,
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  imports: [
    CommonModule,
    RouterModule,
    HttpClientModule,
    AuthRoutingModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    MatCardModule,
    MatGridListModule,
    ReactiveFormsModule,
    FormsModule,
    MatSnackBarModule,
    MatProgressSpinnerModule
  ]
})
export class AuthModule { }