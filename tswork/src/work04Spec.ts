import {equals, WrioDate, WrioMap, WrioValue, WrioRecord, WrioRecordPair} from './model02';
import {getNameIndexPairs, vals2Rec, conv2Map, conv2Array2D} from './wriolib01';

//
// memo npm install --save-dev @types/jasmine
//


describe("work04Spec suite", function() {

  it("vals2Rec2 Test 01", function() {

    let rec = [new WrioDate("2019-07-01T00:00:00"),	"A0001", 10];
    let hdrNmIdxPairs : [string, number][] = [["sales_date", 0], ["item_cd", 1]];

    let hdr = vals2Rec(hdrNmIdxPairs, rec);

    expect(equals(hdr.get("sales_date"), new WrioDate("2019-07-01T00:00:00"))).toBeTrue();
    expect(hdr.get("item_cd")).toBe("A0001");
  });
/*
  it("conv2Map Test 01", function() {
    const recs : WrioValue[][] = [
      [new WrioDate("2019-07-01T00:00:00"),	"A0001", 10],
      [new WrioDate("2019-07-01T00:00:00"), "A0002", 15],
      [new WrioDate("2019-07-02T00:00:00"), "A0001", 8],
      [new WrioDate("2019-07-02T00:00:00"), "A0002", 12]
    ];

    const rowHdrNmIdxPairs: [string, number][] = [["sales_date", 0]];
    const colHdrNmIdxPairs: [string, number][] = [["item_cd", 1]];
    const valNmIdxPairs: [string, number][] = [["nof_sales", 2]];

    let [rowHdrSet, colHdrSet, dicPivotdata] = conv2Map(recs, rowHdrNmIdxPairs, colHdrNmIdxPairs, valNmIdxPairs);
  
    let ite = rowHdrSet.values();
    let rowHdr1 = ite.next().value;
    expect(equals(rowHdr1.get("sales_date"), new WrioDate("2019-07-01T00:00:00"))).toBeTrue();
    let rowHdr2 = ite.next().value;
    expect(equals(rowHdr2.get("sales_date"), new WrioDate("2019-07-02T00:00:00"))).toBeTrue();
  
    ite = colHdrSet.values();
    let colHdr1 = ite.next().value;
    expect(colHdr1.get("item_cd")).toBe("A0001");
    let colHdr2 = ite.next().value;
    expect(colHdr2.get("item_cd")).toBe("A0002");

    let hdrPair = new WrioRecordPair(rowHdr1, colHdr1);
    let cell = dicPivotdata.get(hdrPair);
    expect(cell.get("nof_sales")).toBe(10);
    hdrPair = new WrioRecordPair(rowHdr1, colHdr2);
    cell = dicPivotdata.get(hdrPair);
    expect(cell.get("nof_sales")).toBe(15);
    hdrPair = new WrioRecordPair(rowHdr2, colHdr1);
    cell = dicPivotdata.get(hdrPair);
    expect(cell.get("nof_sales")).toBe(8);
    hdrPair = new WrioRecordPair(rowHdr2, colHdr2);
    cell = dicPivotdata.get(hdrPair);
    expect(cell.get("nof_sales")).toBe(12);

  });

  //
  // WrioMap Test
  //
  it("conv2Array2D Test 01", function() {
    let rowHdrNames = ["sales_date"];
    let colHdrNames = ["item_cd"];
    let rowHdr1 = new WrioRecord();
    rowHdr1.set(rowHdrNames[0], new WrioDate("2019-07-01T00:00:00"));
    let rowHdr2 = new WrioRecord();
    rowHdr2.set(rowHdrNames[0], new WrioDate("2019-07-02T00:00:00"));
    let aryRowHdr = [rowHdr1, rowHdr2];

    let colHdr1 = new WrioRecord();
    colHdr1.set(colHdrNames[0], "A0001");
    let colHdr2 = new WrioRecord();
    colHdr2.set(colHdrNames[0], "A0002");
    let aryColHdr = [colHdr1, colHdr2];

    let valNames = ["nof_sales"];
    let dicPivotData = new WrioMap();

    const setPivotData = (idxRow: number, idxCol: number, v: number) :void=> {
      let vals = new WrioRecord();
      vals.set(valNames[0], v);
      dicPivotData.set(new WrioRecordPair(aryRowHdr[idxRow], aryColHdr[idxCol]), vals);
    }

    setPivotData(0, 0, 10);
    setPivotData(0, 1, 12);
    setPivotData(1, 0, 13);
    setPivotData(1, 1, 14);

    //console.log("=== dicPivotData start ===");
    //console.log(dicPivotData.toString());
    //console.log("=== dicPivotData end ===");

    let ret: WrioValue[][] = conv2Array2D(aryRowHdr, aryColHdr, rowHdrNames, colHdrNames, valNames, dicPivotData);

    expect(ret[0][1]).toBe("A0001");
    expect(ret[0][2]).toBe("A0002");
    expect(equals(ret[1][0], aryRowHdr[0].get("sales_date"))).toBeTrue();
    expect(ret[1][1]).toBe(10);
    expect(ret[1][2]).toBe(12);
    expect(equals(ret[2][0], aryRowHdr[1].get("sales_date"))).toBeTrue();
    expect(ret[2][1]).toBe(13);
    expect(ret[2][2]).toBe(14);

  });
  */
});