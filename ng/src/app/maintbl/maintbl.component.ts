import { Component, OnInit } from '@angular/core';
import { MaintblService } from '../maintbl.service';


@Component({
  selector: 'app-maintbl',
  templateUrl: './maintbl.component.html',
  styleUrls: ['./maintbl.component.css']
})
export class MaintblComponent implements OnInit {

  aaa = 'Hey Hey Hey! Yeah!';
  bbb = 'bbb';
  
  constructor(private maintblSvc: MaintblService) { }
  //constructor() { }

  getAbc(): void {
    var ab: string = 'xxx';
    ab = this.maintblSvc.getAbc();
    this.bbb = ab;
  }

    
  ngOnInit() {
    this.getAbc();
  }

}
