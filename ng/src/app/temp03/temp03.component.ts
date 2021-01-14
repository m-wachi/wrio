import { Component, OnInit } from '@angular/core';
import { PivotData, OptPivotData } from '../pivotdata';
import { Pivot, DsColumn } from '../model';
import { PivotService } from '../pivot.service';

@Component({
  selector: 'app-temp03',
  templateUrl: './temp03.component.html',
  styleUrls: ['./temp03.component.css']
})
export class Temp03Component implements OnInit {

  pivot: Pivot = null;
  sPivot: string = "";
  optPivotData: OptPivotData = null;
  pivotData: PivotData = null;
  factColumns: Array<DsColumn> = [];
  aryColumn: Array<DsColumn> = [];

  constructor(private pivotSvc: PivotService) { }

  getPivot(pivotId : number): void {
    this.pivotSvc.getPivot(pivotId).subscribe(
      optPvt => {
        this.pivot = optPvt.value;
        let colHdr = this.pivot.setting.colHdr;
        let sPivot = JSON.stringify(this.pivot);
        this.sPivot = sPivot;
        sessionStorage.setItem("pivot", sPivot);
      });
  }

  ngOnInit(): void {
    this.getPivot(2);
  }

  button1_Click(): void {
    this.sPivot = sessionStorage.getItem("pivot");
    this.pivot = JSON.parse(this.sPivot);
    this.pivotSvc.putPivotData01(this.pivot).subscribe(
      optPvtDt => {
        this.optPivotData = optPvtDt;
        this.pivotData = optPvtDt.value;
      }
    );
  }

  button2_Click(): void {
    alert('button2 click');
    this.pivotSvc.getDatasetColumn01(2).subscribe(
      aryColumn => {
        this.aryColumn = aryColumn;
      }
    );
  }

  button3_Click(): void {
    alert('button3 click');
    this.pivotSvc.getPivot(2).subscribe(
      optPvt => {
        this.pivot = optPvt.value;
        let colHdr = this.pivot.setting.colHdr;
        let sPivot = JSON.stringify(this.pivot);
        this.sPivot = sPivot;
        this.factColumns = this.pivot.dataSet.fact.columns;
        sessionStorage.setItem("pivot", sPivot);
      }
    );
  }

}
