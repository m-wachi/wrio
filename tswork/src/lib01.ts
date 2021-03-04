import {WrioDate, WrioMap, WrioValue, WrioRecord, WrioRecordPair, WrioSet} from './mdl01';

export const getNameIndexPairs = (aryName: string[], aryColumnName: string[]) : [string, number][]=> {
  return aryName.map((nm) => [nm, aryColumnName.indexOf(nm)])
};

export const vals2Rec = (nmIdxPairs: [string, number][], rec: WrioValue[]) => {
  let hdr = new WrioRecord();
  nmIdxPairs.forEach(([nm, nmIdx]) => {
    hdr.set(nm as string, rec[nmIdx as number]);
  });
  return hdr;
} 
