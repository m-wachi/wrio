import { defaultCoreCipherList } from 'constants';
//import * as mmt from 'moment';
import { WrioDate, WrioMap, WrioValue, WrioRecord } from './model02';
import * as wl from './wriolib01';

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


const fieldNames = ["sales_date", "item_cd", "nof_sales"];

//const colnmIdxs = [0, 1, 2];

const recs : WrioValue[][] = [
  [new WrioDate("2019-07-01T00:00:00"),	"A0001", 10],
  [new WrioDate("2019-07-01T00:00:00"), "A0002", 15],
  [new WrioDate("2019-07-02T00:00:00"), "A0001", 8],
  [new WrioDate("2019-07-02T00:00:00"), "A0002", 12]
];

const rowNmIdxPairs = wl.getNameIndexPairs(["sales_date"], fieldNames);
const colNmIdxPairs = wl.getNameIndexPairs(["item_cd"], fieldNames);
const valNmIdxPairs = wl.getNameIndexPairs(["nof_sales"], fieldNames);

console.log("rowNmIdxPairs: " + rowNmIdxPairs.toString());


const [rowHdrSet1, colHdrSet1, dicVal] = wl.conv2Map(recs, rowNmIdxPairs, colNmIdxPairs, valNmIdxPairs);
console.log("rowHdrSet1: " + rowHdrSet1.toString());

console.log("dicVal: " + dicVal.toString());

const a = wl.conv2Array2D(rowHdrSet1.toArray(), colHdrSet1.toArray(), 
                          ["sales_date"], ["item_cd"], ["nof_sales"], dicVal);
console.log(a);                          