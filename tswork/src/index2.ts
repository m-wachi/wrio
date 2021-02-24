import moment, { isMoment } from 'moment';
import {WrioDate, WrioMap, WrioValue, WrioRecord} from './mdl01';

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

console.log(colHdrNames);

for(let i=0; i<recs.length; i++) {
  let colHdr = new WrioRecord();
  for(let j=0; j<colnames.length; j++) {
    
  }
  for(let k=0; k<rowHdrNmIdxs.length; k++) {
    colHdr.set(colnames[k], recs[i][colHdrNmIdxs[k]]);
    
  }
  console.log(colHdr)
}

