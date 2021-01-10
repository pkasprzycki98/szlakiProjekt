import { User } from './../models/user';
import { Token } from './../models/token';
import { config } from './../../../config';
import { Injectable } from '@angular/core'; 
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import {Observable, of as observableOf } from 'rxjs';
import { ForgottenPassword } from '../models/forgotten-password';
import  { Response} from '../../trails/models/response';

@Injectable()
export class AuthService {
    public isLog = false;
    private tokenUrl = config.apiUrl + "/login";
    private registerUrl = config.apiUrl + "/api/register/registration";

    constructor(private httpClient: HttpClient) { }

    public getToken(username: string, password: string): Observable<Token> {
        return this.httpClient.post(this.tokenUrl, {
            username: username,
            password: password
        }) as Observable<Token>;
    }

    public register(username:string, password: string): Observable<Response<string>> {
        return this.httpClient.post(this.registerUrl, {
            username: username,
            password: password
        }) as Observable<Response<string>>;
    }

    public refreshToken(token: string, refreshToken: string): Observable<Token> {
        return this.httpClient.post(this.tokenUrl + '/refresh', {
            token: token,
            refreshToken: refreshToken
        }) as Observable<Token>;
    }

    public revokeToken(): Observable<any> {
        return this.httpClient.post(this.tokenUrl + '/revoke', {}) as Observable<any>;
    }

    public logOut(): void {
        localStorage.setItem('JWT_TOKEN', '');
        localStorage.setItem('REFRESH_TOKEN', '');
    }

    public isUserLogIn(): boolean {
        const authRecord = localStorage.getItem('JWT_TOKEN');
        return authRecord !== null && authRecord !== '';
    }
    public sendEmailToChangePassword(model: ForgottenPassword) : Observable<any>
    {
        return this.httpClient.post(config.apiUrl + 'register/sendResetPasswordEmail', model);
    }

    public changeLogginStatus() : Observable<boolean>{
        return observableOf(!this.isLog);
    }

    /*public changePassword(currentPassword: string, newPassword: string, confirmNewPassword) {
        return this.httpClient.put(this.userUrl + 'password', {
            currentPassword: currentPassword,
            newPassword: newPassword, confirmNewPassword: confirmNewPassword
        });
    }
    */

}