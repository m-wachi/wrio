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
    alert('button1 click');
  }


}
