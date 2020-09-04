import {WrioDate, WrioMap, WrioValue, WrioRecord} from './mdl01';

//
// memo npm install --save-dev @types/jasmine
//


describe("A suite", function() {
  //
  // WrioDate Test
  //
  it("WrioDate.equals Test 01", function() {
    let wd1 = new WrioDate("2019-07-01T00:00:00");
    let wd2 = new WrioDate("2019-07-01T00:00:00");

    expect(wd1.equals(wd2)).toBe(true);
  });
  it("WrioDate.equals Test 02", function() {
    let wd1 = new WrioDate("2019-07-01T00:00:00");
    let wd3 = new WrioDate("2019-07-02T00:00:00");

    expect(wd1.equals(wd3)).toBe(false);
  });

  //
  // WrioMap Test
  //
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

});