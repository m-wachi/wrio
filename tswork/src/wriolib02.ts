import {WrioDate, WrioMap, WrioValue, WrioRecord, WrioRecordPair, WrioSet} from './model02';
import { PivotTableCell } from './pivottable';

export const getNameIndexPairs = (aryName: string[], aryColumnName: string[]) : [string, number][]=> {
  return aryName.map((nm) => [nm, aryColumnName.indexOf(nm)])
};

/*
export const vals2Rec = (nmIdxPairs: [string, number][], rec: WrioValue[]) => {
  let hdr = new WrioRecord();
  nmIdxPairs.forEach(([nm, nmIdx]) => {
    hdr.set(nm as string, rec[nmIdx as number]);
  });
  return hdr;
} 
*/

class PtcRecord {
  constructor(public nmIdxPairs: [string, number][], public aryPtc: PivotTableCell[]) {}

  get(fieldName: string) : PivotTableCell | undefined {
    const f = (x: [string, number]) => {
      if (x[0] === fieldName) return true;
      else false;
    }

    let filtered = this.nmIdxPairs.filter(f)
    if (0 === filtered.length) {
      return undefined;
    }
    else {
      const idx = filtered[0][1];
      return this.aryPtc[idx];
    }

  }
}


export const vals2PtcRec = (nmIdxPairs: [string, number][], aryPtc: PivotTableCell[]) => {
  /*
  let hdr = new WrioRecord();
  nmIdxPairs.forEach(([nm, nmIdx]) => {
    hdr.set(nm as string, aryPtc[nmIdx as number]);
  });
  return hdr;
  */
 return new PtcRecord(nmIdxPairs, aryPtc);
} 


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
}

// write test code!
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