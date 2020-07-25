import { Component, OnInit } from '@angular/core';
import { PivotData, OptPivotData } from '../pivotdata';
import { PivotService } from '../pivot.service';

@Component({
  selector: 'app-temp02',
  templateUrl: './temp02.component.html',
  styleUrls: ['./temp02.component.css']
})
export class Temp02Component implements OnInit {

  optPivotData: OptPivotData = null;
  pivotData: PivotData = null;
  
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
	this.pivotData = optPvtDt.value
      });
    
  }

  ngOnInit(): void {
    this.getPivotData();
  }


}
