import { WrioValue, WrioDate } from './model02';
import {PivotTableCell, PivotTableCells} from './pivottable';

describe("pivottable test suite", function() {

  const recs : WrioValue[][] = [
    [new WrioDate("2019-07-01"),	"A0001", 10],
    [new WrioDate("2019-07-01"), "A0002", 15],
    [new WrioDate("2019-07-02"), "A0001", 8],
    [new WrioDate("2019-07-02"), "A0002", 12]
  ];
  
  
  //
  // WrioDate Test
  //
  it("WrioDate.equals Test 01", function() {
    let wd1 = new WrioDate("2019-07-01T00:00:00");
    let wd2 = new WrioDate("2019-07-01T00:00:00");

    expect(wd1.equals(wd2)).toBe(true);
  });
});