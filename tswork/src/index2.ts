import moment, { isMoment } from 'moment';
import {WrioDate, WrioMap, WrioValue, WrioRecord, WrioRecordPair} from './mdl01';

const colnames = ["sales_date", "item_cd", "nof_sales"];

const colnmIdxs = [0, 1, 2];

const recs : WrioValue[][] = [
  [new WrioDate("2019-07-01T00:00:00"),	"A0001", 10],
  [new WrioDate("2019-07-01T00:00:00"), "A0002", 15],
  [new WrioDate("2019-07-02T00:00:00"), "A0001", 8],
  [new WrioDate("2019-07-02T00:00:00"), "A0002", 12]
];

const rowHdrNames = ["sales_date"];

const rowHdrNmIdxs = [0];

const colHdrNames = ["item_cd"];

const colHdrNmIdxs = [1];

const valNames = ["nof_sales"];

const valNmIdxs = [2];


console.log(colHdrNames);
let pivotdata1 = new WrioMap();

for(let i=0; i<recs.length; i++) {
  for(let j=0; j<colnames.length; j++) {
    
  }
  let rowHdr = new WrioRecord();
  for(let k=0; k<rowHdrNmIdxs.length; k++) {
    rowHdr.set(rowHdrNames[k], recs[i][rowHdrNmIdxs[k]]);
  }
  let colHdr = new WrioRecord();
  for(let k=0; k<rowHdrNmIdxs.length; k++) {
    colHdr.set(colHdrNames[k], recs[i][colHdrNmIdxs[k]]);
  }
  let values = new WrioRecord();
  for(let k=0; k<rowHdrNmIdxs.length; k++) {
    values.set(valNames[k], recs[i][valNmIdxs[k]]);
  }
  let wrp = new WrioRecordPair(rowHdr, colHdr);
  pivotdata1.set(wrp, values);

  console.log(wrp.toString());
  console.log(values.toString());

}
console.log(pivotdata1.toString());
