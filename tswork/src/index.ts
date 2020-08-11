function func01(colNames : string[], idx: number, endIdx: number, dic: any, rec: any) : any {
  console.log("idx=" + String(idx) + ", endIdx=" + String(endIdx));

  let sKey: string = String(rec[colNames[idx]]);

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
    func01(colNames, idx + 1, endIdx, dic[sKey], rec);
    return dic;
  }
}

console.log("Hello World.");

// {"sales_date":"2019-07-01T00:00:00","item_name":"アイテム０１","nof_sales":10}

/*
var rec1 = {"sales_date":"2019-07-01T00:00:00","item_name":"アイテム０１","nof_sales":10};
var rec2 = {"sales_date":"2019-07-01T00:00:00","item_name":"アイテム０２","nof_sales":15};
var rec3 = {"sales_date":"2019-07-02T00:00:00","item_name":"アイテム０１","nof_sales":20};
var rec4 = {"sales_date":"2019-07-02T00:00:00","item_name":"アイテム０２","nof_sales":25};
*/

var recs = [];
recs.push({"sales_date":"2019-07-01T00:00:00","item_name":"アイテム０１","nof_sales":10});
recs.push({"sales_date":"2019-07-01T00:00:00","item_name":"アイテム０２","nof_sales":15});
recs.push({"sales_date":"2019-07-02T00:00:00","item_name":"アイテム０１","nof_sales":20});
recs.push({"sales_date":"2019-07-02T00:00:00","item_name":"アイテム０２","nof_sales":25});


//console.log(rec1);

var colNames = ["sales_date", "item_name"];

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
  func01(colNames, 0, 1, d1, rec);
});

console.log("==== d1 ====");
console.log(d1);
