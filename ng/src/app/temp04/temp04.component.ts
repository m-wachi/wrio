import { Component, OnInit } from '@angular/core';
import { PivotData, OptPivotData } from '../pivotdata';
import { Pivot, DtSet, DsColumn, CellVal } from '../model';
import { PivotService } from '../pivot.service';
import { MessageService } from '../message.service';
import { WrioValue } from '../model02';
import { PivotTableCell } from '../pivottable';
import * as wlib01 from '../wriolib01';
import * as wlib02 from '../wriolib02';

@Component({
  selector: 'app-temp04',
  templateUrl: './temp04.component.html',
  styleUrls: ['./temp04.component.css']
})
export class Temp04Component implements OnInit {
  pivot: Pivot = null;
  dataSet: DtSet = null;
  sPivot: string = "";
  optPivotData: OptPivotData = null;
  pivotData: PivotData = null;
  pivotTable: PivotTableCell[][];
  factColumns: Array<DsColumn> = [];
  aryColumn: Array<DsColumn> = [];
  sRowHdrSet: string = "";
  cells: WrioValue[][] = [];

  removable = true;

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
    this.getPivot(1);
  }

  // put button
  button1_Click(): void {
    //this.sPivot = sessionStorage.getItem("pivot");
    //this.pivot = JSON.parse(this.sPivot);
    this.pivotSvc.putPivotData01(this.pivot).subscribe(
      optPvtDt => {
        this.optPivotData = optPvtDt;
        this.pivotData = optPvtDt.value;


        let recs = this.pivotData.rows;
        let colnames = this.pivotData.colNames;
        console.log("colnames=" + colnames.toString());

        //
        // このあたりにthis.pivotData.rowsをPivotTableCellに変換するコードを入れる
        // WrioValue[][] -> PivotTableCell[][]
        //
        /*
        const rowHdrNames = this.pivot.setting.rowHdr;
        const rowHdrNmIdxPairs : [string, number][] = wlib01.getNameIndexPairs(rowHdrNames, colnames);
        console.log("rowHdrNmIdxPairs=" + rowHdrNmIdxPairs.toString());

        //const colHdrNames = ["item_cd"];
        const colHdrNames = this.pivot.setting.colHdr;
        const colHdrNmIdxPairs = wlib01.getNameIndexPairs(colHdrNames, colnames);
        console.log("colHdrNmIdxPairs=" + colHdrNmIdxPairs.toString());
        
        //const valNames = ["nof_sales"];
        const cv1 = this.pivot.setting.cellVal[0];
        //const valNames = [this.pivot.setting.cellVal[0].colName];
        const valNames = [cv1.abbrev + "." + cv1.colName];
        const valNmIdxPairs = wlib01.getNameIndexPairs(valNames, colnames);
        console.log("valNmIdxPairs=" + valNmIdxPairs.toString());
        
        let [rowHdrSet, colHdrSet, pivotdata1] = wlib01.conv2Map(recs, rowHdrNmIdxPairs, colHdrNmIdxPairs, valNmIdxPairs);
        console.log("rowHdrSet=" + rowHdrSet.toString());
        console.log("pivotdata1=" + pivotdata1.toString());

        this.sRowHdrSet = rowHdrSet.toString();

        const aryRowHdr = rowHdrSet.toArray();
        const aryColHdr = colHdrSet.toArray();

        this.cells = wlib01.conv2Array2D(aryRowHdr, aryColHdr, rowHdrNames, colHdrNames, valNames, pivotdata1);
        //this.cells = [["cell11", "cell12"], ["cell21", "cell22"]];
        */
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

  btnPut2_Click(): void {
    this.pivotSvc.putPivotData01(this.pivot).subscribe(
      optPvtDt => {
        this.optPivotData = optPvtDt;
        this.pivotData = optPvtDt.value;

        let recs = this.pivotData.rows;
        let colnames = this.pivotData.colNames;
        //this.pivotData.colNames

        let dsColumns = this.pivotData.colNames;

        let ary2dPtCell = wlib02.convAry2dWv2AryPtcs(dsColumns, recs);

        const fieldNames2 = dsColumns.map((x)=>{return x.colName;});
        console.log("fieldNames2=" + fieldNames2.toString());
        
        //const rowNmIdxPairs = wlib01.getNameIndexPairs(["sales_date"], fieldNames2);
        const aryRowName = this.pivot.setting.rowHdr.map(x => { return x.split(".")[1]});
        const rowNmIdxPairs = wlib01.getNameIndexPairs(aryRowName, fieldNames2);
        //const colNmIdxPairs = wlib01.getNameIndexPairs(["item_name"], fieldNames2);
        const aryColName = this.pivot.setting.colHdr.map(x => { return x.split(".")[1]});
        const colNmIdxPairs = wlib01.getNameIndexPairs(aryColName, fieldNames2);
        //const valNmIdxPairs = wlib01.getNameIndexPairs(["sales_amount"], fieldNames2);
        const aryValName = this.pivot.setting.cellVal.map(x => {return x.colName});
        const valNmIdxPairs = wlib01.getNameIndexPairs(aryValName, fieldNames2);
        
        console.log("rowNmIdxPairs: " + rowNmIdxPairs.toString());
        
        const [rowHdrSet1_2, colHdrSet1_2, dicVal_2] = wlib02.conv2Map2(ary2dPtCell, rowNmIdxPairs, colNmIdxPairs, valNmIdxPairs);
        console.log("rowHdrSet1_2: " + rowHdrSet1_2.toString());
        console.log("colHdrSet1_2: " + colHdrSet1_2.toString());
        console.log("dicVal: " + dicVal_2.toString());

        const tbl = wlib02.conv2Array2D2(rowHdrSet1_2.toArray(), colHdrSet1_2.toArray(), 
        aryRowName, aryColName, aryValName, dicVal_2);

        for(const r of tbl) {
          console.log("r[0]=" + r[0].toString());
          let ary = r.map((x)=>{return x?.text();});
          console.log("ary=" + ary.toString());
        }
        this.pivotTable = tbl;
      });
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
    this.hdrValDrop(event, 1);
  }
  // row header drop event
  rowHdr_Drop(event) {
    this.hdrValDrop(event, 2);
  }

  // cell value drop event
  cellVal_Drop(event) {
    this.hdrValDrop(event, 3);
  }


  // column and row header drop event common routine.
  hdrValDrop(event, colrow: number) {
    //event.preventDefault(); DO NOT preventDefault(). component not redrawing. 
    event.dataTransfer.dropEffect = "copy";
    //get id attribute from "DataTransfer" object.
    let elmId = event.dataTransfer.getData("text");
    console.log("drop element id=" + elmId);
    //get element from id
    var elmDrag =document.getElementById(elmId);
    console.log("drop id=[" + elmDrag.id + "]");
    //let sColumn = elmDrag.textContent;

    if (colrow === 1) {
      this.pivot.setting.colHdr.push(elmId);
    } else if (colrow === 2) {
      this.pivot.setting.rowHdr.push(elmId);
    } else if (colrow === 3) {
      let aryElmId = (elmId as string).split('.');
      let cellVal0: CellVal = {colName: aryElmId[1], abbrev: aryElmId[0], aggFuncDiv: 0};
      this.pivot.setting.cellVal.push(cellVal0);
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

  tblHdr_Click2(chip_list_name : string) {
    console.log("tblHdr_Click2: " + chip_list_name);
    let chipListElm = document.getElementById(chip_list_name);
    chipListElm.hidden = !chipListElm.hidden;

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

  //remove cell value columns
  cellValRemove_Click(colIndex: number) {
    console.log("call cellValRemove_Click(" + colIndex.toString() + ")");
    this.removeColumn(colIndex, 3);
  }

  removeColumn(colIndex: number, colrow: number) {

    if (colrow === 1) {
      this.pivot.setting.colHdr.splice(colIndex ,1);
    } 
    else if (colrow === 2) {
      this.pivot.setting.rowHdr.splice(colIndex, 1);
    }
    else if (colrow === 3) {
      this.pivot.setting.cellVal.splice(colIndex, 1);
    }
  }

  chipRemove(col) : void {
    console.log("chipremove");
  }

}
