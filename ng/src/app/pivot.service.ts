import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PivotData } from './pivotdata';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class PivotService {
  
  constructor(
    private http: HttpClient,
    private messageSvc: MessageService) { }

  getPivot01(): Observable<PivotData> {
    let pvtData: PivotData = {
      colNames: ["col1", "col2"],
      rows: [[1, 2], [3, 4]]
    };
    
    this.log('getPivot01 log');
    return of(pvtData);
  }

  /** MaintblServiceのメッセージをMessageServiceを使って記録 */
  private log(message: string) {
    this.messageSvc.add(`MaintblService: ${message}`);
  }
}
