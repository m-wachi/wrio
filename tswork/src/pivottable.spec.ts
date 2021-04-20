import { WrioValue, WrioDate, WrioValueType } from './model02';
import { DsColumn } from './model';
import {PivotTableCell, PivotTableCells, getPivotTableFieldDef, PivotTableDateFieldDef, 
        PivotTableStringFieldDef, PtcMap, PtcsPair, PivotTableNumberFieldDef} from './pivottable';
import * as wl2 from './wriolib02';

describe("pivottable test suite", function() {

  const dsColumns : DsColumn[] = [
    {colName: "sales_date", colType: WrioValueType.DATE},
    {colName: "item_cd", colType: WrioValueType.STRING},
    {colName: "nof_sales", colType: WrioValueType.NUMBER}
  ];
  

  const recs : WrioValue[][] = [
    [new WrioDate("2019-07-01"),	"A0001", 10],
    [new WrioDate("2019-07-01"), "A0002", 15],
    [new WrioDate("2019-07-02"), "A0001", 8],
    [new WrioDate("2019-07-02"), "A0002", 12]
  ];
  
  
  it("getPivotTableFieldDef Test 01(Date)", function() {
    let ptfdDate1 = getPivotTableFieldDef(dsColumns[0].colName, dsColumns[0].colType);
    let wd1 = new WrioDate("2019-07-01T00:00:00");
    
    expect(ptfdDate1.fieldName).toBe("sales_date");
    expect(ptfdDate1.format(wd1)).toBe("2019-07-01");
  });

  it("PivotTableCells.valEquals Test 01", function() {
    const fieldName1 = "dateField";
    let wd11 = new WrioDate("2019-07-01");
    let wd12 = new WrioDate("2019-07-02");
    let wd21 = new WrioDate("2019-07-01");
    let wd22 = new WrioDate("2019-07-02");
    let dtfd1 = new PivotTableDateFieldDef(fieldName1);
    let dtfd2 = new PivotTableDateFieldDef(fieldName1);
  
    let ptc11 = new PivotTableCell(wd11, dtfd1);
    let ptc12 = new PivotTableCell(wd12, dtfd2);
    let ptc21 = new PivotTableCell(wd21, dtfd1);
    let ptc22 = new PivotTableCell(wd22, dtfd2);
 
    let ptcs1 = new PivotTableCells([ptc11, ptc12]);
    let ptcs2 = new PivotTableCells([ptc21, ptc22]);

    expect(ptcs1.valEquals(ptcs2)).toBe(true);
  });
  it("PivotTableCells.valEquals Test 02", function() {
    const fieldName1 = "dateField";
    let wd11 = new WrioDate("2019-07-01");
    let wd12 = new WrioDate("2019-07-02");
    let wd21 = new WrioDate("2019-07-01");
    let wd22 = new WrioDate("2019-07-03");
    let dtfd1 = new PivotTableDateFieldDef(fieldName1);
    let dtfd2 = new PivotTableDateFieldDef(fieldName1);
  
    let ptc11 = new PivotTableCell(wd11, dtfd1);
    let ptc12 = new PivotTableCell(wd12, dtfd2);
    let ptc21 = new PivotTableCell(wd21, dtfd1);
    let ptc22 = new PivotTableCell(wd22, dtfd2);
 
    let ptcs1 = new PivotTableCells([ptc11, ptc12]);
    let ptcs2 = new PivotTableCells([ptc21, ptc22]);

    expect(ptcs1.valEquals(ptcs2)).toBeFalse();
  });

  it("WrioMap Test 01", function() {
    let wd1 = new WrioDate("2019-07-01T00:00:00");
    let wd2 = new WrioDate("2019-07-01T00:00:00");
    let wd3 = new WrioDate("2019-07-02T00:00:00");

    //let wrMap = new WrioMap();
    let dtfd1 = new PivotTableDateFieldDef("date1");
    let ptc11 = new PivotTableCell(wd1, dtfd1);
    let ptc21 = new PivotTableCell(wd2, dtfd1);

    let sfd2 = new PivotTableStringFieldDef("item_cd");
    let ptc12 = new PivotTableCell("A001", sfd2);
    let ptc22 = new PivotTableCell("A002", sfd2);


    let nfd = new PivotTableNumberFieldDef("nof_sales");
    let ptc1v = new PivotTableCell(10, nfd);
    let ptc2v = new PivotTableCell(12, nfd);

    let rowHdr1 = new PivotTableCells([ptc11]);
    let colHdr1 = new PivotTableCells([ptc12]);
    let ptcsPair1 = new PtcsPair(rowHdr1, colHdr1);
    let ptcsv1 = new PivotTableCells([ptc1v]);

    let rowHdr2 = new PivotTableCells([ptc21]);
    let colHdr2 = new PivotTableCells([ptc22]);
    let ptcsPair2 = new PtcsPair(rowHdr2, colHdr2);

    let ptcMap = new PtcMap();
    ptcMap.set(ptcsPair1, ptcsv1);
    ptcMap.set(wd2, "wd2");  // overwrite wd1
    ptcMap.set(wd3, "wd3");

    expect(wrMap.get(wd1)).toBe("wd2");
    expect(wrMap.get(wd2)).toBe("wd2");
    expect(wrMap.get(wd3)).toBe("wd3");

  });


});