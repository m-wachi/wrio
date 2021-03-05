import moment, { isMoment } from 'moment';
import {equals, WrioDate, WrioMap, WrioValue, WrioRecord, WrioRecordPair, WrioSet} from './mdl01';
import {getNameIndexPairs, vals2Rec} from './lib01';

const colnames = ["sales_date", "item_cd", "nof_sales"];

const colnmIdxs = [0, 1, 2];

const recs : WrioValue[][] = [
  [new WrioDate("2019-07-01T00:00:00"),	"A0001", 10],
  [new WrioDate("2019-07-01T00:00:00"), "A0002", 15],
  [new WrioDate("2019-07-02T00:00:00"), "A0001", 8],
  [new WrioDate("2019-07-02T00:00:00"), "A0002", 12]
];

/*
const getNameIndexPairs = (aryName: string[], aryColumnName: string[]) : [string, number][]=> {
  return aryName.map((nm) => [nm, aryColumnName.indexOf(nm)])
};

const vals2Rec = (nmIdxPairs: [string, number][], rec: WrioValue[]) => {
  let hdr = new WrioRecord();
  nmIdxPairs.forEach(([nm, nmIdx]) => {
    hdr.set(nm as string, rec[nmIdx as number]);
  });
  return hdr;
} 
*/

const rowHdrNames = ["sales_date"];
const rowHdrNmIdxs = rowHdrNames.map((v) => colnames.indexOf(v));
console.log("rowHdrNmIdxs=" + rowHdrNmIdxs.toString());

//const rowHdrNmIdxPairs : [string, number][] = rowHdrNames.map((nm) => [nm, colnames.indexOf(nm)]);
const rowHdrNmIdxPairs : [string, number][] = getNameIndexPairs(rowHdrNames, colnames);
console.log("rowHdrNmIdxPairs=" + rowHdrNmIdxPairs.toString());

const colHdrNames = ["item_cd"];
//const colHdrNmIdxs = [1];
const colHdrNmIdxs = colHdrNames.map((v) => colnames.indexOf(v));
console.log("colHdrNmIdxs=" + colHdrNmIdxs.toString());

const colHdrNmIdxPairs = getNameIndexPairs(colHdrNames, colnames);
console.log("colHdrNmIdxPairs=" + colHdrNmIdxPairs.toString());


const valNames = ["nof_sales"];
//const valNmIdxs = [2];
const valNmIdxs = valNames.map((v) => colnames.indexOf(v));
console.log("valNmIdxs=" + valNmIdxs.toString());

const valNmIdxPairs = getNameIndexPairs(valNames, colnames);



let pivotdata1 = new WrioMap();
let colHdrSet = new WrioSet();
let rowHdrSet = new WrioSet();

for(let i=0; i<recs.length; i++) {
  for(let j=0; j<colnames.length; j++) {
    
  }
  /*
  let rowHdr = new WrioRecord();
  rowHdrNmIdxPairs.forEach(([nm, nmIdx]) => {
    rowHdr.set(nm as string, recs[i][nmIdx as number]);
  });
  */
  let rowHdr = vals2Rec(rowHdrNmIdxPairs, recs[i]);

  console.log("rowHdr=" + rowHdr.toString());
  rowHdrSet.add(rowHdr);
  /*
  let colHdr = new WrioRecord();
  for(let k=0; k<rowHdrNmIdxs.length; k++) {
    colHdr.set(colHdrNames[k], recs[i][colHdrNmIdxs[k]]);
  } 
  */
  let colHdr = vals2Rec(colHdrNmIdxPairs, recs[i]);
  colHdrSet.add(colHdr);
  
  /*
  let values = new WrioRecord();
  for(let k=0; k<rowHdrNmIdxs.length; k++) {
    values.set(valNames[k], recs[i][valNmIdxs[k]]);
  }
  */
  let values = vals2Rec(valNmIdxPairs, recs[i]);

  let wrp = new WrioRecordPair(rowHdr, colHdr);
  pivotdata1.set(wrp, values);

  console.log(wrp.toString());
  console.log(values.toString());

}
console.log(pivotdata1.toString());

console.log("-- rowHdrSet --");
console.log(rowHdrSet.toString());
console.log("-- colHdrSet --");
console.log(colHdrSet.toString());


const aryRowHdr = rowHdrSet.toArray();
const aryColHdr = colHdrSet.toArray();


console.log(" === pivot data table === ");

let sLine = "____";

for(let colHdr of aryColHdr) {
  const colHdr2 = colHdr as WrioRecord;
  for(const colHdrName of colHdrNames) {
    sLine += ", " + colHdr2.get(colHdrName);
  }
}
console.log(sLine);
sLine = "";

for(const rowHdr of aryRowHdr) {
  const rowHdr2 = rowHdr as WrioRecord;
  for(const rowHdrName of rowHdrNames) {
    sLine += ", " + rowHdr2.get(rowHdrName);
  }
  for(let colHdr of aryColHdr) {

    let vals = pivotdata1.get(new WrioRecordPair(rowHdr as WrioRecord, colHdr as WrioRecord));
    let vals2 = vals as WrioRecord;
    for(const valName of valNames) {
      sLine += ", " + vals2.get(valName);
    }

  }
  console.log(sLine);
  sLine = "";
}
