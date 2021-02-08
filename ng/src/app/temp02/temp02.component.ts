import { Component, OnInit } from '@angular/core';
import { PivotData, OptPivotData } from '../pivotdata';
import { Pivot } from '../model';
import { PivotService } from '../pivot.service';


class PivotTable {
  constructor(aryDctPivotData) { }

  tableData = [];

}





@Component({
  selector: 'app-temp02',
  templateUrl: './temp02.component.html',
  styleUrls: ['./temp02.component.css']
})
export class Temp02Component implements OnInit {

  optPivotData: OptPivotData = null;
  pivotData: PivotData = null;
  pivot: Pivot = null;
  aryDctPivotData: any = null;
  sPivot: string = "";
  sDicPivotData: string = "";
  lstColHdrVal = null;
  lstRowHdrVal = null;
  constructor(private pivotSvc: PivotService) { }

  colnames = ["col1", "col2", "col3"];

  dataRows = [[1, 2, 3], [4, 5, 6], [7, 8, 9]];

  func01(colNames : string[], idx: number, endIdx: number, dic: any, rec: any) : any {
    if (idx > endIdx) {
      return dic;
    } else if (idx === endIdx) {
        dic[colNames[idx]] = rec;
        return dic;
    } else {
      var d3 = null;
      var d2 = null;
      if (colNames[idx] in dic) {
        d2 = this.func01(colNames, idx + 1, endIdx, dic[colNames[idx]], rec);
      } else {
        d3 = {};
        dic[colNames[idx]] = d3;
        d2 = this.func01(colNames, idx + 1, endIdx, dic[colNames[idx]], rec);
      }
    }
  }


  getPivotData(): void {
    this.pivotSvc.getPivot(1).subscribe(
      optPvt => {
        this.pivot = optPvt.value;
        let colHdr = this.pivot.setting.colHdr;
        console.log("colHdr[0]=" + colHdr[0]);
        let rowHdr = this.pivot.setting.rowHdr;
        let sPivot = JSON.stringify(this.pivot);
        this.sPivot = sPivot;
        sessionStorage.setItem("pivot", sPivot);

        this.pivotSvc.getPivotData01().subscribe(
          optPvtDt => {
            this.optPivotData = optPvtDt;
            this.pivotData = optPvtDt.value;
            let pvtDt = this.pivotData;
            let colHdrSet = new Set();
            let rowHdrSet = new Set();   
            let aryDctRec = [];
            //pvtDt.rows.forEach((row) => {
            for (let i=0; i<pvtDt.rows.length; i++) {
              let dctRec = {};
              for(let j=0; j<pvtDt.colNames.length; j++) {
                dctRec[pvtDt.colNames[j]] = pvtDt.rows[i][j];
              }
              console.log("dctRec=" + JSON.stringify(dctRec));
              this.sDicPivotData = JSON.stringify(dctRec);
              colHdrSet.add(dctRec[colHdr[0]]);
              rowHdrSet.add(dctRec[rowHdr[0]]);
              aryDctRec.push(dctRec);
            //});
            }
            this.aryDctPivotData = aryDctRec;
            this.lstColHdrVal = Array.from(colHdrSet);
            console.log("colHdrSet[0])=" + colHdrSet[0]);
            this.lstRowHdrVal = Array.from(rowHdrSet);
            //dctRec[pvtDt.colNames[0]] = pvtDt.rows[0][0];
            //dctRec[pvtDt.colNames[1]] = pvtDt.rows[0][1];
	        });


      });
    

    /*
    this.pivotSvc.getPivot().subscribe({
      next(optPvt) {this.pivot = optPvt.value;},
      error(err) { console.log('Recieved an error: ' + err); }
    });
    */
   
  }

  ngOnInit(): void {
    this.getPivotData();
  }


}
