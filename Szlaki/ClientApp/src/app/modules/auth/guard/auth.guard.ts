import { AuthService } from './../services/AuthService';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';


@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private loginService: AuthService,
    private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> | boolean {
    const isLogged = this.loginService.isUserLogIn();

    if (isLogged === false) 
      this.router.navigate(['']);
    
    if (isLogged) {
        return isLogged;      
    }
  }
}

