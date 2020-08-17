import moment, { isMoment } from 'moment';

interface WrioDict<T> {
  [key: string] : T;
}


function func01(colNames : string[], idx: number, dic: WrioDict<any>, rec: WrioDict<any>) : WrioDict<any> {
  //console.log("idx=" + String(idx));

  let sKey: string = String(rec[colNames[idx]]);
  let endIdx: number = colNames.length - 1;

  if (idx > endIdx) {
    return dic;
  } else if (idx === endIdx) {
    dic[sKey] = rec;
    return dic;
  } else {
    if (!(sKey in dic)) {
      //console.log("func01 path B");
      dic[sKey] = {};
      //console.log(dic);
    }
    func01(colNames, idx + 1, dic[sKey], rec);
    return dic;
  }
}

/**
 * apply func01 to array items
 * @param colNames 
 * @param recs 
 */
function func01s(colNames : string[], recs: WrioDict<any>[]) : WrioDict<any> {
  var d1 = {};
  recs.forEach(rec => {
    func01(colNames, 0, d1, rec);
  });

  return d1;
}



/**
 * Extract Row/Col Header data.
 * @param hdrs row/col header
 * @param recs array of record
 */
function func02(hdrs: string[], recs: WrioDict<any>[]) : WrioDict<any> {

  let xs = recs.map(rec => {
    var x : WrioDict<any> = {};
    hdrs.forEach(hdr => {
      x[hdr] = rec[hdr];
    });
    return x;
  });
  //let xSets = new Set(xs);

  return func01s(hdrs, xs);
}

/**
 * test Objects have same property value.
 * @param o1 
 * @param o2 
 * @param propNames property names compare
 */
function objValEq(o1: WrioDict<any>, o2: WrioDict<any>, propNames: string[]) : boolean {

  var i : number;

  for (i=0; i<propNames.length; i++) {
    let propName = propNames[i];
    console.log("propName = " + propName);
    console.log("o1 = " + o1[propName]);
    console.log("o2 = " + o2[propName]);
    if (o1[propName] !== o2[propName]) {
      return false;
    }
  }

  return true;
}

console.log("Hello World.");

var recs = [];
recs.push({"sales_date":moment("2019-07-01T00:00:00"),"item_name":"アイテム０１","nof_sales":10});
recs.push({"sales_date":moment("2019-07-01T00:00:00"),"item_name":"アイテム０２","nof_sales":15});
recs.push({"sales_date":moment("2019-07-02T00:00:00"),"item_name":"アイテム０１","nof_sales":20});
recs.push({"sales_date":moment("2019-07-02T00:00:00"),"item_name":"アイテム０２","nof_sales":25});
recs.push({"sales_date":moment("2019-07-01T00:00:00"),"item_name":"アイテム０１","nof_sales":3});

console.log("ismoment");
console.log(isMoment(recs[0]["sales_date"]));


var pvtColHdrs = ["item_name"];
var pvtRowHdrs = ["sales_date"];

var colNames = pvtColHdrs.concat(pvtRowHdrs);

console.log(colNames);

/*
var d1 = {};
var d2 = func01(colNames, 0, 1, d1, rec1);

console.log("==== d2 ====");
console.log(d2);
console.log("==== d1 ====");
console.log(d1);

var d2 = func01(colNames, 0, 1, d1, rec2);

console.log("==== d1 ====");
console.log(d1);

var d2 = func01(colNames, 0, 1, d1, rec3);
var d2 = func01(colNames, 0, 1, d1, rec4);

console.log("==== d1 ====");
console.log(d1);

recs.forEach(rec => {
  func01(colNames, 0, d1, rec);
});
*/

var pvt1 = func01s(colNames, recs);

console.log("==== pvt1 ====");
console.log(pvt1);

let k1s = recs.map(rec => {
  var a : WrioDict<any> = {};
  a["sales_date"] = rec["sales_date"];
  a["item_name"] = rec["item_name"];
  return a;
});

console.log("==== k1s ====");
console.log(k1s);

let k1Sets = new Set(k1s);
console.log("===== k1Sets ====");
console.log(k1Sets);

let k2Sets = func02(pvtRowHdrs, recs);
console.log("===== k2Sets ====");
console.log(k2Sets);

let k3Sets = func02(colNames, recs);
console.log("===== k3Sets ====");
console.log(k3Sets);
/*
let o1 = {"sales_date":"2019-07-01T00:00:00"};
let o2 = {"sales_date":"2019-07-01T00:00:00"};
let o3 = {"sales_date":"2019-07-02T00:00:00"};

console.log("objValEq(o1, o2) = " + String(objValEq(o1, o2, ["sales_date"])));
console.log("objValEq(o1, o3) = " + String(objValEq(o1, o3, ["sales_date"])));
console.log("o1 = " + String(o1["sales_date"]));
console.log("o3 = " + String(o3["sales_date"]));


let day1 = moment("2019-07-01T00:00:00");
console.log("day1 = [" + String(day1) + "]");
let o1a = {"sales_date": day1};
*/

var pvtColHdr2 = func02(pvtColHdrs, recs);
console.log("===== pvtColHdr2 ====");
console.log(pvtColHdr2);

var pvtRowHdr2 = func02(pvtRowHdrs, recs);
console.log("===== pvtRowHdr2 ====");
console.log(pvtRowHdr2);

console.log("-----------------------");
var s1 = "| |" + Object.keys(pvtColHdr2).join("|") + "|";

console.log(s1);

let pvtColHdr3 : any[] = Object.keys(pvtColHdr2);

/*
var colKey1 = Object.keys(pvtColHdr2)[0];
var colKey2 = Object.keys(pvtColHdr2)[1];
*/

var pvtRowHdr3 : any[] = Object.keys(pvtRowHdr2);
var rowKey1 = Object.keys(pvtRowHdr2)[0];

console.log("ismoment");
console.log(isMoment(pvtRowHdr3[0]));

s1 = "|"  + pvtRowHdr3[0] + "|";


let valProps = ["nof_sales"];
/*
var o1 = pvt1[colKey1][rowKey1];
s1 += o1[valProps[0]] + "|";
var o2 = pvt1[colKey2][rowKey1];
s1 += o2[valProps[0]] + "|";
*/
for (const colHdr of pvtColHdr3) {
  let o1 = pvt1[colHdr][pvtRowHdr3[0]];
  s1 += o1[valProps[0]] + "|";
}
console.log(s1);

s1 = "|"  + pvtRowHdr3[1] + "|";
for (const colHdr of pvtColHdr3) {
  let o1 = pvt1[colHdr][pvtRowHdr3[1]];
  s1 += o1[valProps[0]] + "|";
}


console.log(s1);
console.log("-----------------------");

