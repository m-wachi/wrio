import * as utl1 from './util01';
import { DsColumn } from './model';
import {WrioDate, WrioMap, WrioValue, WrioRecord, WrioRecordPair, WrioSet} from './model02';
import { PivotTableCell, PivotTableCells, PtcsSet, PtcsPair, PtcMap, PivotTableStringFieldDef, getPivotTableFieldDef } from './pivottable';
import { PtcRecord } from './model03';

export const getNameIndexPairs = (aryName: string[], aryColumnName: string[]) : [string, number][]=> {
  return aryName.map((nm) => [nm, aryColumnName.indexOf(nm)])
};

export const getPtcFromPair = (pair: [DsColumn, WrioValue]) => {
  const [dsColumn, wv] = pair;
  let fd = getPivotTableFieldDef(dsColumn.colName, dsColumn.colType);
  return new PivotTableCell(wv, fd);
}

//
// convert WrioValue[][] to PivotTableCells[]
//
export const convAry2dWv2AryPtcs = (dsColumns1: DsColumn[], recs1: WrioValue[][]) => {
  return recs1.map((x) => {
    let c = utl1.myZip(dsColumns1, x);
    return new PivotTableCells(c.map(getPtcFromPair));
  });
}


/*
export const vals2Rec = (nmIdxPairs: [string, number][], rec: WrioValue[]) => {
  let hdr = new WrioRecord();
  nmIdxPairs.forEach(([nm, nmIdx]) => {
    hdr.set(nm as string, rec[nmIdx as number]);
  });
  return hdr;
} 
*/

/*
export const filterPtcs = (nmIdxPairs: [string, number][], ptcs: PivotTableCells) => {
  //let hdr = new WrioRecord();
  const f;

  ptc
  nmIdxPairs.forEach(([nm, nmIdx]) => {
    hdr.set(nm as string, rec[nmIdx as number]);
  });
  return hdr;
} 
*/
/*
export function conv2Map(
      recs: WrioValue[][], 
      rowHdrNmIdxPairs: [string, number][], colHdrNmIdxPairs: [string, number][], 
      valNmIdxPairs: [string, number][]) : [WrioSet, WrioSet, WrioMap] {
  let dicPivotData = new WrioMap();
  let colHdrSet = new WrioSet();
  let rowHdrSet = new WrioSet();
  
  for(const rec of recs) {
    let rowHdr = vals2Rec(rowHdrNmIdxPairs, rec);
    rowHdrSet.add(rowHdr);

    let colHdr = vals2Rec(colHdrNmIdxPairs, rec);
    colHdrSet.add(colHdr);
    
    let values = vals2Rec(valNmIdxPairs, rec);
  
    let wrp = new WrioRecordPair(rowHdr, colHdr);
    dicPivotData.set(wrp, values);
  
  }
  return [rowHdrSet, colHdrSet, dicPivotData];
} */

export function conv2Map2(
  ary2dPtCell: PivotTableCells[], 
  rowHdrNmIdxPairs: [string, number][], colHdrNmIdxPairs: [string, number][], 
  valNmIdxPairs: [string, number][]) : [PtcsSet, PtcsSet, PtcMap] {
  //let dicPivotData = new WrioMap();
  let dicPivotData = new PtcMap();
  //let colHdrSet = new WrioSet();
  //let rowHdrSet = new WrioSet();
  let colHdrSet = new PtcsSet();
  let rowHdrSet = new PtcsSet();

  for(const aryPtCell of ary2dPtCell) {
    //let rowHdr = vals2Rec(rowHdrNmIdxPairs, rec);
    //rowHdrSet.add(rowHdr);
    let rowHdrIdxs = rowHdrNmIdxPairs.map((x)=>{return x[1];});
    let rowHdr = aryPtCell.filterByFieldIndexes(rowHdrIdxs);
    rowHdrSet.add(rowHdr);

    //let colHdr = vals2Rec(colHdrNmIdxPairs, rec);
    //colHdrSet.add(colHdr);
    let colHdrIdxs = colHdrNmIdxPairs.map((x)=>{return x[1];});
    let colHdr = aryPtCell.filterByFieldIndexes(colHdrIdxs);
    colHdrSet.add(colHdr);

    //let values = vals2Rec(valNmIdxPairs, rec);
    let valIdxs = valNmIdxPairs.map((x)=>{return x[1];});
    let values = aryPtCell.filterByFieldIndexes(valIdxs);

    //let wrp = new WrioRecordPair(rowHdr, colHdr);
    //dicPivotData.set(wrp, values);
    let pp = new PtcsPair(rowHdr, colHdr);
    dicPivotData.set(pp, values);
  }
  return [rowHdrSet, colHdrSet, dicPivotData];
}

// write test code!
/*
export function conv2Array2D(
      aryRowHdr: WrioValue[], aryColHdr: WrioValue[], 
      rowHdrNames: string[], colHdrNames: string[], valNames: string[],
      dicPivotData: WrioMap) : WrioValue[][] {

  let retAry2d : WrioValue[][] = [];
  retAry2d.push(["_____"]);
  let idxRow = 0;
  
  for(let colHdr of aryColHdr) {
    const colHdr2 = colHdr as WrioRecord;
    for(const colHdrName of colHdrNames) {
      retAry2d[idxRow].push(colHdr2.get(colHdrName) as WrioValue);
    }
  }
  
  for(const rowHdr of aryRowHdr) {
    retAry2d.push([]);
    idxRow += 1;
    const rowHdr2 = rowHdr as WrioRecord;
    for(const rowHdrName of rowHdrNames) {
      retAry2d[idxRow].push(rowHdr2.get(rowHdrName) as WrioValue);
    }
    for(let colHdr of aryColHdr) {
  
      let vals = dicPivotData.get(new WrioRecordPair(rowHdr as WrioRecord, colHdr as WrioRecord));
      let vals2 = vals as WrioRecord;
      for(const valName of valNames) {
        retAry2d[idxRow].push(vals2.get(valName) as WrioValue);
      }
  
    }
  }
  
  return retAry2d;
}
*/

export function conv2Array2D2(
  aryRowHdr: PivotTableCells[], aryColHdr: PivotTableCells[], 
  rowHdrNames: string[], colHdrNames: string[], valNames: string[],
  dicPivotData: PtcMap) : PivotTableCell[][] {

  //let retAry2d : WrioValue[][] = [];
  let retAry2d : PivotTableCell[][] = [];
  //retAry2d.push(["_____"]);
  let dummyPtc = new PivotTableCell("_____", new PivotTableStringFieldDef("dummy"));
  retAry2d.push([dummyPtc]);
  let idxRow = 0;

  for(let colHdr of aryColHdr) {
    //const colHdr2 = colHdr as PivotTableCells;
    console.log("colHdr: " + colHdr.toString());
    for(const colHdrName of colHdrNames) {
      //retAry2d[idxRow].push(colHdr2.get(colHdrName) as WrioValue);
      let ptc1 = colHdr.getPtc(colHdrName);
      console.log("colHdrName=" + colHdrName + ", colHdr ptc1: " + ptc1?.toString());
      if (undefined !== ptc1) {
        retAry2d[idxRow].push(ptc1);
      }
    }
  }

  for(const rowHdr of aryRowHdr) {
    retAry2d.push([]);
    idxRow += 1;
    //const rowHdr2 = rowHdr as WrioRecord;
    for(const rowHdrName of rowHdrNames) {
      //retAry2d[idxRow].push(rowHdr2.get(rowHdrName) as WrioValue);
      let ptc1 = rowHdr.getPtc(rowHdrName);
      if (undefined !== ptc1) {
        retAry2d[idxRow].push(ptc1);
      }
    }
    for(let colHdr of aryColHdr) {
      //let vals = dicPivotData.get(new WrioRecordPair(rowHdr as WrioRecord, colHdr as WrioRecord));
      let vals = dicPivotData.get(new PtcsPair(rowHdr, colHdr));
      console.log("vals:" + vals?.toString());
      //let vals2 = vals as WrioRecord;
      if (vals === undefined) continue;
      for(const valName of valNames) {
        console.log("valName=" + valName);
        let ptc1 = vals.getPtc(valName);
        if (undefined !== ptc1) {
          retAry2d[idxRow].push(ptc1);
        }
      }
    }
  }

  return retAry2d;
}