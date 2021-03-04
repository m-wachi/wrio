import {equals, WrioDate, WrioMap, WrioValue, WrioRecord} from './mdl01';
import {getNameIndexPairs, vals2Rec} from './lib01';

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

  //
  // WrioMap Test
  //
  /*
  it("WrioMap Test 01", function() {
    let wd1 = new WrioDate("2019-07-01T00:00:00");
    let wd2 = new WrioDate("2019-07-01T00:00:00");
    let wd3 = new WrioDate("2019-07-02T00:00:00");

    let wrMap = new WrioMap();
    wrMap.set(wd1, "wd1");
    wrMap.set(wd2, "wd2");  // overwrite wd1
    wrMap.set(wd3, "wd3");

    expect(wrMap.get(wd1)).toBe("wd2");
    expect(wrMap.get(wd2)).toBe("wd2");
    expect(wrMap.get(wd3)).toBe("wd3");

  });

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