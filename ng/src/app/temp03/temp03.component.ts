import { Component, OnInit } from '@angular/core';
import { PivotData, OptPivotData } from '../pivotdata';
import { Pivot, DsColumn } from '../model';
import { PivotService } from '../pivot.service';
import { MessageService } from '../message.service';

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

  constructor(private pivotSvc: PivotService, 
              private messageSvc: MessageService) { }

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


  /***** ドラッグ開始時の処理 *****/
  f_dragstart(event){
    //this.messageSvc.add("drag start.");
    console.log("drag start.");
    //ドラッグするデータのid名をDataTransferオブジェクトにセット
    event.dataTransfer.setData("text", event.target.id);
  }
  
  /***** ドラッグ要素がドロップ要素に重なっている間の処理 *****/
  f_dragover(event){
    //dragoverイベントをキャンセルして、ドロップ先の要素がドロップを受け付けるようにする
    //console.log("dragover start.");
    event.preventDefault();
  }
  

  /***** ドロップ時の処理 *****/
  f_drop(event){
    console.log("drop start.");
    //ドラッグされたデータのid名をDataTransferオブジェクトから取得
    var id_name = event.dataTransfer.getData("text");
    console.log("id_name=" + id_name);
    //id名からドラッグされた要素を取得
    var drag_elm =document.getElementById(id_name);

    let drag_elem_txt = drag_elm.textContent;
    console.log("drag_elm=" + drag_elm);
    console.log("drag_elem_txt=" + drag_elem_txt);
    //ドロップ先にドラッグされた要素を追加
    event.currentTarget.appendChild(drag_elem_txt);
    //エラー回避のため、ドロップ処理の最後にdropイベントをキャンセルしておく
    event.preventDefault();
  }

}
