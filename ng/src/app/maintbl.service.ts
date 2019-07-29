import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MainTbl } from './maintbl';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class MaintblService {

  constructor(  //){}
    private http: HttpClient,
    private messageSvc: MessageService) { }

  getAbc(): Observable<MainTbl> {
    let tbl = new MainTbl();
    tbl.id = 34;
    tbl.name = 'Yes, I do.';
    //return of(tbl);
    this.log('hello log');
    return this.http.get<MainTbl>('/wrio/api/mdl01/path3');
  }

  /** MaintblServiceのメッセージをMessageServiceを使って記録 */
  private log(message: string) {
    this.messageSvc.add(`MaintblService: ${message}`);
  }
}
