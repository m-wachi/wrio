function func01(colNames : string[], idx: number, endIdx: number, dic: any, rec: any) : any {
  console.log("idx=" + String(idx) + ", endIdx=" + String(endIdx));

  if (idx > endIdx) {
    return dic;
  } else if (idx === endIdx) {

    dic[rec[colNames[idx]]] = rec;
    return dic;
  } else {
    var d3 = null;
    var d2 = null;
    if (rec[colNames[idx]] in dic) {
      console.log("func01 path A");
      d2 = func01(colNames, idx + 1, endIdx, dic[rec[colNames[idx]]], rec);
    } else {
      console.log("func01 path B");
      d3 = {};
      dic[rec[colNames[idx]]] = d3;
      console.log(dic);
      d2 = func01(colNames, idx + 1, endIdx, dic[rec[colNames[idx]]], rec);
    }
    return dic;
  }
}

console.log("Hello World.");

// {"sales_date":"2019-07-01T00:00:00","item_name":"アイテム０１","nof_sales":10}

var rec1 = {"sales_date":"2019-07-01T00:00:00","item_name":"アイテム０１","nof_sales":10};

console.log(rec1);

var colNames = ["sales_date", "item_name"];

console.log(colNames);

var d1 = {};

var d2 = func01(colNames, 0, 1, d1, rec1);

console.log(d2);
