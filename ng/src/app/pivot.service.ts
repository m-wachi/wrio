import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { PivotData, OptPivotData } from './pivotdata';
import { Pivot, OptPivot } from './model';
import { MessageService } from './message.service';
import { stringify } from '@angular/compiler/src/util';

@Injectable({
  providedIn: 'root'
})
export class PivotService {
  
  constructor(
    private http: HttpClient,
    private messageSvc: MessageService) { }

  /**
   * 失敗したHttp操作を処理します。
   * アプリを持続させます。
   * @param operation - 失敗した操作の名前
   * @param result - observableな結果として返す任意の値
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: リモート上のロギング基盤にエラーを送信する
      console.error(error); // かわりにconsoleに出力

      // TODO: ユーザーへの開示のためにエラーの変換処理を改善する
      this.log(`${operation} failed: ${error.message}`);

      // 空の結果を返して、アプリを持続可能にする
      return of(result as T);
    };
  }
  getPivot(pivotId: number): Observable<any> {
    this.log('getPivot log');
    //return of(pvtData);
    //return this.http.get<OptPivot>('/wrio/api/sys01/pvt/1');
    return this.http.get<OptPivot>('/wrio/api/sys01/pvt/' + pivotId.toString());
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

  putPivotData01(pvt: Pivot): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };

    this.log('putPivotData01 log');
    this.log('pvt.pivotId = ' + pvt.pivotId.toString());
    
    let ret = this.http.put<OptPivotData>('/wrio/api/sys01/pvt/2', pvt, httpOptions)
      .pipe(
        catchError(this.handleError('putPivotData', pvt))
      );
    
    this.log('putPivotData01 end.');
    return ret;
  }

  getDatasetColumn01(datasetId: number): Observable<any> {

    this.log('getDatasetColumn01 log');
    this.log('datasetId = ' + datasetId.toString());
    
    let ret = this.http.get<Array<string>>('/wrio/api/sys01/dataset/2')
      .pipe(
        catchError(this.handleError('getDatasetColumn01', datasetId))
      );
    
    this.log('getDatasetColumn01 end.');
    return ret;
  }

  /** MaintblServiceのメッセージをMessageServiceを使って記録 */
  private log(message: string) {
    this.messageSvc.add(`PivotService: ${message}`);
  }
}
