interface WrioDict<T> {
  [key: string] : T;
}


function func01(colNames : string[], idx: number, dic: any, rec: any) : any {
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
 * Extract Row/Col Header data.
 * @param hdrs row/col header
 * @param recs array of record
 */
function func02(hdrs: string[], recs: Array<any>) : Set<any> {

  let xs = recs.map(rec => {
    var x : WrioDict<any> = {};
    hdrs.forEach(hdr => {
      x[hdr] = rec[hdr];
    });
    return x;
  });
  let xSets = new Set(xs);

  return xSets;
}



console.log("Hello World.");

var recs = [];
recs.push({"sales_date":"2019-07-01T00:00:00","item_name":"アイテム０１","nof_sales":10});
recs.push({"sales_date":"2019-07-01T00:00:00","item_name":"アイテム０２","nof_sales":15});
recs.push({"sales_date":"2019-07-02T00:00:00","item_name":"アイテム０１","nof_sales":20});
recs.push({"sales_date":"2019-07-02T00:00:00","item_name":"アイテム０２","nof_sales":25});


var pvtColHdrs = ["item_name"];
var pvtRowHdrs = ["sales_date"];

var colNames = pvtColHdrs.concat(pvtRowHdrs);

console.log(colNames);

var d1 = {};
/*
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
*/

recs.forEach(rec => {
  func01(colNames, 0, d1, rec);
});

console.log("==== d1 ====");
console.log(d1);

console.log(Object.keys(d1));

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


