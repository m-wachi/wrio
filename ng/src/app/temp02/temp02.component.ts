import { Component, OnInit } from '@angular/core';
import { PivotData } from '../pivotdata';
import { PivotService } from '../pivot.service';

@Component({
  selector: 'app-temp02',
  templateUrl: './temp02.component.html',
  styleUrls: ['./temp02.component.css']
})
export class Temp02Component implements OnInit {

  pivotData: PivotData = null;

  constructor(private pivotSvc: PivotService) { }

  colnames = ["col1", "col2", "col3"];

  dataRows = [[1, 2, 3], [4, 5, 6], [7, 8, 9]];

  getPivotData(): void {
    //this.pivotSvc.getPivot01().subscribe(pvtDt => this.pivotData = pvtDt);
    this.pivotData = {
      colNames: ["colA", "colB"],
      rows: [[5, 6], [7, 8]]
    };
    
  }

  ngOnInit(): void {
    this.getPivotData();
  }


}
