import { Component, OnInit } from '@angular/core';
import { PivotData, OptPivotData } from '../pivotdata';
import { Pivot } from '../model';
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
    //alert('button1 click');
    this.pivotSvc.putPivotData01(this.pivot).subscribe(
      optPvtDt => {
        this.optPivotData = optPvtDt;
        this.pivotData = optPvtDt.value;
      }
    );
  }


}
