import { Component, OnInit } from '@angular/core';
import { MainTbl } from '../maintbl';
import { MaintblService } from '../maintbl.service';

@Component({
  selector: 'app-maintbl',
  templateUrl: './maintbl.component.html',
  styleUrls: ['./maintbl.component.css']
})
export class MaintblComponent implements OnInit {

  aaa = 'Hey Hey Hey! Yeah!';
  mainTbl : MainTbl = null;
  
  constructor(private maintblSvc: MaintblService) { }

  getAbc(): void {
    //this.mainTbl = this.maintblSvc.getAbc();
    this.maintblSvc.getAbc()
      .subscribe(mainTbl => this.mainTbl = mainTbl);
    //this.mainTbl = new MainTbl();
    //this.mainTbl.id = 12;
    //this.mainTbl.name = "Good Morning!";
  }

    
  ngOnInit() {
    this.getAbc();
  }

}
