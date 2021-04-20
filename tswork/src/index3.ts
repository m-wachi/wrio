import * as utl1 from './util01';
import { DsColumn } from './model';
//import * as mmt from 'moment';
import { WrioDate, WrioMap, WrioValue, WrioRecord, WrioValueType } from './model02';
import * as wl from './wriolib01';
import * as wl2 from './wriolib02';
import {getPivotTableFieldDef, PivotTableCell, PivotTableCells} from './pivottable';

console.log("Hello index.ts");

const v1 = 1234.567;
const v2 = 3456.7;

console.log(new Intl.NumberFormat('ja').format(v1));

// 四捨五入
// 小数点以下をどう扱いたいかもオプションで指定可能です。
// minimumFractionDigitsで最小桁数（足りない場合は 0 で埋める）、
// maximumFractionDigitsで最大桁数（超える場合はこの桁数に収まるよう四捨五入）
// を指定できます。
let nf1 = new Intl.NumberFormat('ja', {style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2});

console.log("v1=" + nf1.format(v1));
console.log("v2=" + nf1.format(v2));


//const fieldNames = ["sales_date", "item_cd", "nof_sales"];
const dsColumns : DsColumn[] = [
  {colName: "sales_date", colType: WrioValueType.DATE},
  {colName: "item_cd", colType: WrioValueType.STRING},
  {colName: "nof_sales", colType: WrioValueType.NUMBER}
];
const fieldNames2 = dsColumns.map((x)=>{return x.colName;});
console.log("fieldNames2");
console.log(fieldNames2);
//const colnmIdxs = [0, 1, 2];

const recs : WrioValue[][] = [
  [new WrioDate("2019-07-01"),	"A0001", 10],
  [new WrioDate("2019-07-01"), "A0002", 15],
  [new WrioDate("2019-07-02"), "A0001", 8],
  [new WrioDate("2019-07-02"), "A0002", 12]
];

let fd1 = getPivotTableFieldDef(dsColumns[0].colName, dsColumns[0].colType);
let ptc1 = new PivotTableCell(recs[0][0], fd1);


let a = utl1.myZip(dsColumns, recs[0]);

let b = a.map(wl2.getPtcFromPair);
let c = b.map((x) => {return x.text();});
console.log(c);



let ary2dPtCell = wl2.convAry2dWv2AryPtcs(dsColumns, recs);
console.log(ary2dPtCell.map((x) => {return x.getPtcArray().map((y) => {return y.text();})}));

const rowNmIdxPairs = wl.getNameIndexPairs(["sales_date"], fieldNames2);
const colNmIdxPairs = wl.getNameIndexPairs(["item_cd"], fieldNames2);
const valNmIdxPairs = wl.getNameIndexPairs(["nof_sales"], fieldNames2);

console.log("rowNmIdxPairs: " + rowNmIdxPairs.toString());


//const [rowHdrSet1, colHdrSet1, dicVal] = wl.conv2Map(recs, rowNmIdxPairs, colNmIdxPairs, valNmIdxPairs);
//console.log("rowHdrSet1: " + rowHdrSet1.toString());
const [rowHdrSet1_2, colHdrSet1_2, dicVal_2] = wl2.conv2Map2(ary2dPtCell, rowNmIdxPairs, colNmIdxPairs, valNmIdxPairs);
console.log("rowHdrSet1_2: " + rowHdrSet1_2.toString());
console.log("colHdrSet1_2: " + colHdrSet1_2.toString());
console.log("dicVal: " + dicVal_2.toString());

//const tbl = wl.conv2Array2D(rowHdrSet1.toArray(), colHdrSet1.toArray(), 
//                          ["sales_date"], ["item_cd"], ["nof_sales"], dicVal);

const tbl = wl2.conv2Array2D2(rowHdrSet1_2.toArray(), colHdrSet1_2.toArray(), 
                          ["sales_date"], ["item_cd"], ["nof_sales"], dicVal_2);

for(const r of tbl) {
  let ary = r.map((x)=>{return x?.text();});
  console.log(ary);
}
