import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MainTbl } from './maintbl';

@Injectable({
  providedIn: 'root'
})
export class MaintblService {

  constructor(  //){}
    private http: HttpClient) { }

  getAbc(): Observable<MainTbl> {
    let tbl = new MainTbl();
    tbl.id = 34;
    tbl.name = 'Yes, I do.';
    //return of(tbl);
    return this.http.get<MainTbl>('/wrio/api/mdl01/path3');
  }
}
