import { Component, OnInit } from '@angular/core';
import { PivotData, OptPivotData } from '../pivotdata';
import { Pivot } from '../model';
import { PivotService } from '../pivot.service';

@Component({
  selector: 'app-temp02',
  templateUrl: './temp02.component.html',
  styleUrls: ['./temp02.component.css']
})
export class Temp02Component implements OnInit {

  optPivotData: OptPivotData = null;
  pivotData: PivotData = null;
  pivot: Pivot = null;
  dicPivotData: any = null;
  sPivot: string = "";
  sDicPivotData: string = "";
  
  constructor(private pivotSvc: PivotService) { }

  colnames = ["col1", "col2", "col3"];

  dataRows = [[1, 2, 3], [4, 5, 6], [7, 8, 9]];

  getPivotData(): void {
    // this.pivotData = {
    //   colNames: ["colA", "colB"],
    //   rows: [[5, 6], [7, 8]]
    // };
    this.pivotSvc.getPivotData01().subscribe(
      optPvtDt => {
	this.optPivotData = optPvtDt;
	this.pivotData = optPvtDt.value;
	
	this.pivotSvc.getPivot().subscribe(
	  optPvt => {
	    this.pivot = optPvt.value;
	    let colHdr = this.pivot.setting.colHdr;
	    this.sPivot = JSON.stringify(this.pivot);
	    var a = {dummykey: "dummydata"};
	    console.log("colHdr[0]=" + colHdr[0]);
	    a[colHdr[0]] = "bbbb";
	    this.sDicPivotData = JSON.stringify(a);
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
