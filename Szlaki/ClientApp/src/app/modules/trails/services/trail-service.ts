import { Trail } from './../models/trail';
import { config } from './../../../config';
import { Injectable } from '@angular/core'; 
import { HttpClient } from '@angular/common/http';
import {FileModel} from '../models/file';
import {Response} from '../models/response';
import {Observable, of as observableOf } from 'rxjs';
import { DateQuery } from '../models/between-date';

@Injectable()
export class TrailSerivce {
    public isLog = false;
    private trailUrl = config.apiUrl + "/api/Trail";

    constructor(private httpClient: HttpClient) { }

    public getTrails(): Observable<Trail[]> {
        return this.httpClient.get(this.trailUrl + '/getAll', {
        }) as Observable<Trail[]>;
    }

    public getTrail(id: String): Observable<Trail> {
        return this.httpClient.get(this.trailUrl + '/trailId/' + id, {
        }) as Observable<Trail>;
    }

    public postTrail(trail: Trail): Observable<Response<Trail>> {
        return this.httpClient.post<Response<Trail>>(this.trailUrl,trail);
    }

    public putTrail(trail: Trail): Observable<Response<Trail>> {
        return this.httpClient.put<Response<Trail>>(this.trailUrl,trail);
    }

    public deleteTrail(trailId: String) {
        return this.httpClient.delete(this.trailUrl + '/delete/' + trailId);
    }

    public getTrailsBetweenDate(dateQuery: DateQuery): Observable<Trail[]> {
        return this.httpClient.post(this.trailUrl + '/getBetweenDate',dateQuery) as Observable<Trail[]>;
    }

    public postPhoto(file: any): Observable<Boolean> {
        return this.httpClient.post<Boolean>(this.trailUrl + '/photo',file);
    }

    public postVideo(file: any): Observable<Boolean> {
        return this.httpClient.post<Boolean>(this.trailUrl + '/video',file);
    }

}