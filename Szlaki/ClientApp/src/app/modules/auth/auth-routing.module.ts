import { ForgottenPasswordComponent } from './components/forgotten-password/forgotten-password.component';
import { LoginComponent } from './components/login/login.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [ 
  {
    path: '',
    component: LoginComponent,  
  },
  {
    path: 'forgotten-password',
    component: ForgottenPasswordComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}