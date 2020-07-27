import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PivotData, OptPivotData } from './pivotdata';
import { OptPivot } from './model';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class PivotService {
  
  constructor(
    private http: HttpClient,
    private messageSvc: MessageService) { }

  getPivot(): Observable<any> {
      this.log('getPivot log');
      //return of(pvtData);
      return this.http.get<OptPivot>('/wrio/api/sys01/pvt/1');
  }
  

  //getPivotData01(): Observable<PivotData> {
  getPivotData01(): Observable<any> {
    let pvtData: PivotData = {
      colNames: ["col1", "col2"],
      rows: [[1, 2], [3, 4]]
    };
    
    this.log('getPivotData01 log');
    //return of(pvtData);
    return this.http.get<OptPivotData>('/wrio/api/sys01/pvtdt1');
  }

  /** MaintblServiceのメッセージをMessageServiceを使って記録 */
  private log(message: string) {
    this.messageSvc.add(`PivotService: ${message}`);
  }
}
