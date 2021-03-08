import {equals, WrioDate, WrioMap, WrioValue, WrioRecord, WrioRecordPair} from './mdl01';
import {getNameIndexPairs, vals2Rec, conv2Map, conv2Array2D} from './lib01';

//
// memo npm install --save-dev @types/jasmine
//


describe("A suite", function() {
  //
  // getNameIndexPairs Test
  //
  it("getNameIndexPairs Test 01", function() {

    const colnames = ["sales_date", "item_cd", "nof_sales"];
    const hdrNames = ["nof_sales", "item_cd"];
    const hdrNmIdxPairs : [string, number][] = getNameIndexPairs(hdrNames, colnames);
    
    const hdrNmIdx1 = hdrNmIdxPairs[0];
    const hdrNmIdx2 = hdrNmIdxPairs[1];

    expect(hdrNmIdx1[0]).toBe("nof_sales");
    expect(hdrNmIdx1[1]).toBe(2);

    expect(hdrNmIdx2[0]).toBe("item_cd");
    expect(hdrNmIdx2[1]).toBe(1);

  });

  it("vals2Rec Test 01", function() {

    let rec = [new WrioDate("2019-07-01T00:00:00"),	"A0001", 10];
    let hdrNmIdxPairs : [string, number][] = [["sales_date", 0], ["item_cd", 1]];

    let hdr = vals2Rec(hdrNmIdxPairs, rec);

    expect(equals(hdr.get("sales_date"), new WrioDate("2019-07-01T00:00:00"))).toBeTrue();
    expect(hdr.get("item_cd")).toBe("A0001");
  });

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

    let aryColHdr = ["A0001", "A0002"];
    let valNames = ["nof_sales"];
    let dicPivotData = new WrioMap();
    let rowHdr = new WrioRecord();
    rowHdr.set("sales_date", aryRowHdr[0]);
    let colHdr = new WrioRecord();
    colHdr.set("item_cd", aryColHdr[0]);
    dicPivotData.set(new WrioRecordPair(rowHdr, colHdr), 10);

    let ret: WrioValue[][] = conv2Array2D(aryRowHdr, aryColHdr, rowHdrNames, colHdrNames, valNames, dicPivotData);

    expect(ret[0][1]).toBe("A0001");
    expect(equals(ret[1][0], aryRowHdr[0])).toBeTrue();
    expect(ret[1][1]).toBe(10);

  });

  /*
  it("WrioRecord.equals() test 01", function() {
    let wrRec1 = new WrioRecord();
    wrRec1.set("k1", 5);
    wrRec1.set("k2", "strValue1");
    let wrRec2 = new WrioRecord();
    wrRec2.set("k1", 5);
    wrRec2.set("k2", "strValue2");
    let wrRec3 = new WrioRecord();
    wrRec3.set("k1", 5);
    wrRec3.set("k2", "strValue1");

    expect(wrRec1.equals(wrRec2)).toBe(false);
    expect(wrRec1.equals(wrRec3)).toBe(true);

  });
  */
});