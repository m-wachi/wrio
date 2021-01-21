import { Component, OnInit } from '@angular/core';
import { PivotData, OptPivotData } from '../pivotdata';
import { Pivot, DtSet, DsColumn } from '../model';
import { PivotService } from '../pivot.service';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-temp03',
  templateUrl: './temp03.component.html',
  styleUrls: ['./temp03.component.css']
})
export class Temp03Component implements OnInit {

  pivot: Pivot = null;
  dataSet: DtSet = null;
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
        this.dataSet = this.pivot.dataSet;
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
    //this.sPivot = sessionStorage.getItem("pivot");
    //this.pivot = JSON.parse(this.sPivot);
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
  column_Dragstart(event){
    event.dataTransfer.effectAllowed = "copy";
    //ドラッグするデータのid名をDataTransferオブジェクトにセット
    event.dataTransfer.setData("text", event.target.id);
  }
  
  //drag-over event handler for column/row header element
  hdr_Dragover(event){
    //cancel dragover event and enable drop event 
    event.preventDefault();
    event.dataTransfer.dropEffect = "copy";
  }
  

  /***** ドロップ時の処理 *****/
  f_drop(event){
    //event.preventDefault();
    event.dataTransfer.dropEffect = "copy";
    console.log("drop start.");
    //ドラッグされたデータのid名をDataTransferオブジェクトから取得
    var id_name = event.dataTransfer.getData("text");
    console.log("id_name=" + id_name);
    //id名からドラッグされた要素を取得
    var drag_elm =document.getElementById(id_name);
    let drag_elem_txt = drag_elm.textContent;
    console.log("drag_elm=" + drag_elm);
    console.log("drag_elem_txt=" + drag_elem_txt);


    //let dropElm = drag_elm.cloneNode(true) as HTMLElement;
    //let dropElm = document.createElement("span");
    //dropElm.id = "rowHdr_" + id_name;
    //dropElm.textContent = drag_elem_txt;

    //ドロップ先にドラッグされた要素を追加
    //event.currentTarget.appendChild(drag_elm); //これだと移動になる模様
    //event.currentTarget.appendChild(dropElm); //これだと移動になる模様
    //エラー回避のため、ドロップ処理の最後にdropイベントをキャンセルしておく
    
    this.pivot.setting.colHdr.push(drag_elem_txt);  // event.preventDefault() するとなにも画面に影響しない
    
  }

  // column header drop event
  colHdr_Drop(event) {
    this.hdrDrop(event, 1);
  }
  // row header drop event
  rowHdr_Drop(event) {
    this.hdrDrop(event, 2);
  }

  // column and row header drop event common routine.
  hdrDrop(event, colrow: number) {
    //event.preventDefault(); DO NOT preventDefault(). component not redrawing. 
    event.dataTransfer.dropEffect = "copy";
    //get id attribute from "DataTransfer" object.
    let elmId = event.dataTransfer.getData("text");
    console.log("drop element id=" + elmId);
    //get element from id
    var elmDrag =document.getElementById(elmId);
    let sColumn = elmDrag.textContent;

    if (colrow === 1) {
      this.pivot.setting.colHdr.push(sColumn);
    } else if (colrow === 2) {
      this.pivot.setting.rowHdr.push(sColumn);
    }

  }

  //fact or dimention table header click handler
  //toggle column name visibility.
  tblHdr_Click(tblName : string) {
    console.log("tblHdr_Click: " + tblName);

    let tblFact = document.getElementById("tbl_" + tblName) as HTMLTableElement;
    let tBodiesLength = tblFact.tBodies.length;
    for (let i=0; i<tBodiesLength; i++) {
      tblFact.tBodies.item(i).hidden = !tblFact.tBodies.item(i).hidden; 
    }
    
  }

  //remove header columns
  colHdrRemove_Click(colIndex: number) {
    console.log("call colHdrRemove_Click(" + colIndex.toString() + ")");
    this.removeColumn(colIndex, 1);
  }

  //remove header columns
  rowHdrRemove_Click(colIndex: number) {
    console.log("call rowHdrRemove_Click(" + colIndex.toString() + ")");
    this.removeColumn(colIndex, 2);
  }

  removeColumn(colIndex: number, colrow: number) {

    if (colrow === 1) {
      this.pivot.setting.colHdr.splice(colIndex ,1);
    } 
    else if (colrow === 2) {
      this.pivot.setting.rowHdr.splice(colIndex, 1);
    }
  }


}
