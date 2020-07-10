import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-temp01',
  templateUrl: './temp01.component.html',
  styleUrls: ['./temp01.component.css']
})
export class Temp01Component implements OnInit {

  constructor() { }

  colnames = ["col1", "col2", "col3"];

  dataRows = [[1, 2, 3], [4, 5, 6], [7, 8, 9]];

  ngOnInit(): void {
  }

}
